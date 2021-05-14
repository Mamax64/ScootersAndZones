using Cache;
using MessagingLib;
using Microsoft.AspNetCore.Mvc;
using ScootAPI.Models;
using ScootAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScootAPI.Controllers
{
    [Route("api/[controller]")]

    public class ScootersController : ControllerBase
    {
        private const string KEY_PREFIX = "Scooter:";
        private readonly IScootersService _scootersService;
        private readonly IAmqpService _amqpService;
        private readonly IRedisService _redisService;
        public ScootersController(IScootersService scootersService, IAmqpService amqpService, IRedisService redisService)
        {
            _scootersService = scootersService;
            _amqpService = amqpService;
            _redisService = redisService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Scooter>> GetAsync(string id)
        {
            try
            {
                string scooterKey = KEY_PREFIX + id;

                Scooter cachedScooter = _redisService.Get<Scooter>(scooterKey);

                if (cachedScooter != null)
                {
                    return cachedScooter;
                }

                Scooter scooter = await _scootersService.GetScooter(id);

                if (scooter == null) return NotFound();

                _redisService.Set(scooterKey, scooter);

                return scooter;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Scooter scooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string id = Guid.NewGuid().ToString();
                    scooter.IdScooter = id;

                    await _scootersService.AddScooter(scooter);

                    _redisService.Set(KEY_PREFIX + id, scooter);

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

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(string id, [FromBody] Scooter scooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    scooter.IdScooter = id;
                    await _scootersService.UpdateScooter(scooter);

                    _redisService.Set(KEY_PREFIX + id, scooter);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                Scooter scooter = await _scootersService.GetScooter(id);
                if (scooter == null) return NotFound();

                await _scootersService.DeleteScooter(id);
                _redisService.Delete(KEY_PREFIX + id);

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
                return await _scootersService.GetScooters();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
