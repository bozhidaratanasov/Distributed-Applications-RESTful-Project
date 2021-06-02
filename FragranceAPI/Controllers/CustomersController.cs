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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CustomerDTO>))]
        public IActionResult GetCustomers()
        {
            var customersList = _customerRepo.GetCustomers();
            var customersDtos = new List<CustomerDTO>();
            foreach (var item in customersList)
            {
                customersDtos.Add(_mapper.Map<CustomerDTO>(item));
            }

            return Ok(customersDtos);
        }

        [HttpGet("{customerId}", Name ="GetCustomer")]
        [ProducesResponseType(200, Type = typeof(CustomerDTO))]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetCustomer(int customerId)
        {
            var customer = _customerRepo.GetCustomer(customerId);
            if (customer == null)
                return NotFound();
            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return Ok(customerDTO);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Customer))]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCustomer([FromBody] CustomerDTO customerDTO)
        {
            if (customerDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _mapper.Map<Customer>(customerDTO);

            if (!_customerRepo.CreateCustomer(customer))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {customer.FirstName} {customer.LastName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCustomer", new { customerId = customer.CustomerId }, customer);
        }

        [HttpPatch("{customerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCustomer(int customerId, [FromBody] CustomerDTO customerDTO)
        {
            if (customerDTO == null || customerId != customerDTO.CustomerId)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _mapper.Map<Customer>(customerDTO);
            if (!_customerRepo.UpdateCustomer(customer))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {customer.FirstName} {customer.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{customerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCustomer(int customerId)
        {
            if (!_customerRepo.CustomerExists(customerId))
                return NotFound();

            var customer = _customerRepo.GetCustomer(customerId);
            if (!_customerRepo.DeleteCustomer(customer))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {customer.FirstName} {customer.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}
