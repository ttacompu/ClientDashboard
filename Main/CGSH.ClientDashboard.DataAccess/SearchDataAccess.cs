using CGSH.ClientDashboard.Interface.DataAccess;
using CGSH.ClientDashboard.BusinessEntitity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.DataAccess
{
    /// <summary>
    /// Search Client Group, Client and Matters Data Access
    /// </summary>
    public class SearchDataAccess : ISearchDataAccess
    {
        ExceptionManager _exceptionManager;
        string connectionString;
        string asyncConnectionString;

        /// <summary>
        /// Default Constructor
        /// </summary>
        private SearchDataAccess()
        {
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="exceptionManager"></param>
        public SearchDataAccess(ExceptionManager exceptionManager)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ClientDashboardConnection"].ConnectionString;

            asyncConnectionString = new SqlConnectionStringBuilder(connectionString)
            {
                AsynchronousProcessing = true
            }.ToString();

            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// Get a list of client groups based on the search string
        /// </summary>
        /// <param name="searchString">Search String</param>
        /// <returns></returns>
        public async Task<List<ClientGroup>> Get(string searchString)
        {
            List<ClientGroup> clientGroups = new List<ClientGroup>();

            using (SqlConnection connection = new SqlConnection(asyncConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "spClientMatterSearch";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchString", searchString);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            
                            while (reader.Read())
                            {
                                //Client Groups
                                if(clientGroups.Exists(cg => cg.Number == (string)reader["ClientGroupNumber"]) == false)
                                {
                                    ClientGroup clientGroup = new ClientGroup();

                                    clientGroup.Name = (string)reader["ClientGroupName"];
                                    clientGroup.Number = (string)reader["ClientGroupNumber"];

                                    clientGroups.Add(clientGroup);
                                }

                                ClientGroup foundClientGroup = clientGroups.Single(cg => cg.Number == (string)reader["ClientGroupNumber"]);

                                //Clients
                                if (foundClientGroup.Clients.Exists(c => c.Number == (string)reader["ClientNumber"]) == false)
                                {
                                    Client client = new Client();

                                    client.Name = (string)reader["ClientName"];
                                    client.Number = (string)reader["ClientNumber"];

                                    foundClientGroup.Clients.Add(client);
                                }

                                Client foundClient = foundClientGroup.Clients.Single(cg => cg.Number == (string)reader["ClientNumber"]);

                                //Matters
                                if (foundClient.Matters.Exists(m => m.ClientMatterNumber == (string)reader["ClientMatterNumber"]) == false)
                                {
                                    Matter matter = new Matter();

                                    matter.ClientMatterNumber = (string)reader["ClientMatterNumber"];
                                    matter.Name = (string)reader["MatterName"];

                                    foundClient.Matters.Add(matter);
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

            return clientGroups;
        }
    }
}
