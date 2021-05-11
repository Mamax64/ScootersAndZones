using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScootAPI.Models;
using ScootAPI.Models.Messaging;
using ScootAPI.Repositories;
using ScootAPI.Services;
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

        public ZonesController(IZonesService zonesService, IAmqpService amqpService)
        {
            _zoneService = zonesService;
            _amqpService = amqpService;
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
        public ActionResult<Zone> Get(string id)
        {
            try
            {
                return _zoneService.GetZone(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Zone zone)
        {
            if (ModelState.IsValid)
            {
                string id = Guid.NewGuid().ToString();
                zone.IdZone= id;
                _zoneService.AddZone(zone);
                Message msg = new("ScootZone", id, "Create");
                _amqpService.SendMessage(msg);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id, [FromBody] Zone zone)
        {
            if (ModelState.IsValid)
            {
                zone.IdZone = id;
                _zoneService.UpdateZone(zone);
                Message msg = new("ScootZone", id, "Update");
                _amqpService.SendMessage(msg);
                return Ok();
            }
            return BadRequest();
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
            Message msg = new("ScootZone", id, "Delete");
            _amqpService.SendMessage(msg);
            return Ok();
        }
    }
}
