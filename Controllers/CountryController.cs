using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CoffeeShopManagementSystem.Models;

namespace CoffeeShopManagementSystem.Controllers
{
    public class CountryController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            string connectionstr = this._configuration.GetConnectionString("ConnectionString");
            //PrePare a connection
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            //Prepare a Command
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();  
            dt.Load(objSDR);
            conn.Close();
            return View(dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            string connectionstr = _configuration.GetConnectionString("ConnectionString");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_LOC_Country_Delete";
                    sqlCommand.Parameters.AddWithValue("@CountryID", CountryID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(CountryModel modelCountry)
        {
            if (ModelState.IsValid)
            {
                string connectionstr = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    using (SqlCommand objCmd = conn.CreateCommand())
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;

                        // Choose procedure based on operation (insert or update)
                        if (modelCountry.CountryID == null)
                        {
                            objCmd.CommandText = "PR_LOC_Country_Insert";
                        }
                        else
                        {
                            objCmd.CommandText = "PR_LOC_Country_Update";
                            objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelCountry.CountryID;
                        }

                        // Pass parameters
                        objCmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelCountry.CountryName;
                        objCmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelCountry.CountryCode;

                        objCmd.ExecuteNonQuery(); // Execute the query
                    }
                }

                //TempData["CityInsertMsg"] = "Record Saved Successfully"; // Success message
                return RedirectToAction("Index"); // Redirect to city listing
            }

            return View("CountryAddEdit", modelCountry);
        }
        #endregion


        #region Add
        public IActionResult CountryAddEdit(int? CountryID)
        {

            // Check if an edit operation is requested
            if (CountryID.HasValue)
            {
                string connectionstr = _configuration.GetConnectionString("ConnectionString");
                DataTable dt = new DataTable();

                // Fetch city details by ID
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    using (SqlCommand objCmd = conn.CreateCommand())
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "PR_LOC_Country_SelectByPK";
                        objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;

                        using (SqlDataReader objSDR = objCmd.ExecuteReader())
                        {
                            dt.Load(objSDR); // Load data into DataTable
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    // Map data to CityModel
                    CountryModel model = new CountryModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.CountryName = dr["CountryName"].ToString();
                        model.CountryCode = dr["CountryCode"].ToString();
                    }
                    return View("CountryAddEdit", model); // Return populated model to view
                }
            }

            return View("CountryAddEdit"); // For adding a new city
        }
        #endregion
    }
}
