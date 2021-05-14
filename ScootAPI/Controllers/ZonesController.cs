using MessagingLib;
using Microsoft.AspNetCore.Mvc;
using ScootAPI.Models;
using ScootAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cache;

namespace ScootAPI.Controllers
{
    [Route("api/[controller]")]
    public class ZonesController : ControllerBase
    {
        private const string KEY_PREFIX = "Zone:";
        private readonly IZonesService _zoneService;
        private readonly IAmqpService _amqpService;
        private readonly IRedisService _redisService;

        public ZonesController(IZonesService zonesService, IAmqpService amqpService, IRedisService redisService)
        {
            _zoneService = zonesService;
            _amqpService = amqpService;
            _redisService = redisService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Zone>> GetAsync(string id)
        {
            try
            {
                string zoneKey = KEY_PREFIX + id;

                Zone cachedZone = _redisService.Get<Zone>(zoneKey);

                if (cachedZone != null)
                {
                    return cachedZone;
                }

                Zone zone = await _zoneService.GetZone(id);

                if (zone == null) return NotFound();

                _redisService.Set(zoneKey, zone);

                return zone;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Zone zone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string id = Guid.NewGuid().ToString();
                    zone.IdZone = id;

                    await _zoneService.AddZone(zone);

                    _redisService.Set(KEY_PREFIX + id, zone);

                    _amqpService.SendMessage(new MessageEntity("ScootZone", id, "Create"));

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
        public async Task<IActionResult> EditAsync(string id, [FromBody] Zone zone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    zone.IdZone = id;
                    await _zoneService.UpdateZone(zone);

                    _redisService.Set(KEY_PREFIX + id, zone);

                    MessageEntity msg = new("ScootZone", id, "Update");
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

        [Route("{id}/scooters/cached")]
        public ActionResult<IEnumerable<Scooter>> GetScootersByZoneId(string id)
        {
            try
            {
                string zoneKey = "Zone/" + id;

                List<string> allScootersIds = _redisService.GetKeysByPattern("*Scooter:*");
                List<Scooter> scootersInZone = new();
                
                foreach (string scooterKey in allScootersIds)
                {
                    var scooter = _redisService.Get<Scooter>(scooterKey);
                    if (scooter.IdZone == id) scootersInZone.Add(scooter);
                }

                return scootersInZone;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("{id}/scooters")]
        public ActionResult<IEnumerable<Scooter>> GetByZoneId(string id)
        {
            try
            {
                return _zoneService.GetScootersByZoneId(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            Zone zone = await _zoneService.GetZone(id);
            if (zone == null) return NotFound();

            await _zoneService.DeleteZone(id);
            _redisService.Delete(KEY_PREFIX + id);

            MessageEntity msg = new("ScootZone", id, "Delete");
            _amqpService.SendMessage(msg);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zone>>> GetAsync()
        {
            try
            {
                return await _zoneService.GetZones();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
