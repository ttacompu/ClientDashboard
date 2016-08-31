using CGSH.ClientDashboard.Interface.DataAccess;
using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.DataAccess
{
    public class EntityDataAccess : IEntityDataAccess
    {
        ExceptionManager _exceptionManager;
        string connectionString;
        string asyncConnectionString;

        /// <summary>
        /// Hide Default Constructor
        /// </summary>
        private EntityDataAccess()
        {
        }

        /// <summary>
        /// Override Constructor
        /// </summary>
        /// <param name="exceptionManager"></param>
        public EntityDataAccess(ExceptionManager exceptionManager)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ClientDashboardConnection"].ConnectionString;

            asyncConnectionString = new SqlConnectionStringBuilder(connectionString)
            {
                AsynchronousProcessing = true
            }.ToString();
            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// Get a list of entities based on a client group number
        /// </summary>
        /// <param name="clientGroupNumber"></param>
        /// <returns></returns>
        public async Task<List<Client>> Get(string clientGroupNumber)
        {
            List<Client> clientLst = new List<Client>();

            Client client = null;
            using (SqlConnection connection = new SqlConnection(asyncConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "spGetClientEntities";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClGrpNum", clientGroupNumber);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                client = new Client();
                                client.Name = reader["ClientName"] as string;
                                client.Number = reader["ClientNumber"] as string;
                                clientLst.Add(client);
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

            return clientLst;
        }
    }
}
