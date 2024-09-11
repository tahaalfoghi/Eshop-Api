﻿using Eshop.Api.Commands;
using Eshop.Api.Queries;
using Eshop.DataAccess.DataShaping;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace Eshop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IDataShaper<SupplierDTO> dataShaper;
        public SupplierController(IMediator mediator, IDataShaper<SupplierDTO> dataShaper)
        {
            this.mediator = mediator;
            this.dataShaper = dataShaper;
        }
        [HttpGet]
        [Route("suppliers")]
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
        [Route("suppliers/{supplierId:int}")]
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
        [Route("suppliers-by-filter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllByFilter([FromQuery] SupplierRequestParamater param)
        {
            var query = new GetSuppliersByFilterQuery(param);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        
        [HttpPost]
        [Route("create-supplier")]
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
        [Route("delete-supplier/{supplierId:int}")]
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
        [Route("update-supplier")]
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
        [Route("update-patch/{supplierId:int}")]
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
