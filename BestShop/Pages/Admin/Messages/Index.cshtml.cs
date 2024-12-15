using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BestShop.Pages.Admin.Messages
{
    public class IndexModel : PageModel
    {
        public List<MessageInfo> listMessages = new List<MessageInfo>();

        //Pagination::
        public int page = 1;                // The current HTML page
        public int totalPages = 0;
        private readonly int pageSize = 5;  // Each html page size contains number of messages display on single page
        public void OnGet()
        {
            page = 1;
            string requestPage = Request.Query["page"];
            if (requestPage != null)
            {
                try
                {
                    page = int.Parse(requestPage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    page = 1;
                }
            }
                try
                {
					//string connectionString = "Data Source=.\\SQLEXPRESS01;Initial Catalog=bestshop;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                    string connectionString = "Data Source =.\\SQLEXPRESS01; Initial Catalog = bestshop; Integrated Security = True; Encrypt = True; TrustServerCertificate = True";

					using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sqlCount = "SELECT COUNT(*) FROM messages"; //for counting no of messages in database
                        using (SqlCommand command = new SqlCommand(sqlCount, connection))
                        {
                            decimal count = (int)command.ExecuteScalar();  //To Execute the SQL Query we need to call ExecuteScalar() method
							totalPages = (int)Math.Ceiling(count / (decimal) pageSize);
                        }


                        string sql = "SELECT * FROM messages ORDER BY id DESC";
                        sql += " OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY";

                        //----------------------------
						Console.WriteLine($"Request.Query[\"page\"]: {requestPage}");
						Console.WriteLine($"Calculated Page: {page}");

						using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@skip", (page - 1) * pageSize); //to skip previous page messages
                            command.Parameters.AddWithValue("@pageSize", pageSize);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MessageInfo messageInfo = new MessageInfo();
                                    messageInfo.Id = reader.GetInt32(0);
                                    messageInfo.FirstName = reader.GetString(1);
                                    messageInfo.LastName = reader.GetString(2);
                                    messageInfo.Email = reader.GetString(3);
                                    messageInfo.Phone = reader.GetString(4);
                                    messageInfo.Subject = reader.GetString(5);
                                    messageInfo.Message = reader.GetString(6);
                                    messageInfo.CreatedAt = reader.GetDateTime(7).ToString("MM/dd/yyyy");

                                    listMessages.Add(messageInfo);

                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            
        }
    }
        public class MessageInfo
        {
            public int Id { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Subject { get; set; } = "";
            public string Message { get; set; } = "";
            public string CreatedAt { get; set; } = "";

        }
    
}
