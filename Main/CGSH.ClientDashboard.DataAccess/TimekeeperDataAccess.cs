using CGSH.ClientDashboard.Interface.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CGSH.ClientDashboard.BusinessEntitity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CGSH.ClientDashboard.DataAccess
{
    public class TimekeeperDataAccess : ITimekeeperDataAccess
    {
        ExceptionManager _exceptionManager;
        string connectionString;
        string asyncConnectionString;

        /// <summary>
        /// Default Constructor
        /// </summary>
        private TimekeeperDataAccess()
        {
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="exceptionManager"></param>
        public TimekeeperDataAccess(ExceptionManager exceptionManager)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ClearyMartConnection"].ConnectionString;

            asyncConnectionString = new SqlConnectionStringBuilder(connectionString)
            {
                AsynchronousProcessing = true
            }.ToString();

            _exceptionManager = exceptionManager;
        }

        private async Task<List<Person>> GetTimekeepers(string clientGroupNumber, DateTime startDate, DateTime endDate, List<string> titles, int threshold)
        {
            List<Person> people = new List<Person>();

            using (SqlConnection connection = new SqlConnection(asyncConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "spGetTimekeepersBilled";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClientGroupNumber", clientGroupNumber);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@Threshold", threshold);

                    SqlParameter titlesParameter = command.Parameters.AddWithValue("@TitlesTable", CreateDataTable(titles));
                    titlesParameter.SqlDbType = SqlDbType.Structured;
                    titlesParameter.TypeName = "dbo.TitlesTableType";

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (reader.Read())
                            {
                                if (DBNull.Value != reader["Name"])
                                {
                                    people.Add(new Person() { Name = (string)reader["Name"] });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Exception newException;
                        bool rethrow = _exceptionManager.HandleException(ex, "Policy", out newException);

                        if (rethrow)
                        {
                            // Exception policy setting is "ThrowNewException".
                            // Code here to perform any clean up tasks required.
                            // Then throw the exception returned by the exception handling policy.
                            throw newException;
                        }
                    }

                }
            }

            return people;
        }

        private static DataTable CreateDataTable(IEnumerable<string> ids)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            foreach (string id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }

        /// <summary>
        /// Get all timekeepers who have billed for client group number and billing date is between the start and end date
        /// and has a minimum number of billed hours equal to the threshold
        /// </summary>
        /// <param name="clientGroupNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public async Task<List<Person>> Get(string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold)
        {
            return await GetTimekeepers(clientGroupNumber, startDate, endDate, new List<string>(), threshold);
        }

        /// <summary>
        /// Get all partners & senior counsel who have billed for client group number and billing date is between the 
        /// start and end date and has a minimum number of billed hours equal to the threshold
        /// </summary>
        /// <param name="clientGroupNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public async Task<List<Person>> GetPartnersAndSeniorCounsel(string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold)
        {
            return await GetTimekeepers(clientGroupNumber, startDate, endDate, new List<string>() { "Partner", "Senior Counsel" }, threshold);
        }
    }
}
