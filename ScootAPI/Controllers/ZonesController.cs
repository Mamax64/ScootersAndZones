using MessagingLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ScootAPI.Models;
using ScootAPI.Repositories;
using ScootAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScootAPI.Controllers
{
    [Route("api/[controller]")]
    public class ZonesController : ControllerBase
    {
        private readonly IZonesService _zoneService;
        private readonly IAmqpService _amqpService;
        private readonly IDistributedCache _distributedCache;

        public ZonesController(IZonesService zonesService, IAmqpService amqpService, IDistributedCache distributedCache)
        {
            _zoneService = zonesService;
            _amqpService = amqpService;
            _distributedCache = distributedCache;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Zone>> Get(string id)
        {
            try
            {
                string zoneKey = "Zone/" + id;
                var serializedZone = await _distributedCache.GetStringAsync(zoneKey);

                if (serializedZone != null)
                {
                    return JsonConvert.DeserializeObject<Zone>(serializedZone);
                }

                Zone zone = _zoneService.GetZone(id);

                if (zone == null) return NotFound();

                await _distributedCache.SetStringAsync(zoneKey, JsonConvert.SerializeObject(zone));

                return zone;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Zone zone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string id = Guid.NewGuid().ToString();
                    zone.IdZone = id;

                    _zoneService.AddZone(zone);

                    await _distributedCache.SetStringAsync("Zone/" + id, JsonConvert.SerializeObject(zone));

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
                    _zoneService.UpdateZone(zone);

                    await _distributedCache.SetStringAsync("Zone/" + id, JsonConvert.SerializeObject(zone));
                    _distributedCache.
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
        public IActionResult Delete(string id)
        {
            Zone zone = _zoneService.GetZone(id);
            if (zone == null) return NotFound();
            _zoneService.DeleteZone(id);
            MessageEntity msg = new("ScootZone", id, "Delete");
            _amqpService.SendMessage(msg);
            return Ok();
        }
    }
}
