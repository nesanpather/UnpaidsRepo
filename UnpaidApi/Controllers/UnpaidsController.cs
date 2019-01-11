using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnpaidManager.Interfaces;
using UnpaidModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UnpaidApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UnpaidsController : ControllerBase
    {
        private readonly IUnpaidEngineHandler _unpaidEngineHandler;

        public UnpaidsController(IUnpaidEngineHandler unpaidEngineHandler)
        {
            _unpaidEngineHandler = unpaidEngineHandler;
        }

        //// GET: api/Todo
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        //{
        //    return await _context.TodoItems.ToListAsync();
        //}

        // POST: api/Todo
        [HttpPost("Add")]
        public async Task<ActionResult<IEnumerable<UnpaidOutput>>> CreateUnpaidAsync([FromBody] IEnumerable<UnpaidInput> unpaids, CancellationToken cancellationToken)
        {
            // Log Entry.

            var handleUnpaidResult = await _unpaidEngineHandler.HandleUnpaidAsync(unpaids, Guid.NewGuid().ToString(), cancellationToken);

            if (handleUnpaidResult == null)
            {
                return BadRequest();
            }

            if (!handleUnpaidResult.Any())
            {
                return BadRequest();
            }

            return Ok(handleUnpaidResult);
        }

        [HttpPost("response/add")]
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

    }
}
