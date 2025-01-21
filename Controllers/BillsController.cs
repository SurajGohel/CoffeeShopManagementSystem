using CoffeeShopManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CoffeeShopManagementSystem.Controllers
{
    public class BillsController : Controller
    {

        private IConfiguration configuration;

        public BillsController(IConfiguration _configuration)
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
        //    sqlCommand.CommandText = "usp_SelectAllBills";
        //    SqlDataReader reader = sqlCommand.ExecuteReader();
        //    DataTable dataTable = new DataTable();
        //    dataTable.Load(reader);

        //    return View(dataTable);
        //}

        public IActionResult BillsList()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "usp_SelectAllBills";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            return View(dataTable);
        }

        public IActionResult BillsSave(BillsModel billsModel)
        {
            if (billsModel.OrderID <= 0)
            {
                ModelState.AddModelError("OrderID", "A valid Order is required.");
            }
            if (billsModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                if (billsModel.BillID == null)
                {
                    sqlCommand.CommandText = "PR_Bills_Insert";
                }
                else
                {
                    sqlCommand.CommandText = "PR_Bills_Update";
                    sqlCommand.Parameters.Add("@BillID", SqlDbType.Int).Value = billsModel.BillID;
                }
                sqlCommand.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billsModel.BillNumber;
                sqlCommand.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billsModel.BillDate;
                sqlCommand.Parameters.Add("@OrderID", SqlDbType.Int).Value = billsModel.OrderID;
                sqlCommand.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billsModel.TotalAmount;
                sqlCommand.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billsModel.Discount;
                sqlCommand.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billsModel.NetAmount;
                sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = billsModel.UserID;
                sqlCommand.ExecuteNonQuery();
                return RedirectToAction("BillsList");
            }

            return View("BillsAddEdit", billsModel);
        }

        public IActionResult BillsAddEdit(int BillID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<ForBillsUserDropDownModel> userList = new List<ForBillsUserDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                ForBillsUserDropDownModel userDropDownModel = new ForBillsUserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                userList.Add(userDropDownModel);
            }
            ViewBag.UserList = userList;

            SqlConnection connection3 = new SqlConnection(connectionString);
            connection3.Open();
            SqlCommand command3 = connection3.CreateCommand();
            command3.CommandType = System.Data.CommandType.StoredProcedure;
            command3.CommandText = "PR_Order_DropDown";
            SqlDataReader reader3 = command3.ExecuteReader();
            DataTable dataTable3 = new DataTable();
            dataTable3.Load(reader3);

            List<ForOrderDetailOrderDropDownModel> orderList = new List<ForOrderDetailOrderDropDownModel>();
            foreach (DataRow data in dataTable3.Rows)
            {
                ForOrderDetailOrderDropDownModel orderDropDownModel = new ForOrderDetailOrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                orderDropDownModel.OrderDate = Convert.ToDateTime(data["OrderDate"]);
                orderList.Add(orderDropDownModel);
            }
            ViewBag.OrderList = orderList;



            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectByPK";
            command.Parameters.AddWithValue("@BillID", BillID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            BillsModel billsModel = new BillsModel();

            foreach (DataRow dataRow in table.Rows)
            {
                billsModel.BillID = Convert.ToInt32(@dataRow["BillID"]);
                billsModel.BillNumber = @dataRow["BillNumber"].ToString();
                billsModel.BillDate = Convert.ToDateTime(@dataRow["BillDate"]);
                billsModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                billsModel.TotalAmount = Convert.ToDouble(@dataRow["TotalAmount"]);
                billsModel.Discount = Convert.ToDouble(@dataRow["Discount"]);
                billsModel.NetAmount = Convert.ToDouble(@dataRow["NetAmount"]);
                billsModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            return View("BillsAddEdit", billsModel);
        }

        [HttpPost]
        public IActionResult BiilsDelete(int BillID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "usp_DeleteBill";
                sqlCommand.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                sqlCommand.ExecuteNonQuery();
                //TempData["SuccessMessage"] = "Bill deleted successfully.";
                TempData["BillsDeleted"] = "Bill deleted successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //TempData["ErrorMessage"] = "An error occurred while deleting the bill: " + ex.Message;
                TempData["BillsNotDeleted"] = "Bill deletion Failed!";
            }
            return RedirectToAction("BillsList");
        }
    }
}
