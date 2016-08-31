using CGSH.ClientDashboard.Exceptions;
using CGSH.ClientDashboard.Interface.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace CGSH.ClientDashboard.WebApi.Controllers
{
    /// <summary>
    /// Web Api Timekeepers Controller
    /// </summary>
    [RoutePrefix("api/Timekeepers")]
    public class TimekeepersController : ApiController
    {
        ITimekeeperManager _timekeeperManager;
        ICustomExceptionManager _exceptionManager;

        public TimekeepersController(ITimekeeperManager timekeeperManager, ICustomExceptionManager exceptionManager)
        {
            _timekeeperManager = timekeeperManager;
            _exceptionManager = exceptionManager;
        }

        /// <summary>
        /// REST API - Get
        /// </summary>
        /// <example>
        /// http://localhost:24625/api/Timekeepers/All?apiKey=105493DE-8FCD-4F7D-A861-C5ABB372B8E8&clientGroupNumber=04021&startDate=01/01/2016&endDate=08/01/2016&threshold=1
        /// </example>
        /// <param name="apiKey"></param>
        /// <param name="clientGroupNumber">Client Group Number</param>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="threshold">Threshold</param>
        /// <returns></returns>
        [Route("All")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold)
        {
            try
            {
                var errors = await _timekeeperManager.ValidateInputParams(apiKey, clientGroupNumber, startDate, endDate, threshold);
                if (!string.IsNullOrEmpty(errors))
                {
                    throw new ValidationException(errors);
                }
                var results = await _timekeeperManager.Get(apiKey.Trim(), clientGroupNumber, startDate, endDate, threshold);
                if (results == null || results.Count == 0)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch (ValidationException ex) {
                Exception newException;
                bool rethrow = _exceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);
                return BadRequest(ex.Message);
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

        /// <summary>
        /// REST API - Get
        /// </summary>
        /// <example>
        /// http://localhost:24625/api/Timekeepers/PartnersAndSeniorCounsel?apiKey=105493DE-8FCD-4F7D-A861-C5ABB372B8E8&clientGroupNumber=04021&startDate=01/01/2016&endDate=08/01/2016&threshold=1
        /// </example>
        /// <param name="apiKey"></param>
        /// <param name="clientGroupNumber">Client Group Number</param>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="threshold">Threshold</param>
        /// <returns></returns>
        [Route("PartnersAndSeniorCounsel")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPartnersAndSeniorCounsel(string apiKey, string clientGroupNumber, DateTime startDate, DateTime endDate, int threshold)
        {
            try
            {
                var errors = await _timekeeperManager.ValidateInputParams(apiKey, clientGroupNumber, startDate, endDate, threshold);
                if (!string.IsNullOrEmpty(errors))
                {
                    throw new ValidationException(errors);
                }
                var results = await _timekeeperManager.GetPartnersAndSeniorCounsel(apiKey.Trim(), clientGroupNumber, startDate, endDate, threshold);
                if (results == null || results.Count == 0)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch (ValidationException ex)
            {
                Exception newException;
                bool rethrow = _exceptionManager.HandleException(ex, "Presentation Layer Policy", out newException);
                return BadRequest(ex.Message);
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
