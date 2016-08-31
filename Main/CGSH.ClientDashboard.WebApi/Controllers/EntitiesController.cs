using CGSH.ClientDashboard.Interface.BusinessLogic;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace CGSH.ClientDashboard.WebApi.Controllers
{
    /// <summary>
    /// Web Api Entities Controller
    /// </summary>
    [RoutePrefix("api/Entities")]
    public class EntitiesController : ApiController
    {
        IEntityManager _entityManager;
        ICustomExceptionManager _exceptionManager;

        /// <summary>
        /// Overload Constructor
        /// </summary>
        /// <param name="entityManager"></param>
        /// <param name="exceptionManager"></param>
        public EntitiesController(IEntityManager entityManager, ICustomExceptionManager exceptionManager)
        {
            _entityManager = entityManager;
            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// REST API - Get
        /// </summary>
        /// <example>
        /// http://localhost:24625/api/Entities?apiKey=105493DE-8FCD-4F7D-A861-C5ABB372B8E8&clientGroupNumber=04021
        /// </example>
        /// <param name="apiKey"></param>
        /// <param name="clientGroupNumber"></param>
        /// <returns>List of Client</returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string apiKey, string clientGroupNumber)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(clientGroupNumber))
            {
                return NotFound();
            }

            try
            {
                var results = await _entityManager.Get(apiKey.Trim(), clientGroupNumber.Trim());
                if (results == null || results.Count == 0)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                Exception newException;
                bool rethrow = _exceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);

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