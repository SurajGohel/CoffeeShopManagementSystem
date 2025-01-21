using CoffeeShopManagementSystem.Helper;
using CoffeeShopManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CoffeeShopManagementSystem.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        private IConfiguration configuration;

        public ProductController(IConfiguration _configuration)
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
        //    sqlCommand.CommandText = "usp_SelectAllProducts";
        //    SqlDataReader reader = sqlCommand.ExecuteReader();
        //    DataTable dataTable = new DataTable();
        //    dataTable.Load(reader);

        //    return View(dataTable);
        //}
        
        public IActionResult ProductList()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "usp_SelectAllProducts";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            return View(dataTable);
        }

        public IActionResult ProductSave(ProductModel productModel)
        {
            if (productModel.UserID <= 0)
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

                if (productModel.ProductID == null) 
                {
                    sqlCommand.CommandText = "PR_Product_Insert";
                }
                else
                {
                    sqlCommand.CommandText = "PR_Product_Update";
                    sqlCommand.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;
                }
                sqlCommand.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                sqlCommand.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                sqlCommand.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                sqlCommand.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
                sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;
                sqlCommand.ExecuteNonQuery();
                return RedirectToAction("ProductList");
            }
            return View("ProductAddEdit", productModel);
        }

        public IActionResult ProductAddEdit(string? ProductID)
        {
            int? decryptedProductID = null;
            if (!string.IsNullOrEmpty(ProductID))
            {
                string decryptedCityIDString = UrlEncryptor.Decrypt(ProductID); // Decrypt the encrypted CityID
                decryptedProductID = int.Parse(decryptedCityIDString); // Convert decrypted string to integer
            }

            //DropDown STARTS

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);

            List<ForProductUserDropDownModel> userList = new List<ForProductUserDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                ForProductUserDropDownModel userDropDownModel = new ForProductUserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                userList.Add(userDropDownModel);
            }
            ViewBag.UserList = userList;

            //DropDown END

            //SelectById STARTS
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Product_SelectByPK";
            command.Parameters.AddWithValue("@ProductID", decryptedProductID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            ProductModel productModel = new ProductModel();

            foreach (DataRow dataRow in table.Rows)
            {
                productModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                productModel.ProductName = @dataRow["ProductName"].ToString();
                productModel.ProductCode = @dataRow["ProductCode"].ToString();
                productModel.ProductPrice = Convert.ToDouble(@dataRow["ProductPrice"]);
                productModel.Description = @dataRow["Description"].ToString();
                productModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            return View("ProductAddEdit", productModel);
        }

        [HttpPost]
        public IActionResult ProductDelete(int ProductID)
        {
            int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(ProductID.ToString()));
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "usp_DeleteProduct";
                sqlCommand.Parameters.Add("@ProductID",SqlDbType.Int).Value = decryptedCityID;
                sqlCommand.ExecuteNonQuery();
                TempData["ProductDeleted"] = "Product deleted successfully.";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["ProductNotDeleted"] = "An error occurred while deleting the product: " ;

            }
            return RedirectToAction("ProductList");
        }
    }
}



