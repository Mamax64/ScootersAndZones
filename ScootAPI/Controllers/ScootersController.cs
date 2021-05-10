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
        private readonly IScootersService _scootersService;

        public ScootersController(IScootersService scootersService)
        {
            _scootersService = scootersService;
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
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        //////// NON OFFICIEL 

        [HttpPost]
        public IActionResult Create([FromBody] Scooter scooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid obj = Guid.NewGuid();
                    scooter.IdScooter = obj.ToString();
                    _scootersService.AddScooter(scooter);
                    return Ok();
                }
                return BadRequest();
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
