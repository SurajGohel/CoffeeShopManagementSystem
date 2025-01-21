using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CoffeeShopManagementSystem.Models;

namespace CoffeeShopManagementSystem.Controllers
{
    public class StateController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public StateController(IConfiguration configuration)
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
            objCmd.CommandText = "PR_LOC_State_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View(dt);
        }
        #endregion


        #region Delete
        public IActionResult Delete(int StateID)
        {
            string connectionstr = _configuration.GetConnectionString("ConnectionString");
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand sqlCommand = conn.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_LOC_State_Delete";
                    sqlCommand.Parameters.AddWithValue("@StateID", StateID);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region Add
        // This action displays the State Add/Edit form
        public IActionResult StateAddEdit(int? StateID)
        {
            // Load the dropdown list of countries
            LoadCountryList();

            // Check if an edit operation is requested
            if (StateID.HasValue)
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
                        objCmd.CommandText = "PR_LOC_State_SelectByPK";
                        objCmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;

                        using (SqlDataReader objSDR = objCmd.ExecuteReader())
                        {
                            dt.Load(objSDR); // Load data into DataTable
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    // Map data to CityModel
                    StateModel model = new StateModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.StateID = Convert.ToInt32(dr["StateID"]);
                        model.StateName = dr["StateName"].ToString();
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.StateCode = dr["StateCode"].ToString();
                    }
                    return View("StateAddEdit", model); // Return populated model to view
                }
            }

            return View("StateAddEdit"); // For adding a new city
        }
        #endregion



        #region Save
        // Save action handles both insert and update operations
        [HttpPost]
        public IActionResult Save(StateModel modelState)
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
                        if (modelState.StateID == null)
                        {
                            objCmd.CommandText = "PR_LOC_State_Insert";
                        }
                        else
                        {
                            objCmd.CommandText = "PR_LOC_State_Update";
                            objCmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelState.StateID;
                        }

                        // Pass parameters
                        objCmd.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelState.StateName;
                        objCmd.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelState.StateCode;
                        objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelState.CountryID;

                        objCmd.ExecuteNonQuery(); // Execute the query
                    }
                }

                //TempData["CityInsertMsg"] = "Record Saved Successfully"; // Success message
                return RedirectToAction("Index"); // Redirect to city listing
            }

            LoadCountryList(); // Reload dropdowns if validation fails
            return View("StateAddEdit", modelState);
        }
        #endregion


        #region LoadCountryList
        // Load the dropdown list of countries
        private void LoadCountryList()
        {
            string connectionstr = _configuration.GetConnectionString("ConnectionString");
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand objCmd = conn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_LOC_Country_SelectComboBox";

                    using (SqlDataReader objSDR = objCmd.ExecuteReader())
                    {
                        dt.Load(objSDR); // Load data into DataTable
                    }
                }
            }

            // Map data to list
            List<CountryDropDownModelForState> countryList = new List<CountryDropDownModelForState>();
            foreach (DataRow dr in dt.Rows)
            {
                countryList.Add(new CountryDropDownModelForState
                {
                    CountryID = Convert.ToInt32(dr["CountryID"]),
                    CountryName = dr["CountryName"].ToString()
                });
            }
            ViewBag.CountryList = countryList; // Pass list to view
        }
        #endregion
    }
}
