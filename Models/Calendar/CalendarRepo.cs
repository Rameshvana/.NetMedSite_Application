using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace MvcLightphrm_Prject.Models.Calendar
{
    public class CalendarRepo
    {
        private readonly string _ConnectionString;

        public CalendarRepo(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //=====================================================================================================================================================
        public string InsertSiteCalendar(Calendar data)
        {

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("Sp_insertSiteCalendar", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@CalendarUniqueId", data.CalendarUniqueId);
                    cmd.Parameters.AddWithValue("@SiteName", data.SiteName);
                    cmd.Parameters.AddWithValue("@Year", data.Year);
                    cmd.Parameters.AddWithValue("@UserName", data.UserName);
                    cmd.Parameters.AddWithValue("@ReportDate", data.ReportDate);
                    cmd.Parameters.AddWithValue("@SiteId", data.SiteId);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "Site Calender added successfully...!";
                    else
                        return "Site Calender not be adedd. please try again..";

                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while adding product: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }



        }

        public string InsertProductSiteCalendar(Calendar data)
        {

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("Sp_insertProductSiteCalendar", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@CalendarUniqueId", data.CalendarUniqueId);
                    cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                    cmd.Parameters.AddWithValue("@ApprovalDate", data.ApprovalDate);
                    cmd.Parameters.AddWithValue("@Year", data.Year);
                    cmd.Parameters.AddWithValue("@SiteName", data.SiteName);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "Site products Calender added successfully...!";
                    else
                        return "Site products Calender not be adedd. please try again..";

                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while adding product: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }



        }

        public List<Calendar> GetAllCalendarSites()
        {
            var productsList = new List<Calendar>();

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_getAllSiteCalendars", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    var rdr = query.ExecuteReader();

                    while (rdr.Read())
                    {


                        productsList.Add(new Calendar
                        {
                            //SiteName = reader["SiteName"].ToString(),
                            //Year = (int)reader["Year"],
                            //UserName = reader["UserName"].ToString(),
                            //SiteUniqueId = reader["SiteUniqueId"].ToString(),

                            CalendarUniqueId = rdr["CalendarUniqueId"].ToString(),
                            SiteName = rdr["SiteName"].ToString(),
                            Year = Convert.ToInt32(rdr["Year"]),
                            UserName = rdr["UserName"].ToString(),
                            ReportDate = Convert.ToDateTime(rdr["ReportDate"]),
                            SiteId = (int)rdr["SiteId"]
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching sitesCalendar: " + ex.Message);
            }

            return productsList;
        }

        public List<Calendar> GetSpecficProductsSites(string calId)
        {
            var productsList = new List<Calendar>();

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_getProductSiteCalendarById", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@CalendarUniqueId", calId);
                    var rdr = query.ExecuteReader();

                    while (rdr.Read())
                    {
                        productsList.Add(new Calendar
                        {
                            //SiteName = reader["SiteName"].ToString(),
                            //Year = (int)reader["Year"],
                            //UserName = reader["UserName"].ToString(),
                            //SiteUniqueId = reader["SiteUniqueId"].ToString(),

                            //CalendarUniqueId = rdr["CalendarUniqueId"].ToString(),
                            //SiteName = rdr["SiteName"].ToString(),
                            //Year = Convert.ToInt32(rdr["Year"]),
                            //UserName = rdr["UserName"].ToString(),
                            //ReportDate = Convert.ToDateTime(rdr["ReportDate"]),
                            //SiteId = (int)rdr["SiteId"]

                            CalendarUniqueId = rdr["CalendarUniqueId"].ToString(),
                            ProductName = rdr["ProductName"].ToString(),
                            ApprovalDate = Convert.ToDateTime(rdr["ApprovalDate"]),
                            Year = Convert.ToInt32(rdr["Year"]),
                            SiteName = rdr["SiteName"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching ProductssitesCalendar: " + ex.Message);
            }

            return productsList;
        }
    }
}
