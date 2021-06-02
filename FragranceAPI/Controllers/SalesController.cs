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
    [Authorize]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class SalesController : ControllerBase
    {
            private readonly ISaleRepository _saleRepo;
            private readonly IMapper _mapper;

            public SalesController(ISaleRepository saleRepo, IMapper mapper)
            {
                _saleRepo = saleRepo;
                _mapper = mapper;
            }

            [HttpGet]
            [ProducesResponseType(200, Type = typeof(List<SaleDTO>))]
            public IActionResult GetSales()
            {
                var salesList = _saleRepo.GetSales();
                var salesDtos = new List<SaleDTO>();
                foreach (var item in salesList)
                {
                    salesDtos.Add(_mapper.Map<SaleDTO>(item));
                }

                return Ok(salesDtos);
            }

            [HttpGet("{saleId}", Name = "GetSale")]
            [ProducesResponseType(200, Type = typeof(SaleDTO))]
            [ProducesResponseType(404)]
            [Authorize(Roles = "Admin")]
            public IActionResult GetSale(int saleId)
            {
                var sale = _saleRepo.GetSale(saleId);
                if (sale == null)
                    return NotFound();
                var saleDTO = _mapper.Map<SaleDTO>(sale);
                return Ok(saleDTO);
            }

            [HttpPost]
            [ProducesResponseType(201, Type = typeof(SaleCreateDTO))]
            [ProducesResponseType(500)]
            [Authorize(Roles = "Admin")]
            public IActionResult CreateSale([FromBody] SaleCreateDTO saleDTO)
            {
                if (saleDTO == null)
                    return BadRequest(ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var sale = _mapper.Map<Sale>(saleDTO);

                if (!_saleRepo.CreateSale(sale))
                {
                    ModelState.AddModelError("", $"Something went wrong when saving the record ");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtRoute("GetSale", new { saleId = sale.SaleId }, sale);
            }

            [HttpPatch("{saleId}")]
            [ProducesResponseType(204)]
            [ProducesResponseType(500)]
            [Authorize(Roles = "Admin")] 
            public IActionResult UpdateSale(int saleId, [FromBody] SaleUpdateDTO saleDTO)
            {
                if (saleDTO == null || saleId != saleDTO.SaleId)
                    return BadRequest(ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var sale = _mapper.Map<Sale>(saleDTO);
                if (!_saleRepo.UpdateSale(sale))
                {
                    ModelState.AddModelError("", $"Something went wrong when updating the record {sale.SaleId}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }

            [HttpDelete("{saleId}")]
            [ProducesResponseType(204)]
            [ProducesResponseType(404)]
            [ProducesResponseType(500)]
            [Authorize(Roles = "Admin")]
            public IActionResult DeleteSale(int saleId)
            {
                if (!_saleRepo.SaleExists(saleId))
                    return NotFound();

                var sale = _saleRepo.GetSale(saleId);
                if (!_saleRepo.DeleteSale(sale))
                {
                    ModelState.AddModelError("", $"Something went wrong when deleting the record {sale.SaleId}");
                    return StatusCode(500, ModelState);
                }

                return NoContent();

            }
        }
}
