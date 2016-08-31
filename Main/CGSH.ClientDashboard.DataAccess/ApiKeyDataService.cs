using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.Exceptions;
using CGSH.ClientDashboard.Interface.DataAccess;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.DataAccess
{
    public class ApiKeyDataService : IApiKeyDataService
    {
        string connectionString;
        string asyncConnectionString;
        ExceptionManager _exceptionManager;

        /// <summary>
        /// Hide Default Constructor
        /// </summary>
        private ApiKeyDataService()
        {
        }

        /// <summary>
        /// Override Constructor
        /// </summary>
        /// <param name="exceptionManager"></param>
        public ApiKeyDataService(ExceptionManager exceptionManager)
        {
            connectionString =
           ConfigurationManager.ConnectionStrings["ClientDashboardConnection"].ConnectionString;

            asyncConnectionString = new SqlConnectionStringBuilder(connectionString)
            {
                AsynchronousProcessing = true
            }.ToString();

            _exceptionManager = exceptionManager;

        }

        /// <summary>
        /// Get a list of All Api Key
        /// </summary>
        public async  Task<List<ApiKey>> All()
        {
           
            List<ApiKey> apiKeySettings = new List<ApiKey>();
            ApiKey apiKeySetting = null;
            using (SqlConnection connection = new SqlConnection(asyncConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "spApiKey_Select";
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                apiKeySetting = new ApiKey();
                                apiKeySetting.Id = (long)reader["ApiID"];
                                apiKeySetting.ApplicationName = (DBNull.Value == reader["ApplicationName"]) ? string.Empty : (string)reader["ApplicationName"];
                                if (reader["ExpirationDate"] != DBNull.Value)
                                {
                                    apiKeySetting.ExpirationDate = (DateTime)reader["ExpirationDate"];
                                }
                                apiKeySetting.Key = (DBNull.Value == reader["ApiKey"]) ? string.Empty : (string)reader["ApiKey"];
                                apiKeySettings.Add(apiKeySetting);
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

            return apiKeySettings;
        }

        /// <summary>
        /// Delete an Api Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(long id)
        {
            int rowEffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(asyncConnectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "spApiKey_Delete";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApiID", id);
                        connection.Open();
                        rowEffected = await command.ExecuteNonQueryAsync();
                    }
                }
                if (rowEffected == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                Exception newException;
                bool rethrow = _exceptionManager.HandleException(sqlEx, "Policy", out newException);

                if (rethrow)
                {
                    // Exception policy setting is "ThrowNewException".
                    // Code here to perform any clean up tasks required.
                    // Then throw the exception returned by the exception handling policy.
                    throw newException;
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
            return false;
        }

        /// <summary>
        /// Get a ApiKey
        /// By ID
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiKey> Get(long id)
        {
            ApiKey apiKeySetting = null;
            using (SqlConnection connection = new SqlConnection(asyncConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "spApiKey_Select";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApiID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (reader.Read())
                            {
                                apiKeySetting = new ApiKey();
                                apiKeySetting.Id = (long)reader["ApiID"];
                                apiKeySetting.ApplicationName = (DBNull.Value == reader["ApplicationName"]) ? string.Empty : (string)reader["ApplicationName"];
                                if (reader["ExpirationDate"] != DBNull.Value)
                                {
                                    apiKeySetting.ExpirationDate = (DateTime)reader["ExpirationDate"];
                                }
                                apiKeySetting.Key = (DBNull.Value == reader["ApiKey"]) ? string.Empty : (string)reader["ApiKey"];
                            }

                        }

                    }
                    catch (SqlException sqlEx)
                    {
                        switch (sqlEx.Message)
                        {
                            case "Api Key does not exist":
                                throw new ValidationException(string.Format("Api ID {0}; Error Message {1}", id.ToString(), sqlEx.Message));

                        }
                        Exception newException;
                        bool rethrow = _exceptionManager.HandleException(sqlEx, "Policy", out newException);

                        if (rethrow)
                        {
                        //    // Exception policy setting is "ThrowNewException".
                        //    // Code here to perform any clean up tasks required.
                        //    // Then throw the exception returned by the exception handling policy.
                            throw newException;
                        }
                    }
                    catch (Exception Ex)
                    {
                        Exception newException;
                        bool rethrow = _exceptionManager.HandleException(Ex, "Policy", out newException);

                        if (rethrow)
                        {
                        //    // Exception policy setting is "ThrowNewException".
                        //    // Code here to perform any clean up tasks required.
                        //    // Then throw the exception returned by the exception handling policy.
                        //    throw newException;
                        }
                    }

                    return apiKeySetting;
                }
            }
        }

        /// <summary>
        /// Insert or Update an Api Key
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ApiKey> Save(ApiKey item)
        {
            using (SqlConnection connection = new SqlConnection(asyncConnectionString))
            {

                try
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "spApiKey_Update";
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter ApiIDParameter = command.CreateParameter();
                        ApiIDParameter.Direction = ParameterDirection.InputOutput;
                        ApiIDParameter.ParameterName = "@ApiID";
                        ApiIDParameter.SqlDbType = SqlDbType.BigInt;
                        ApiIDParameter.Value = item.Id;
                        command.Parameters.Add(ApiIDParameter);

                        command.Parameters.AddWithValue("@ApplicationName", item.ApplicationName);
                        command.Parameters.AddWithValue("@ExpirationDate", item.ExpirationDate);
                        connection.Open();
                        await command.ExecuteNonQueryAsync();

                        //Check output parameter
                        if (ApiIDParameter != null)
                        {
                            if (ApiIDParameter.Value != null)
                            {
                                item.Id = (long)ApiIDParameter.Value;
                            }
                        }

                    }
                }
                catch (SqlException sqlEx)
                {

                    if (sqlEx.Message.Equals("Attempt made to update a row that does not exist."))
                    {
                        throw new ValidationException(string.Format("Api ID {0}; Error Message {1}", item.Id, sqlEx.Message));
                    }

                    Exception newException;
                    bool rethrow = _exceptionManager.HandleException(sqlEx, "Policy", out newException);

                    if (rethrow)
                    {
                        // Exception policy setting is "ThrowNewException".
                        // Code here to perform any clean up tasks required.
                        // Then throw the exception returned by the exception handling policy.
                        throw newException;
                    }
                }
                catch (Exception Ex)
                {
                    Exception newException;
                    bool rethrow = _exceptionManager.HandleException(Ex, "Policy", out newException);

                    if (rethrow)
                    {
                        // Exception policy setting is "ThrowNewException".
                        // Code here to perform any clean up tasks required.
                        // Then throw the exception returned by the exception handling policy.
                        throw newException;
                    }
                }

            }
            return item;
        }

    }
}
