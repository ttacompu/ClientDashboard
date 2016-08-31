using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.Interface.DataAccess;
using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessLogic
{
    /// <summary>
    /// Search Manager Class
    /// </summary>
    public class SearchManager : ISearchManager
    {
        ISearchDataAccess _SearchDataAccess;
        IMemCache _MemCache;
        ICustomExceptionManager _exceptionManager;
        IApiKeyManager _ApiKeyManager;

        /// <summary>
        /// Constructor with three parameters
        /// </summary>
        /// <param name="IApiKeyManager"></param>
        /// <param name="ISearchDataAccess"></param>
        /// <param name="IMemCache"></param>
        /// <param name="ICustomExceptionManager"></param>
        public SearchManager(IApiKeyManager ApiKeyManager, ISearchDataAccess SearchDataAccess, IMemCache MemCache, ICustomExceptionManager exceptionManager)
        {
            _ApiKeyManager = ApiKeyManager;
            _SearchDataAccess = SearchDataAccess;
            _MemCache = MemCache;
            _exceptionManager = exceptionManager;
        }


        /// <summary>
        /// Get ClientGroup from Cache or DB
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="searchString"></param>
        /// <returns>List of ClientGroup</returns>
        public async Task<List<ClientGroup>> Get(string apiKey, string searchString)
        {
            string CACHEKEY = "SearchManager_" + searchString;
            try
            {
                if (await _ApiKeyManager.IsValid(apiKey))
                {
                    List<ClientGroup> clientGroups;
                    var isInCache = _MemCache.TryGet(CACHEKEY, out clientGroups);
                    if (isInCache)
                    {
                        return await Task.FromResult(clientGroups);
                    }

                    var dbResult = await _SearchDataAccess.Get(searchString);
                    if (dbResult != null) {
                        _MemCache.Set<List<ClientGroup>>(CACHEKEY, dbResult);
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
    }
}
