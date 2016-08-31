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
    /// Entity Manager Class
    /// </summary>
    public class EntityManager : IEntityManager
    {
        IEntityDataAccess _EntityDataAccess;
        IMemCache _MemCache;
        ICustomExceptionManager _exceptionManager;
        IApiKeyManager _ApiKeyManager;
        

        /// <summary>
        /// Hide Default Constructor
        /// </summary>
        public EntityManager()
        {
        }

        /// <summary>
        /// Overload Constructor
        /// </summary>
        /// <param name="apiKeyManager"></param>
        /// <param name="entityDataAccess"></param>
        /// <param name="memCache"></param>
        /// <param name="exceptionManager"></param>
        public EntityManager(IApiKeyManager apiKeyManager, IEntityDataAccess entityDataAccess, IMemCache memCache, ICustomExceptionManager exceptionManager)
        {
            _EntityDataAccess = entityDataAccess;
            _ApiKeyManager = apiKeyManager;
            _MemCache = memCache;
            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="clientGroupNumber"></param>
        /// <returns></returns>
        public async Task<List<Client>> Get(string apiKey, string clientGroupNumber)
        {
            string CACHEKEY = "EntityManager_" + clientGroupNumber;
            try
            {
                if (await _ApiKeyManager.IsValid(apiKey))
                {
                    List<Client> clients;
                    var isInCache = _MemCache.TryGet(CACHEKEY, out clients);
                    if (isInCache)
                    {
                        return await Task.FromResult(clients);
                    }
                    var dbResult = await _EntityDataAccess.Get(clientGroupNumber);

                    if (dbResult != null)
                    {
                        _MemCache.Set<List<Client>>(CACHEKEY, dbResult);
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
