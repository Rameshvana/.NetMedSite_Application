using Microsoft.Data.SqlClient;
using MvcLightphrm_Prject.Models.Site;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MvcLightphrm_Prject.Models.Product
{
    public class ProductRepo
    {
        private readonly string _ConnectionString;

        public ProductRepo(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Product> GetProducts()
        {
            List<Product> Products_List = new List<Product>();

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var command = new SqlCommand("Sp_FetchAllProducts", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Safely read the ApprovalDate, handle as string or DateTime

                        DateTime approvalDate = DateTime.MinValue;

                        if (reader["ApprovalDate"] != DBNull.Value)
                        {
                            approvalDate = Convert.ToDateTime(reader["ApprovalDate"]);
                        }
                        // Add the product to the list
                        Products_List.Add(new Product
                        {
                            ProductId = (int)reader["ProductId"],
                            SiteId = (int)reader["SiteId"],
                            ProductName = reader["ProductName"].ToString(),
                            ProductFullName = reader["ProductFullName"].ToString(),
                            SiteName = reader["SiteName"].ToString(),
                            ApprovalDate = approvalDate 
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching products: " + ex.Message);
            }

            return Products_List;
        }

        public string InsertProduct(Product data)
        {

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("Sp_insertProduct", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                    cmd.Parameters.AddWithValue("@ProductFullName", data.ProductFullName);
                    cmd.Parameters.AddWithValue("@SiteId", data.SiteId);
                    cmd.Parameters.AddWithValue("@ApprovalDate", data.ApprovalDate.Date); // Pass DateTime with just the date part

                    //cmd.Parameters.AddWithValue("@ApprovalDate", data.ApprovalDate.ToDateTime(new TimeOnly(0, 0)));
                    //cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                    //cmd.Parameters.AddWithValue("@ProductName", data.ProductName);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "product added successfully...!";
                    else
                        return "product not be adedd. please try again..";

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

        public string DeleteProduct(int productId)
        {
            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_deleteProduct", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    query.Parameters.AddWithValue("@ProductId", productId);
                    int rowsAffected = query.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "product deleted successfully..!";
                    else
                        return "prodect not be deleted. try again.";
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

        public string UpdateProduct(Product data)
        {
            

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_updateProduct", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@ProductId", data.ProductId);
                    query.Parameters.AddWithValue("@ProductName", data.ProductName);
                    query.Parameters.AddWithValue("@ProductFullName", data.ProductFullName);
                    query.Parameters.AddWithValue("@SiteId", data.SiteId);
                    query.Parameters.AddWithValue("@ApprovalDate", data.ApprovalDate.Date);

                    int rowsAffected = query.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return "product updated successfully..!";
                    else
                        return "prodect not be updated. try again.";
                }
            }
            catch (SqlException ex)
            {
                return "SQL Error while updating product: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Unexpected error: " + ex.Message;
            }

        }

        public List<Product> GetProductsBySiteId(int siteId)
        {
            var productsList = new List<Product>();

            try
            {
                using (var conn = new SqlConnection(_ConnectionString))
                {
                    conn.Open();
                    var query = new SqlCommand("Sp_FetchAllProductsBySiteId", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    query.Parameters.AddWithValue("@SiteId", siteId);

                    var reader = query.ExecuteReader();

                    //while (reader.Read())
                    //{
                    //    productsList.Add(new Product
                    //    {
                    //        ProductId = (int)reader["ProductId"],
                    //        SiteId = (int)reader["SiteId"],
                    //        ProductName = reader["ProductName"].ToString(),
                    //        SiteName = reader["SiteName"].ToString()
                    //    });
                    //}

                    while (reader.Read())
                    {

                        DateTime approvalDate = DateTime.MinValue;

                        if (reader["ApprovalDate"] != DBNull.Value)
                        {
                            approvalDate = Convert.ToDateTime(reader["ApprovalDate"]);
                        }
                        // Add the product to the list
                        productsList.Add(new Product
                        {
                            ProductId = (int)reader["ProductId"],
                            SiteId = (int)reader["SiteId"],
                            ProductName = reader["ProductName"].ToString(),
                            ProductFullName = reader["ProductFullName"].ToString(),
                            SiteName = reader["SiteName"].ToString(),
                            ApprovalDate = approvalDate
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching products: " + ex.Message);
            }

            return productsList;
        }



    }
}
