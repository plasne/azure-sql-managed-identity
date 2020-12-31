using System;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;

namespace sql_test
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // get an access token for an Azure SQL DB
            var tokenProvider = new AzureServiceTokenProvider();
            string accessToken = await tokenProvider.GetAccessTokenAsync("https://database.windows.net");

            // example connection string
            string connstring = "Server=tcp:pelasne-sql-server.database.windows.net,1433;Initial Catalog=pelasne-sql-db;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // connect to the database with the token
            SqlConnection conn = new SqlConnection(connstring);
            conn.AccessToken = accessToken;
            conn.Open();

            // run a sample query
            var query = "SELECT firstName, lastName FROM dbo.Person;";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                    }
                }
            }

        }
    }
}
