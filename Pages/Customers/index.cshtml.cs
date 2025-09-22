using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace crmapp.Pages.Customers
{
    public class index : PageModel
    {
        public List<CustomerInfo> CustomersList { get; set; } = [];

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=crmdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM customers ORDER BY Id DESC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerInfo customerInfo = new CustomerInfo();
                                customerInfo.Id = reader.GetInt32(0);
                                customerInfo.Firstname = reader.GetString(1);
                                customerInfo.Lastname = reader.GetString(2);
                                customerInfo.Email = reader.GetString(3);
                                customerInfo.Phone = reader.GetString(4);
                                customerInfo.Address = reader.GetString(5);
                                customerInfo.Company = reader.GetString(6);
                                customerInfo.Notes = reader.GetString(7);
                                customerInfo.CreatedAt = reader.GetDateTime(8).ToString("MM/dd/yyyy");
                                CustomersList.Add(customerInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("We have an error: " + ex.Message);
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=crmdb;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM customers WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete error: " + ex.Message);
            }
            return RedirectToPage();
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
            public string CreatedAt { get; set; } = "";
        }
    }
}