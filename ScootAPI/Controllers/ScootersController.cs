using Microsoft.AspNetCore.Mvc;
using ScootAPI.Models;
using ScootAPI.Models.Messaging;
using ScootAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScootAPI.Controllers
{
    [Route("api/[controller]")]

    public class ScootersController : ControllerBase
    {
        private readonly IScootersService _scootersService;

        private readonly IAmqpService _amqpService;

        public ScootersController(IScootersService scootersService, IAmqpService amqpService)
        {
            _scootersService = scootersService;
            _amqpService = amqpService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Scooter>> Get(string id)
        {
            try
            {
                return _scootersService.GetScooter(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id, [FromBody] Scooter scooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    scooter.IdScooter = id;
                    _scootersService.UpdateScooter(scooter);
                    Message msg = new("Scooter", id, "Update");
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
        public IActionResult Create([FromBody] Scooter scooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string id = Guid.NewGuid().ToString();
                    scooter.IdScooter = id;
                    _scootersService.AddScooter(scooter);
                    Message msg = new("Scooter", id, "Create");
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
        public IActionResult Delete(string id)
        {
            Scooter scooter = _scootersService.GetScooter(id);
            if (scooter == null) return NotFound();
            _scootersService.DeleteScooter(id);
            Message msg = new("Scooter", id, "Delete");
            _amqpService.SendMessage(msg);
            return Ok();
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
