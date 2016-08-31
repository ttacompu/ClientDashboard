using CGSH.ClientDashboard.Interface.BusinessLogic;
using CGSH.ClientDashboard.Interface.UI;
using CGSH.ClientDashboard.BusinessEntitity;
using CGSH.ClientDashboard.WebApi.Util;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace CGSH.ClientDashboard.WebApi.Controllers
{
    /// <summary>
    /// Web Api Api Key Controller
    /// </summary>
    [RoutePrefix("api/ApiKey")]
    public class ApiKeyController : ApiController, IClientDashboardWebAPI<ApiKey>
    {
        IApiKeyManager _ApiKeyManager;
        ICustomExceptionManager _CustomExceptionManager;

        /// <summary>
        /// Overload Constructor
        /// </summary>
        /// <param name="apiKeyManager"></param>
        /// <param name="customExceptionManager"></param>
        public ApiKeyController(IApiKeyManager apiKeyManager, ICustomExceptionManager customExceptionManager)
        {
            _ApiKeyManager = apiKeyManager;
            _CustomExceptionManager = customExceptionManager;
        }

        /// <summary>
        /// Get All api keys
        /// </summary>
        /// <example>
        /// example URL: http://localhost:24625/api/apikey/
        /// </example>
        /// <returns>List of ApiKey with OK Response</returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> All()
        {
            try
            {
                var results = await _ApiKeyManager.All();
                if (results == null || results.Count == 0)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch(Exception ex)
            {
                Exception newException;
                bool rethrow = _CustomExceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);

                if (rethrow)
                {
                    // Exception policy setting is "ThrowNewException".
                    // Code here to perform any clean up tasks required.
                    // Then throw the exception returned by the exception handling policy.
                    throw newException;
                }
                return InternalServerError();
            }
        }

        /// <summary>
        ///  Get Single Api
        /// </summary>
        /// <param name="id"></param>
        ///  <example>
        /// example URL: http://localhost:24625/api/apikey/1
        /// </example>
        /// <returns>Single Api Key</returns>
        [Route("{id:long}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByID(long id)
        {
            try
            {
                var result = await _ApiKeyManager.Get(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                Exception newException;
                bool rethrow = _CustomExceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);

                if (rethrow)
                {
                    // Exception policy setting is "ThrowNewException".
                    // Code here to perform any clean up tasks required.
                    // Then throw the exception returned by the exception handling policy.
                    throw newException;
                }
                return InternalServerError();
            }
        }

        /// <summary>
        /// Create a API Key
        /// </summary>
        /// <param name="apikey"></param>
        /// 
        /// <example>
        /// example URL: http://localhost:24625/api/apikey/
        /// </example>
        /// 
        /// <returns>http link to API key created</returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save(ApiKey apikey)
        {
            try
            {
                var result = await _ApiKeyManager.Save(apikey);
                if (result == null)
                {
                    return Conflict();
                }
                return CreatedAtRoute("DefaultApi", new { id = result.Id, controller = "ApiKey" }, result);
            }
            catch (Exception ex)
            {
                Exception newException;
                bool rethrow = _CustomExceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);

                if (rethrow)
                {
                    // Exception policy setting is "ThrowNewException".
                    // Code here to perform any clean up tasks required.
                    // Then throw the exception returned by the exception handling policy.
                    throw newException;
                }
                return InternalServerError();
            }


        }

        /// <summary>
        /// Modify Existing API key
        /// </summary>
        /// <param name="apikey"></param>
        /// <example>
        /// example URL: http://localhost:24625/api/apikey/
        /// </example>
        /// <returns>Modified Api Key</returns>
        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> Update(ApiKey apikey)
        {
            try
            {
                var result = await _ApiKeyManager.Save(apikey);
                if (result == null)
                {
                    return NotFound();
                }

                return Content(HttpStatusCode.Accepted, result);

            }
            catch (Exception ex)
            {
                Exception newException;
                bool rethrow = _CustomExceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);

                if (rethrow)
                {
                    // Exception policy setting is "ThrowNewException".
                    // Code here to perform any clean up tasks required.
                    // Then throw the exception returned by the exception handling policy.
                    throw newException;
                }
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete existing API Key
        /// </summary>
        /// <param name="id"></param>
        ///  <example>
        /// example URL: http://localhost:24625/api/apikey/1
        /// </example>
        /// <returns>true if deleting is success</returns>
        [HttpDelete]
        [Route("{id:long}")]
        public async Task<IHttpActionResult> Delete(long id)
        {
            try
            {
               var result=await _ApiKeyManager.Delete(id);
                if (result)
                {
                    return Ok(new { message = string.Format("api id {0} is deleted", id) });
                }
                else {
                    return NotFound();
                }
                
            }
            catch (Exception ex)
            {
                Exception newException;
                bool rethrow = _CustomExceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);

                if (rethrow)
                {
                    // Exception policy setting is "ThrowNewException".
                    // Code here to perform any clean up tasks required.
                    // Then throw the exception returned by the exception handling policy.
                    throw newException;
                }
                return InternalServerError();
            }
        }
    }
}
