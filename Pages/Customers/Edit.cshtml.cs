using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace crmapp.Pages.Customers
{
    public class Edit : PageModel
    {
        [BindProperty]
        public CustomerInfo Customer { get; set; } = new CustomerInfo();

        public IActionResult OnGet(int id)
        {
            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=crmdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM customers WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Customer.Id = reader.GetInt32(0);
                                Customer.Firstname = reader.GetString(1);
                                Customer.Lastname = reader.GetString(2);
                                Customer.Email = reader.GetString(3);
                                Customer.Phone = reader.GetString(4);
                                Customer.Address = reader.GetString(5);
                                Customer.Company = reader.GetString(6);
                                Customer.Notes = reader.GetString(7);
                            }
                            else
                            {
                                return RedirectToPage("index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Edit Get error: " + ex.Message);
                return RedirectToPage("index");
            }
            return Page();
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
                    string sql = @"UPDATE customers SET 
                        Firstname=@Firstname, Lastname=@Lastname, Email=@Email, Phone=@Phone, 
                        Address=@Address, Company=@Company, Notes=@Notes WHERE Id=@Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", Customer.Firstname);
                        command.Parameters.AddWithValue("@Lastname", Customer.Lastname);
                        command.Parameters.AddWithValue("@Email", Customer.Email);
                        command.Parameters.AddWithValue("@Phone", Customer.Phone);
                        command.Parameters.AddWithValue("@Address", Customer.Address);
                        command.Parameters.AddWithValue("@Company", Customer.Company);
                        command.Parameters.AddWithValue("@Notes", Customer.Notes);
                        command.Parameters.AddWithValue("@Id", Customer.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Edit Post error: " + ex.Message);
                return Page();
            }
            return RedirectToPage("index");
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