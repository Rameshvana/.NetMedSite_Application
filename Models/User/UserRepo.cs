using Microsoft.Data.SqlClient;
using System.Data;

namespace MvcLightphrm_Prject.Models.User
{
    public class UserRepo
    {
        private readonly string _ConnectionString;

        public UserRepo(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<User> GetUsers()
        {
            var users_List = new List<User>();
            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_FetchAllUsers", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    var reader = query.ExecuteReader();

                    while (reader.Read())
                    {
                        users_List.Add(new User
                        {
                            UserId = (int)reader["UserId"],
                            UserName = reader["UserName"].ToString(),
                            UserFullName = reader["UserFullName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            SiteName = reader["SiteName"].ToString(),
                            ProductName= reader["ProductName"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching users: " + ex.Message);
                // You can also log this or rethrow it
            }

            return users_List;




        }

        public User GetSpecificUser(int userId)
        {
            User user = null;
            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_FetchUserById", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@UserId", userId);
             
                    var reader = query.ExecuteReader();

                    while (reader.Read())
                    {
                         user = (new User
                        {
                            UserId = (int)reader["UserId"],
                             SiteId = (int)reader["SiteId"],
                             UserName = reader["UserName"].ToString(),
                            UserFullName = reader["UserFullName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            SiteName = reader["SiteName"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching users: " + ex.Message);
                // You can also log this or rethrow it
            }

            return user;




        }

        public string InsertUser(User data)
        {

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_insertUser", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    query.Parameters.AddWithValue("@UserName", data.UserName);
                    query.Parameters.AddWithValue("@UserFullName", data.UserFullName);
                    query.Parameters.AddWithValue("@Description", data.Description);
                    query.Parameters.AddWithValue("@Email", data.Email);
                    query.Parameters.AddWithValue("@Password", data.Password);
                    query.Parameters.AddWithValue("@SiteId", data.SiteId);
                    query.Parameters.AddWithValue("@ProductId", data.ProductId);
                 

                    int rowsAffected = query.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "user added successfully...!";
                    else
                        return "user not be adedd. please try again..";

                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while adding user: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }



        }

        public string DeleteUser(int userId)
        {
            try
            {
                using(var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_deleteUser", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = query.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "user deleted successfully..!";
                    else
                        return "user not be deleted. try again.";
                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while deleting user: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }
        }

        public string UpdateUser(User data)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    connection.Open();
                    var query = new SqlCommand("Sp_updateUser", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@UserId", data.UserId);
                    query.Parameters.AddWithValue("@UserName", data.UserName);
                    query.Parameters.AddWithValue("@UserFullName", data.UserFullName);
                    query.Parameters.AddWithValue("@Description", data.Description);
                    query.Parameters.AddWithValue("@Email", data.Email);
                    query.Parameters.AddWithValue("@Password", data.Password);
                    query.Parameters.AddWithValue("@SiteId", data.SiteId);
                    query.Parameters.AddWithValue("@ProductId", data.ProductId);

                    int rowsAffected = query.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "user updated successfully..!";
                    else
                        return "user not be updated. try again.";
                }

            }
            catch (SqlException ex)
            {
                return "SQL Error while updating user: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }


        }




    }
}
