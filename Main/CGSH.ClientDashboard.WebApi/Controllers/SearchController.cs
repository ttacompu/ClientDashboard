using CGSH.ClientDashboard.Interface.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CGSH.ClientDashboard.WebApi.Controllers
{
    /// <summary>
    /// Web Api Search Controller
    /// </summary>
    [RoutePrefix("api/Search")]
    public class SearchController : ApiController
    {
        ISearchManager _searchManager;
        ICustomExceptionManager _exceptionManager;

        /// <summary>
        /// Overload Constructor
        /// </summary>
        /// <param name="searchManager"></param>
        /// <param name="exceptionManager"></param>
        public SearchController(ISearchManager searchManager, ICustomExceptionManager exceptionManager)
        {
            _searchManager = searchManager;
            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// REST API - Get 
        /// </summary>
        /// <example>
        /// http://localhost:24625/api/search?apiKey=105493DE-8FCD-4F7D-A861-C5ABB372B8E8&searchString=Citi
        /// </example>
        /// <param name="apiKey"></param>
        /// <param name="searchString"></param>
        /// <returns>List of ClientGroup</returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string apiKey, string searchString)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(searchString))
            {
                return NotFound();
            }

            try
            {
                var results = await _searchManager.Get(apiKey.Trim(), searchString.Trim());
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