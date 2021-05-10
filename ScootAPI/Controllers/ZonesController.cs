using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScootAPI.Models;
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

        public ZonesController(IZonesService zonesService)
        {
            _zoneService = zonesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zone>>> Get()
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
                Guid obj = Guid.NewGuid();
                zone.IdZone = obj.ToString();
                _zoneService.AddZone(zone);
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
                return Ok();
            }
            return BadRequest();
        }

        [Route("{id}/scooters")]
        public async Task<ActionResult<IEnumerable<Scooter>>> GetByZoneId(string id)
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
    }
}
