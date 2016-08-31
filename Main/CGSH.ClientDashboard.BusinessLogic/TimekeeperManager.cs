using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.Exceptions;
using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.Interface.DataAccess;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessLogic
{
    public class TimekeeperManager : ITimekeeperManager
    {
        ITimekeeperDataAccess _TimekeeperDataAccess;
        IMemCache _MemCache;
        ICustomExceptionManager _exceptionManager;
        IApiKeyManager _ApiKeyManager;

        /// <summary>
        /// Hide Default Constructor
        /// </summary>
        public TimekeeperManager()
        {
        }

        /// <summary>
        /// Overload Constructor
        /// </summary>
        /// <param name="apiKeyManager"></param>
        /// <param name="timekeeperDataAccess"></param>
        /// <param name="memCache"></param>
        /// <param name="exceptionManager"></param>
        public TimekeeperManager(IApiKeyManager apiKeyManager, ITimekeeperDataAccess timekeeperDataAccess, IMemCache memCache, ICustomExceptionManager exceptionManager)
        {
            _TimekeeperDataAccess = timekeeperDataAccess;
            _ApiKeyManager = apiKeyManager;
            _MemCache = memCache;
            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// Get Timekeepers based on parameters
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public async Task<List<Person>> Get(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold)
        {
            string CACHEKEY = "TimekeeperManager_Get_" + clientGroupNumber + "_" +startDate.ToLongDateString() + "_" + endDate.ToLongDateString() + "_" + threshold.ToString();
            try
            {
                if (await _ApiKeyManager.IsValid(apiKey))
                {
                    List<Person> persons;
                    var isInCache = _MemCache.TryGet(CACHEKEY, out persons);
                    if (isInCache)
                    {
                        return await Task.FromResult(persons);
                    }
                    var dbResult = await _TimekeeperDataAccess.Get(clientGroupNumber, startDate, endDate, threshold);

                    if (dbResult != null)
                    {
                        _MemCache.Set<List<Person>>(CACHEKEY, dbResult);
                    }

                    return dbResult;
                }
                else
                {
                    throw new ValidationException("Bad API Key!");
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
                return null;
            }
        }

        /// <summary>
        /// Get Partner and Senior Counsel based on parameters
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public async Task<List<Person>> GetPartnersAndSeniorCounsel(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold)
        {
            string CACHEKEY = "TimekeeperManager_GetPartnersAndSeniorCounsel_" + clientGroupNumber + "_" +  startDate.ToLongDateString() + "_" + endDate.ToLongDateString() + "_" + threshold.ToString();
            try
            {
                if (await _ApiKeyManager.IsValid(apiKey))
                {
                    List<Person> persons;
                    var isInCache = _MemCache.TryGet(CACHEKEY, out persons);
                    if (isInCache)
                    {
                        return await Task.FromResult(persons);
                    }
                    var dbResult = await _TimekeeperDataAccess.GetPartnersAndSeniorCounsel(clientGroupNumber, startDate, endDate, threshold);

                    if (dbResult != null)
                    {
                        _MemCache.Set<List<Person>>(CACHEKEY, dbResult);
                    }

                    return dbResult;
                }
                else
                {
                    throw new ValidationException("Bad API Key!");
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
                return null;
            }
        }


        // <summary>
        /// Validate Input Parameter are correct
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="threshold"></param>
        /// <returns>String with keys</returns>
        public Task<string> ValidateInputParams(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold)
        {
            var dic = new Dictionary<string, string>();
            string errors = "";

            if (string.IsNullOrEmpty(apiKey)) {
                dic.Add("apiKey", "Api key is required!");
            }

            if (string.IsNullOrEmpty(clientGroupNumber))
            {
                dic.Add("clientGroupNumber", "clientGroupNumber is required!");
            }

            if (startDate >= endDate)
            {
                dic.Add("startDate", "Start Date must be less than End Date!");
            }

            if (threshold < 1) {
                dic.Add("threshold", "Threshold should be a positive number!");
            }
            foreach (var error in dic) {
                errors += string.Format("{0}: {1}", error.Key, error.Value);
            }


            return Task.FromResult(errors);
        }
    }
}
