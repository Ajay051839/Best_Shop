using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

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

        public List<SelectListItem> SubjectList { get; } = new List<SelectListItem>
        {
            new SelectListItem{Value="Order Status", Text="Order Status"},
            new SelectListItem{Value="Refund Request", Text="Refund Request"},
            new SelectListItem{Value="Job Application", Text="Job Application"},
            new SelectListItem{Value="Other",Text="Other"},
        };

        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnPost()
        {
			/* 
            //Read and validate the form data using traditional method
            FirstName = Request.Form["firstname"];
            LastName = Request.Form["lastname"];
            Email = Request.Form["email"];
            Phone = Request.Form["phone"];
            Subject = Request.Form["subject"];
            Message = Request.Form["message"];
            */

/* if(FirstName.Length==0 || LastName.Length==0 || Email.Length==0 || Phone.Length==0 || Subject.Length==0 || Message.Length == 0)
{
	//Error
	ErrorMessage = "Please fill all required fields";
	return;
} */

if(!ModelState.IsValid)
{
	//Error
	ErrorMessage = "Please fill all required fields";
	return;
}

 //Since Phone number is optional need to check for Database update
 if (Phone == string.Empty) Phone = "";

            //Add this message to the database
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS01;Initial Catalog=bestshop;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO messages " +
                        "(firstname ,lastname, email, phone, subject, message) VALUES " +
                        "(@firstname, @lastname, @email, @phone, @subject, @message);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstname",FirstName);
						command.Parameters.AddWithValue("@lastname", LastName);
						command.Parameters.AddWithValue("@email", Email);
						command.Parameters.AddWithValue("@phone", Phone);
						command.Parameters.AddWithValue("@subject", Subject);
						command.Parameters.AddWithValue("@message", Message);

                        command.ExecuteNonQuery();
					}
            }
            }
            catch (Exception ex) 
            {
                //Error
                ErrorMessage = ex.Message;
                return;
            }

//Send Confirmation Email to the client

SuccessMessage = "Your message has been received correctly";
FirstName=string.Empty;
LastName=string.Empty;
Email=string.Empty;
Phone=string.Empty;
Subject=string.Empty;
Message=string.Empty;

ModelState.Clear(); //If this is not given then these variable displays the old values

}
}
}
