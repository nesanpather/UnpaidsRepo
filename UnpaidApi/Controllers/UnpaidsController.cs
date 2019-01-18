using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UnpaidManager.Interfaces;
using UnpaidModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UnpaidApi.Controllers
{    
    [EnableCors("SiteCorsPolicy")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UnpaidsController : ControllerBase
    {
        private readonly IUnpaidEngineHandler _unpaidEngineHandler;
        private readonly IUnpaidRequestClient _unpaidRequestClient;
        private readonly IUnpaidResponseClient _unpaidResponseClient;

        public UnpaidsController(IUnpaidEngineHandler unpaidEngineHandler, IUnpaidRequestClient unpaidRequestClient, IUnpaidResponseClient unpaidResponseClient)
        {
            _unpaidEngineHandler = unpaidEngineHandler;
            _unpaidRequestClient = unpaidRequestClient;
            _unpaidResponseClient = unpaidResponseClient;
        }

        /// <summary>  
        /// This method contains Authorize attribute for authentication and authroization  
        /// </summary>  
        /// <returns></returns>  
        [HttpGet]        
        [Authorize]
        [Route("AuthenticateUser")]
        public ActionResult<HttpResponseMessage> AuthenticateUser()
        {
            if (User != null)
            {                                       
                //return Ok(new
                //{
                //    status = (int)HttpStatusCode.OK,
                //    isAuthenticated = true,
                //    isLibraryAdmin = User.IsInRole(@"domain\AdminGroup"),
                //    username = User.Identity.Name.Substring(User.Identity.Name.LastIndexOf(@"\", StringComparison.InvariantCultureIgnoreCase) + 1)
                //});
                return Ok(new { a = User.Identity.IsAuthenticated, b = User.Identity.Name, c = User.Identity.AuthenticationType});
            }

            return BadRequest();
        }

        [HttpPost("Add")]
        public async Task<ActionResult<UnpaidOutput>> CreateUnpaidAsync([FromBody] IEnumerable<UnpaidInput> unpaids, CancellationToken cancellationToken)
        {
            // Log Entry.

            var handleUnpaidResult = await _unpaidEngineHandler.HandleUnpaidAsync(unpaids, Guid.NewGuid().ToString(), cancellationToken);

            if (handleUnpaidResult == null)
            {
                return BadRequest();
            }

            return Ok(handleUnpaidResult);
        }

        [HttpPost("responses/add")]
        public async Task<ActionResult<IEnumerable<UnpaidOutput>>> AddNotificationResponse([FromBody] IEnumerable<UnpaidResponseInput> unpaidResponses, CancellationToken cancellationToken)
        {
            // Log Entry.

            var handleUnpaidResponseResult = await _unpaidEngineHandler.HandleUnpaidResponseAsync(unpaidResponses, cancellationToken);

            if (handleUnpaidResponseResult == null)
            {
                return BadRequest();
            }

            if (!handleUnpaidResponseResult.Any())
            {
                return BadRequest();
            }

            return Ok(handleUnpaidResponseResult);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllUnpaidRequestOutput>>> GetAllUnpaidRequestAsync(CancellationToken cancellationToken)
        {
            // Log Entry.

            var getAllUnpaidRequestAsyncResult = await _unpaidRequestClient.GetAllUnpaidRequestAsync(cancellationToken);

            if (getAllUnpaidRequestAsyncResult == null)
            {
                return BadRequest();
            }

            if (!getAllUnpaidRequestAsyncResult.Any())
            {
                return BadRequest();
            }

            return Ok(getAllUnpaidRequestAsyncResult);
        }

        [HttpGet("policynumber/{policyNumber}")]
        public async Task<ActionResult<IEnumerable<GetAllUnpaidRequestOutput>>> GetAllUnpaidRequestAsync(string policyNumber, CancellationToken cancellationToken)
        {
            // Log Entry.

            var getAllUnpaidRequestAsyncResult = await _unpaidRequestClient.GetAllUnpaidRequestAsync(policyNumber, cancellationToken);

            if (getAllUnpaidRequestAsyncResult == null)
            {
                return BadRequest();
            }

            if (!getAllUnpaidRequestAsyncResult.Any())
            {
                return BadRequest();
            }

            return Ok(getAllUnpaidRequestAsyncResult);
        }

        [HttpGet("pageindex/{pageIndex}/pagesize/{pageSize}")]
        public async Task<ActionResult<IEnumerable<GetAllUnpaidRequestOutput>>> GetAllUnpaidRequestAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            // Log Entry.

            var getAllUnpaidRequestAsyncResult = await _unpaidRequestClient.GetAllUnpaidRequestAsync(pageIndex, pageSize, cancellationToken);

            if (getAllUnpaidRequestAsyncResult == null)
            {
                return BadRequest();
            }

            if (!getAllUnpaidRequestAsyncResult.Any())
            {
                return BadRequest();
            }

            return Ok(getAllUnpaidRequestAsyncResult);
        }

        [HttpGet("responses")]
        public async Task<ActionResult<IEnumerable<GetAllUnpaidResponseOutput>>> GetAllUnpaidResponseAsync(CancellationToken cancellationToken)
        {
            // Log Entry.

            var getAllUnpaidResponseAsyncResult = await _unpaidResponseClient.GetAllUnpaidResponseAsync(cancellationToken);

            if (getAllUnpaidResponseAsyncResult == null)
            {
                return BadRequest();
            }

            if (!getAllUnpaidResponseAsyncResult.Any())
            {
                return BadRequest();
            }

            return Ok(getAllUnpaidResponseAsyncResult);
        }

        [HttpGet("responses/policynumber/{policyNumber}")]
        public async Task<ActionResult<IEnumerable<GetAllUnpaidResponseOutput>>> GetAllUnpaidResponseAsync(string policyNumber, CancellationToken cancellationToken)
        {
            // Log Entry.

            var getAllUnpaidResponseAsyncResult = await _unpaidResponseClient.GetAllUnpaidResponseAsync(policyNumber, cancellationToken);

            if (getAllUnpaidResponseAsyncResult == null)
            {
                return BadRequest();
            }

            if (!getAllUnpaidResponseAsyncResult.Any())
            {
                return BadRequest();
            }

            return Ok(getAllUnpaidResponseAsyncResult);
        }

    }
}
