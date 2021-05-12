using MessagingLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ScootAPI.Models;
using ScootAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScootAPI.Controllers
{
    [Route("api/[controller]")]

    public class ScootersController : ControllerBase
    {
        private readonly IScootersService _scootersService;
        private readonly IAmqpService _amqpService;
        private readonly IDistributedCache _distributedCache;

        public ScootersController(IScootersService scootersService, IAmqpService amqpService, IDistributedCache distributedCache)
        {
            _scootersService = scootersService;
            _amqpService = amqpService;
            _distributedCache = distributedCache;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Scooter>> Get(string id)
        {
            try
            {
                string scooterKey = "Scooter/" + id;
                var serializedScooter = await _distributedCache.GetStringAsync(scooterKey);

                if (serializedScooter != null)
                {
                    return JsonConvert.DeserializeObject<Scooter>(serializedScooter);
                }

                Scooter scooter = _scootersService.GetScooter(id);

                if (scooter == null) return NotFound();

                await _distributedCache.SetStringAsync(scooterKey, JsonConvert.SerializeObject(scooter));

                return scooter;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(string id, [FromBody] Scooter scooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    scooter.IdScooter = id;
                    _scootersService.UpdateScooter(scooter);

                    await _distributedCache.SetStringAsync("Scooter/" + id, JsonConvert.SerializeObject(scooter));

                    MessageEntity msg = new("Scooter", id, "Update");
                    _amqpService.SendMessage(msg);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Scooter scooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string id = Guid.NewGuid().ToString();
                    scooter.IdScooter = id;

                    _scootersService.AddScooter(scooter);

                    await _distributedCache.SetStringAsync("Scooter/" + id, JsonConvert.SerializeObject(scooter));

                    _amqpService.SendMessage(new MessageEntity("Scooter", id, "Create"));

                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Scooter scooter = _scootersService.GetScooter(id);
                if (scooter == null) return NotFound();
                _scootersService.DeleteScooter(id);
                await _distributedCache.RemoveAsync("Scooter" + id);

                MessageEntity msg = new("Scooter", id, "Delete");
                _amqpService.SendMessage(msg);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scooter>>> Get()
        {
            try
            {
                return _scootersService.GetScooters();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
