using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace crmapp.Pages.Customers
{
    public class Create : PageModel
    {
        [BindProperty]
        public CustomerInfo Customer { get; set; } = new CustomerInfo();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=crmdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO customers 
                        (Firstname, Lastname, Email, Phone, Address, Company, Notes, CreatedAt) 
                        VALUES (@Firstname, @Lastname, @Email, @Phone, @Address, @Company, @Notes, @CreatedAt)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", Customer.Firstname);
                        command.Parameters.AddWithValue("@Lastname", Customer.Lastname);
                        command.Parameters.AddWithValue("@Email", Customer.Email);
                        command.Parameters.AddWithValue("@Phone", Customer.Phone);
                        command.Parameters.AddWithValue("@Address", Customer.Address);
                        command.Parameters.AddWithValue("@Company", Customer.Company);
                        command.Parameters.AddWithValue("@Notes", Customer.Notes);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create error: " + ex.Message);
                return Page();
            }
            return RedirectToPage("./index");
        }

        public class CustomerInfo
        {
            public int Id { get; set; }
            public string Firstname { get; set; } = "";
            public string Lastname { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Address { get; set; } = "";
            public string Company { get; set; } = "";
            public string Notes { get; set; } = "";
        }
    }
}