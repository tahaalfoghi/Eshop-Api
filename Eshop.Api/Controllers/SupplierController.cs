using Eshop.Api.Commands;
using Eshop.Api.Queries;
using Eshop.DataAccess.DataShaping;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RiskFirst.Hateoas;


namespace Eshop.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Eshop-UI")]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IDataShaper<SupplierDTO> dataShaper;
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContextAccessor;
        public SupplierController(IMediator mediator, IDataShaper<SupplierDTO> dataShaper, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.dataShaper = dataShaper;
            this.linkGenerator = linkGenerator;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Route("Suppliers",Name = "GetSuppliers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [EnableRateLimiting("Fixed")]
        public async Task<IActionResult> GetSuppliers([FromQuery] RequestParameter requestParameter)
        {
            var query = new GetSupplliersQuery(requestParameter);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("Suppliers/{supplierId:int}",Name ="GetSupplier")]
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
        [Route("SuppliersByFilter")]
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
        [Route("CreateSupplier", Name = "CreateSupplier")]
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
        [Route("DeleteSupplier/{supplierId:int}", Name = "DeleteSupplier")]
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
        [Route("UpdateSupplier/{supplierId:int}", Name ="UpdateSupplier")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSupplier(int supplierId, SupplierDTO dto_supplier)
        {
            
            var command = new UpdateSupplierRequest(supplierId, dto_supplier);
            var supplierResult = await mediator.Send(command);

            return supplierResult ? Ok($"Supplier updated successfully") : BadRequest();
        }
        [HttpPatch]
        [Route("UpdatePatch/{supplierId:int}",Name = "UpdatePatchSupplier")]
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
        [HttpOptions]
        public async Task<IActionResult> GetSupplierOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,DELETE,UPDATE,PATCH");
            return Ok();
        }
    }
}
