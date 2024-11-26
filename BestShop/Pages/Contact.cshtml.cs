using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BestShop.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {

        }

		//Can achieve this in single line as well
		//[BindProperty,Required(ErrorMessage="The First Name is Required")]

		//Properties for storing form data
		[BindProperty]  
        [Required(ErrorMessage="The First Name is Required")]
        [Display(Name ="First Name*")]
        public string FirstName {  get; set; } =string.Empty;
		[BindProperty]
		[Required(ErrorMessage = "The Last Name is Required")]
		[Display(Name = "Last Name*")]
		public string LastName { get; set; } = string.Empty;
		[BindProperty]
		[Required(ErrorMessage = "The Email is Required")]
        [EmailAddress]
		[Display(Name = "Email*")]
		public string Email { get; set; } = string.Empty;
		[BindProperty]
		public string? Phone { get; set; } = string.Empty; //Phone can be NULL
		[BindProperty]
		[Required(ErrorMessage = "The Subject is Required")]
		[Display(Name = "Subject*")]
		public string Subject { get; set; } = string.Empty;
		[BindProperty]
		[Required(ErrorMessage = "The message is Required")]
        [MinLength(5,ErrorMessage="The message should be of atleast 5 characters")]
        [MaxLength(1024,ErrorMessage="The message should be less than 1024 characters")]
		[Display(Name = "Message*")]
		public string Message { get; set; } = string.Empty;

        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnPost()
        {
            //Read and validate the form data using traditional method
            FirstName = Request.Form["firstname"];
            LastName = Request.Form["lastname"];
            Email = Request.Form["email"];
            Phone = Request.Form["phone"];
            Subject = Request.Form["subject"];
            Message = Request.Form["message"];

            if(FirstName.Length==0 || LastName.Length==0 || Email.Length==0 || Phone.Length==0 || Subject.Length==0 || Message.Length == 0)
            {
                //Error
                ErrorMessage = "Please fill all required fields";
                return;
            }

            //Add this message to the database
            //Send Confirmation Email to the client
            SuccessMessage = "Your message has been received correctly";
            FirstName=string.Empty;
            LastName=string.Empty;
            Email=string.Empty;
            Phone=string.Empty;
            Subject=string.Empty;
            Message=string.Empty;


        }
    }
}
