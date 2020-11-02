using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DatabaseAssignmentPart2.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using DatabaseAssignmentPart2.Additional_Code;

namespace DatabaseAssignmentPart2.Controllers
{
    public class HomeController : Controller
    {
       // private bool isLoggedIn = LoggedIn.IsLoggedIn;

        public IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
            

        }

        public IActionResult LogOut()
        {
            LoggedIn.SetLoggedIn(false);
            return View();
        }

       
        public IActionResult Index()
        {
            if( LoggedIn.getLoggedIn() == true )
            { 
            List<Records> recordList = new List<Records>();
            // Get the database connection details to connect to.
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SQL Data reader
                connection.Open();

                string sql = "Select * From Prescriptions";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    //Loop through columns in table
                    while (dataReader.Read())
                    {
                        //Map the objects to the columns, add them to the record variable  
                        Records record = new Records();
                        record.Id = Convert.ToInt32(dataReader["Id"]);
                        record.FirstName = Convert.ToString(dataReader["FirstName"]);
                        record.LastName = Convert.ToString(dataReader["LastName"]);
                        record.Email = Convert.ToString(dataReader["Email"]);
                        record.Doctor = Convert.ToString(dataReader["Doctor"]);
                        record.Medication = Convert.ToString(dataReader["Medication"]);
                        record.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);
                        record.PickupDate = Convert.ToDateTime(dataReader["PickupDate"]);
                        recordList.Add(record);
                    }
                }
                //Making sure to close the connection!
                connection.Close();
            }

            //List the records as the model!
            return View(recordList);
            }

            else
            {
                return RedirectToAction("NotLoggedIn", "User");
            }
        }

        
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateRecord(Records record)
        {


            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into Prescriptions (FirstName, LastName, Email, Doctor, Medication, PickupDate) Values ('{record.FirstName }', '{record.LastName}','{record.Email}', '{record.Doctor}', '{record.Medication}', '{record.PickupDate}')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    return RedirectToAction("Index");
                }
            }
            else
                return View();

        }



        public IActionResult Update(int Id)
        {
            // Get the database connection details to connect to.
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            Records record = new Records();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * From Prescriptions Where Id ='{Id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    //Loop through columns in table
                    while (dataReader.Read())
                    {
                        //Map the objects to the columns, add them to the record variable  

                        record.Id = Convert.ToInt32(dataReader["Id"]);
                        record.FirstName = Convert.ToString(dataReader["FirstName"]);
                        record.LastName = Convert.ToString(dataReader["LastName"]);
                        record.Email = Convert.ToString(dataReader["Email"]);
                        record.Doctor = Convert.ToString(dataReader["Doctor"]);
                        record.Medication = Convert.ToString(dataReader["Medication"]);
                        record.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);
                        record.PickupDate = Convert.ToDateTime(dataReader["PickupDate"]);

                    }
                }
                //Making sure to close the connection!
                connection.Close();
            }
            //List the records as the model!
            return View(record);


        }




        [HttpPost]
        [ActionName("Update")]
        public IActionResult UpdateRecord(Records record)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Prescriptions SET FirstName='{record.FirstName}', LastName='{record.LastName}', Email='{record.Email}', Doctor ='{record.Doctor}', Medication ='{record.Medication}', PickupDate ='{record.PickupDate}' Where Id='{record.Id}'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            // Get the database connection details to connect to.
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Delete From Prescriptions Where Id='{Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }

                    catch (SqlException exception)
                    {
                        ViewBag.Result = "Operation Error:" + exception.Message;
                    }
                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }

    }

}
