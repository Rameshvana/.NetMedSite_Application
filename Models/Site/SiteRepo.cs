using System.Data;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcLightphrm_Prject.Models.Site
{
    public class SiteRepo

    {
        private readonly string _ConnectionString;

        public SiteRepo(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public List<Site> GetSites()
        {
            List<Site> sites_List = new List<Site>();

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var command = new SqlCommand("Sp_FetchAllSites", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        sites_List.Add(new Site
                        {
                            SiteId = (int)reader["SiteId"],
                            SiteName = reader["SiteName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Address = reader["Address"].ToString(),
                            SiteCode = reader["SiteCode"].ToString(),
                            SiteType = reader["SiteType"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching sites: " + ex.Message);
                // You can also log this or rethrow it
            }

            return sites_List;
        }

        public string InsertSite(Site data)
        {

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("Sp_insertSite", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@SiteName", data.SiteName);
                    cmd.Parameters.AddWithValue("@Description", data.Description);
                    cmd.Parameters.AddWithValue("@Address", data.Address);
                    cmd.Parameters.AddWithValue("@SiteCode", data.SiteCode);
                    cmd.Parameters.AddWithValue("@SiteType", data.SiteType);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "Site added successfully...!";
                    else
                        return "Site not be adedd. please try again..";

                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while adding Site: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }



        }

        public string DeleteSite(int siteId)
        {
            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_deleteSiteById", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@SiteId", siteId);

                    int rowsAffected = query.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "Site deleted successfully..!";
                    else
                        return "Site not be deleted. try again.";

                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while deleting Site: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }
        }

        public string UpdateSite(Site data)
        {
            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_updateSite", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@SiteId", data.SiteId);
                    query.Parameters.AddWithValue("@SiteName", data.SiteName);
                    query.Parameters.AddWithValue("@Description", data.Description);
                    query.Parameters.AddWithValue("@Address", data.Address);
                    query.Parameters.AddWithValue("@SiteCode", data.SiteCode);
                    query.Parameters.AddWithValue("@SiteType", data.SiteType);

                    int rowsAffected = query.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "Site Updated successfully..!";
                    else
                        return "Site not be Updated. try again.";
                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while updating site: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }


        
        }




    }
}
