using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UnpaidManager.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UnpaidNotificationsApi.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController : Controller
    {
        private readonly IUnpaidNotificationsEngineHandler _unpaidNotificationsEngineHandler;

        public NotificationsController(IUnpaidNotificationsEngineHandler unpaidNotificationsEngineHandler)
        {
            _unpaidNotificationsEngineHandler = unpaidNotificationsEngineHandler;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{idempotencyKey}")]
        public async Task<ActionResult<string>> ProcessNotificationsAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                // Log Error.
                return BadRequest("IdempotencyKey is null or empty.");
            }

            try
            {
                await _unpaidNotificationsEngineHandler.HandleUnpaidNotificationsAsync(idempotencyKey, cancellationToken);
            }
            catch (Exception e)
            {
                // Log Error.
                return BadRequest("Failed to process the notifications");
            }

            return Ok("Success");
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
