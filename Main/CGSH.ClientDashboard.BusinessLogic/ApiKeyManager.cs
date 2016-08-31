using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.Interface.DataAccess;
using CGSH.ClientDashboard.BusinessEntitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGSH.ClientDashboard.BusinessLogic
{
    /// <summary>
    /// Application Key Manager
    /// </summary>
    public class ApiKeyManager : IApiKeyManager
    {
        IApiKeyDataService _apiKeyDataService;
        ICustomExceptionManager _exceptionManager;

        /// <summary>
        /// Hide Default Constructor
        /// </summary>
        public ApiKeyManager()
        {
        }

        /// <summary>
        /// Constructor with two parameters
        /// </summary>
        /// <param name="IApiKeyDataService"></param>
        /// <param name="ICustomExceptionManager"></param>
        public ApiKeyManager(IApiKeyDataService apiKeyDataService, ICustomExceptionManager exceptionManager)
        {
            _apiKeyDataService = apiKeyDataService;
            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// Get all Api Keys
        /// </summary>
        /// <returns></returns>
        public Task<List<ApiKey>> All()
        {
            try
            {
                return _apiKeyDataService.All();
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
        /// Get a single Api Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApiKey> Get(long id)
        {
            try
            {
                return _apiKeyDataService.Get(id);
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
        /// Delete an Api Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> Delete(long id)
        {
            try
            {
                return _apiKeyDataService.Delete(id);
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
                return Task.FromResult(false);
            }

        }

        /// <summary>
        /// Insert or Update an Api Key
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<ApiKey> Save(ApiKey item)
        {
            try
            {
                return _apiKeyDataService.Save(item);
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
        /// Is Api Key Valid
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<bool> IsValid(string apiKey)
        {
            try
            {
                var allEntries = await All();
                var result = allEntries.SingleOrDefault(x => x.Key == apiKey);
                if (result != null)
                {
                    return true;
                }
                return false;
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
                return false;
            }
        }
    }
}
