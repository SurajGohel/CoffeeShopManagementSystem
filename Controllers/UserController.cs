using CoffeeShopManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CoffeeShopManagementSystem.Controllers
{
    public class UserController : Controller
    {

        private IConfiguration configuration;

        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        //public IActionResult Index()
        //{
        //    string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //    SqlConnection sqlConnection = new SqlConnection(connectionString);
        //    sqlConnection.Open();
        //    SqlCommand sqlCommand = sqlConnection.CreateCommand();
        //    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //    sqlCommand.CommandText = "usp_SelectAllUsers";
        //    SqlDataReader reader = sqlCommand.ExecuteReader();
        //    DataTable dataTable = new DataTable();
        //    dataTable.Load(reader);

        //    return View(dataTable);
        //}

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult UserRegister(UserRegisterModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Register";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                    sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                    sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userRegisterModel.MobileNo;
                    sqlCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = userRegisterModel.Address;
                    sqlCommand.ExecuteNonQuery();
                    return RedirectToAction("Login", "User");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Register");
            }
            return RedirectToAction("Register");
        }

        public IActionResult Login()
        {
            return View();  
        }

        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Login";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                            TempData["User"] = dr["UserName"];
                        }
                        
                        return RedirectToAction("ProductList", "Product");
                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }

        //public IActionResult Login(UserLoginModel userLoginModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //            SqlConnection sqlConnection = new SqlConnection(connectionString);
        //            sqlConnection.Open();
        //            SqlCommand sqlCommand = sqlConnection.CreateCommand();
        //            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //            sqlCommand.CommandText = "PR_User_Login";
        //            sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
        //            sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
        //            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        //            DataTable dataTable = new DataTable();
        //            dataTable.Load(sqlDataReader);
        //            foreach (DataRow dr in dataTable.Rows)
        //            {
        //                HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
        //            }

        //            return RedirectToAction("ProductList", "Product");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["ErrorMessage"] = e.Message;
        //    }

        //    return RedirectToAction("Login");
        //}

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        public IActionResult UserList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "usp_SelectAllUsers";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            return View(dataTable);
        }

        public IActionResult UserSave(UserModel userModel)
        {

            //if (ModelState.IsValid)
            //{
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                if (userModel.UserID == null)
                {
                    sqlCommand.CommandText = "PR_User_Insert";
                }
                else
                {
                    sqlCommand.CommandText = "PR_User_Update";
                    sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;
                }
                sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
                sqlCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
                sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;
                sqlCommand.ExecuteNonQuery();
                return RedirectToAction("UserList");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
            return View("UserAddEdit", userModel);
        }

        public IActionResult UserAddEdit(int UserID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectByPK";
            command.Parameters.AddWithValue("@UserID", UserID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            UserModel userModel = new UserModel();

            foreach (DataRow dataRow in table.Rows)
            {
                userModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                userModel.UserName = @dataRow["UserName"].ToString();
                userModel.Email = @dataRow["Email"].ToString();
                userModel.Password = @dataRow["Password"].ToString();
                userModel.MobileNo = @dataRow["MobileNo"].ToString();
                userModel.Address = @dataRow["Address"].ToString();
                userModel.IsActive = Convert.ToBoolean(@dataRow["IsActive"]);
            }

            return View("UserAddEdit", userModel);
        }

        [HttpPost]
        public IActionResult UserDelete(int UserID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "usp_DeleteUser";
                sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                sqlCommand.ExecuteNonQuery();
                TempData["UserDeleted"] = "User deleted successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.ShowError = true;
                Console.WriteLine(ex.ToString());
                TempData["UserNotDeleted"] = "Unable to delete this record";
            }
            return RedirectToAction("UserList");
        }
    }
}
