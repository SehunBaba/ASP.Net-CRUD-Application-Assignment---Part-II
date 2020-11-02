using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAssignmentPart2.Additional_Code;
using DatabaseAssignmentPart2.Models;
using DatabaseAssignmentPart2.PasswordSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DatabaseAssignmentPart2.Controllers
{
    public class UserController : Controller
    {
        private bool isLoggedIn = LoggedIn.IsLoggedIn;

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IConfiguration Configuration { get; }

        public UserController(IConfiguration configuration)
        {
            Configuration = configuration;
            isLoggedIn = false;
        }


        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Incorrect()
        {
            return View();
        }


        public IActionResult SucessfullyRegistered()
        {
            return View();
        }

        public IActionResult NotLoggedIn()
        {
            return View();
        }


        [HttpPost]

        public IActionResult RegisterAccount(User user)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    user.Password = SecurePasswordHasher.Hash(user.Password);
                    user.ConfirmPassword = SecurePasswordHasher.Hash(user.ConfirmPassword);

                    string sql = $"Insert Into Users (FirstName, LastName, Email, DateOfBirth, Password) Values ('{user.FirstName }', '{user.LastName}','{user.Email}', '{user.DateOfBirth}', '{user.ConfirmPassword}')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    return RedirectToAction("SucessfullyRegistered");
                }
            }
            else
                return View();

        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login( string email, string password)
        {
            // Get the database connection details to connect to.
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            User user = new User();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * From Users Where Email ='{email}'";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    
                    while (dataReader.Read())
                    {


                        user.Password = Convert.ToString(dataReader["Password"]);

                        if (SecurePasswordHasher.Verify(password, user.Password))
                        {
                            LoggedIn.SetLoggedIn(true);
                           // isLoggedIn = true;
                            
                            return RedirectToAction("Index", "Home");
                        }

                        else
                        {
                            return RedirectToAction("Incorrect");
                        }


                    }

                    
                }
                //Making sure to close the connection!
                connection.Close();
            }
            //List the records as the model!
            return RedirectToAction("Incorrect");





        }
    }

}