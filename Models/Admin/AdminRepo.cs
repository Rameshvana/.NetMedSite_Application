using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace MvcLightphrm_Prject.Models.Admin
{
    public class AdminRepo

    {
        private readonly string _ConnectionString;

        public AdminRepo(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Admin> GetAdmins()
        {
            var admins_List = new List<Admin>();
            try
            {
                using(var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_FetchAllAdmins", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    var reader = query.ExecuteReader();

                    while (reader.Read())
                    {


                        admins_List.Add(new Admin
                        {
                            AdminId = (int)reader["AdminId"],
                            AdminName = reader["AdminName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching admins: " + ex.Message);
            }

            return admins_List;
        }

        //public string GetSpecificAdmin(Admin data)
        //{
        //    Admin admin = null;

        //    string message = null;

        //    try
        //    {
        //        using (var conn = new SqlConnection(_ConnectionString))
        //        {
        //            conn.Open();
        //            var query = new SqlCommand("Sp_FetchAllAdminbyEmail", conn)
        //            {
        //                CommandType = CommandType.StoredProcedure
        //            };
        //            query.Parameters.AddWithValue("@Email", data.Email);


        //            var reader = query.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                admin = (new Admin
        //                {
        //                    AdminId = (int)reader["AdminId"],
        //                    AdminName = reader["AdminName"].ToString(),
        //                    Email = reader["Email"].ToString(),
        //                    Password = reader["Password"].ToString(),
        //                });
        //            }

        //            Debug.WriteLine(reader);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error fetching admins: " + ex.Message);
        //    }

        //    if(admin != null)
        //    {
        //        message = "Admin found in Db";
        //    }
        //    else
        //    {
        //        message = "Admin not found in Db";

        //    }
        //    return message;


        //}





        public string GetSpecificAdmin(Admin data)
        {
            Admin admin = null;
            string resultMessage;

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_FetchAllAdminbyEmail", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@Email", data.Email);

                    var reader = query.ExecuteReader();

                    if (reader.Read())
                    {
                        admin = new Admin
                        {
                            AdminId = (int)reader["AdminId"],
                            AdminName = reader["AdminName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                    }
                }

                if (admin == null)
                {
                    return "No account found with this email address";
                }

                if (admin.Password != data.Password)
                {
                    return "Password is incorrect Please try again with correct Password.";
                }

                return "True";
                //return "Your successfully Admin login, Thank You.";
                //string token = GenerateJwtToken(admin);
                //return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching admin: " + ex.Message);
                return "Error occurred while processing login";
            }
        }

    }
}
