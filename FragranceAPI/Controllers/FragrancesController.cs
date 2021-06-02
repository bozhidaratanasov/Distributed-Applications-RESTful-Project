using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public class FragrancesController : ControllerBase
    {
        private readonly IFragranceRepository _fragranceRepo;
        private readonly IMapper _mapper;

        public FragrancesController(IFragranceRepository fragranceRepo, IMapper mapper)
        {
            _fragranceRepo = fragranceRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<Fragrance>))]
        public IActionResult GetFragrances()
        {
            var fragranceList = _fragranceRepo.GetFragrances();
            var fragranceDtos = new List<FragranceDTO>();
            foreach (var item in fragranceList)
            {
                fragranceDtos.Add(_mapper.Map<FragranceDTO>(item));
            }
            return Ok(fragranceDtos);
        }

        [HttpGet("{fragranceId}", Name = "GetFragrance")]
        [ProducesResponseType(200, Type = typeof(Fragrance))]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetFragrance(int fragranceId)
        {
            var fragrance = _fragranceRepo.GetFragrance(fragranceId);
            if (fragrance == null)
                return NotFound();
            var fragranceDto = _mapper.Map<FragranceDTO>(fragrance);
            return Ok(fragranceDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Fragrance))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateFragrance([FromBody] FragranceDTO fragranceDTO)
        {
            if (fragranceDTO == null)
                return BadRequest(ModelState);

            if (_fragranceRepo.FragranceExists(fragranceDTO.Brand, fragranceDTO.Name))
            {
                ModelState.AddModelError("", "Fragrance already exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fragrance = _mapper.Map<Fragrance>(fragranceDTO);

            if (!_fragranceRepo.CreateFragrance(fragrance))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {fragrance.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetFragrance", new { fragranceId = fragrance.FragranceId }, fragrance);
        }

        [HttpPatch("{fragranceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateFragrance(int fragranceId, [FromBody] FragranceDTO fragranceDTO)
        {
            if (fragranceDTO == null || fragranceId!=fragranceDTO.FragranceId)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fragrance = _mapper.Map<Fragrance>(fragranceDTO);

            if (!_fragranceRepo.UpdateFragrance(fragrance))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {fragrance.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{fragranceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteFragrance(int fragranceId)
        {
            if (!_fragranceRepo.FragranceExists(fragranceId))
                return NotFound();

            var fragrance = _fragranceRepo.GetFragrance(fragranceId);
            if (!_fragranceRepo.DeleteFragrance(fragrance))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {fragrance.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
