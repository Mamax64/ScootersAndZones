using MessagingLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ScootAPI.Models;
using ScootAPI.Services;
using StackExchange.Redis.Extensions.Core.Abstractions;
using ScootAPI.Extentions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
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
        public ActionResult<Zone> Get(string id)
        {
            try
            {
                string zoneKey = KEY_PREFIX + id;

                Zone cachedZone = _redisService.Get<Zone>(zoneKey);

                if (cachedZone != null)
                {
                    return cachedZone;
                }

                Zone zone = _zoneService.GetZone(id);

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
        public IActionResult Create([FromBody] Zone zone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string id = Guid.NewGuid().ToString();
                    zone.IdZone = id;

                    _zoneService.AddZone(zone);

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
        public IActionResult Edit(string id, [FromBody] Zone zone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    zone.IdZone = id;
                    _zoneService.UpdateZone(zone);

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
        public async Task<IActionResult> Delete(string id)
        {
            Zone zone = _zoneService.GetZone(id);
            if (zone == null) return NotFound();

            _zoneService.DeleteZone(id);
            _redisService.Delete(KEY_PREFIX + id);

            MessageEntity msg = new("ScootZone", id, "Delete");
            _amqpService.SendMessage(msg);
            return Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Zone>> Get()
        {
            try
            {
                return _zoneService.GetZones();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
