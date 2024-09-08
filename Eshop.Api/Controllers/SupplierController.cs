using AutoMapper;
using eshop.DataAccess.Data;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.Api.Queries;
using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Eshop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator mediator;
        public SupplierController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("Suppliers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSuppliers([FromQuery] RequestParameter requestParameter)
        {
            var query = new GetSupplliersQuery(requestParameter);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("Suppliers/{supplierId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSupplier(int supplierId)
        {
            
            var query = new GetSuppllierQuery(supplierId);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("SupppliersByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllByFilter([FromQuery] TableSearch search)
        {
            var query = new GetSuppliersByFilterQuery(search);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("SupplierByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSingleByFilter([FromQuery] TableSearch search)
        {
            var query = new GetSupplierByFilterQuery(search);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpPost]
        [Route("CreateSupplier")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSupplier(SupplierDTO dto_supplier)
        {
            var command = new CreateSupplierRequest(dto_supplier);
            var supplierResult = await mediator.Send(command);

            return CreatedAtAction(nameof(GetSupplier), new {supplierId = supplierResult.Id}, supplierResult);
        }
        [HttpDelete]
        [Route("DeleteSupplier/{supplierId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSupplier([FromRoute] int supplierId)
        {

            var command = new DeleteSupplierRequest(supplierId);
            var result = await mediator.Send(command);

            return result ? Ok($"supplier with id: {supplierId} deleted successfully") : BadRequest();
        }
        [HttpPut]
        [Route("UpdateSupplier")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSupplier(SupplierDTO dto_supplier)
        {
            
            var command = new UpdateSupplierRequest(dto_supplier);
            var supplierResult = await mediator.Send(command);

            return supplierResult ? Ok($"Supplier updated successfully") : BadRequest();
        }
        [HttpPatch]
        [Route("UpdatePatch/{supplierId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePatch(int supplierId, [FromBody] JsonPatchDocument<SupplierDTO> patch)
        {
           
           var command = new UpdatePatchSupplierRequest(supplierId, patch);
           var result = await mediator.Send(command);
           return result ? Ok($"Supplier updated successfully") : BadRequest();
        }
    }
}
