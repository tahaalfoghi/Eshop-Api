using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Eshop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public SupplierController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("Suppliers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await uow.SupplierRepository.GetAllAsync(includes: "Categories");
            if(suppliers is null)
            {
                return NotFound("Suppliers not found");
            }
            var dto_suppliers = mapper.Map<List<SupplierDTO>>(suppliers);
            return Ok(dto_suppliers);
        }
        [HttpGet]
        [Route("Suppliers/{supplierId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSupplier(int supplierId)
        {
            if (supplierId <= 0)
            {
                throw new Exception($"id:{supplierId} not a valid value");
            }
            var supplier =  await uow.SupplierRepository.GetByIdAsync(supplierId, includes:"Categories");
            if(supplier is null)
            {
                return NotFound($"Supplier with id:{supplierId} not found");
            }
            var dto_supplier = mapper.Map<SupplierDTO>(supplier);
            return Ok(dto_supplier);
        }
        [HttpGet]
        [Route("SupppliersByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllByFilter([FromQuery]TableSearch search)
        {
            var suppliers = await uow.SupplierRepository.GetAllByFilterAsync(search,includes:"Categories");
            if(suppliers is null)
            {
                return NotFound();
            }
            var dto_records = mapper.Map<List<SupplierDTO>>(suppliers);
            return Ok(dto_records);
        }
        [HttpGet]
        [Route("SupplierByFilter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSingleByFilter([FromQuery] TableSearch search)
        {
            var supplier = await uow.SupplierRepository.GetFirstOrDefaultAsync(search);
            if(supplier is null)
            {
                return NotFound();
            }
            var dto_supplier = mapper.Map<SupplierDTO>(supplier);
            return Ok(dto_supplier);
        }
        [HttpPost]
        [Route("CreateSupplier")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSupplier(SupplierDTO dto_supplier)
        {
            var validate = new SupplierValidator();
            var result = validate.Validate(dto_supplier);
            if (!result.IsValid)
            {
                return BadRequest(result.ToString());
            }
            if (!ModelState.IsValid)
            {
                throw new Exception($"ERROR Invalid model for supplier:{{ {dto_supplier.ToString()} }}");
            }
            var supplier = mapper.Map<Supplier>(dto_supplier);
            await uow.SupplierRepository.CreateAsync(supplier);
            await uow.CommitAsync();

            return CreatedAtAction(nameof(GetSupplier), new {supplierId = supplier.Id}, supplier);
        }
        [HttpDelete]
        [Route("DeleteSupplier/{supplierId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSupplier([FromRoute]int supplierId)
        {
            if (supplierId <= 0)
            {
                throw new Exception($"Invalid id value:{supplierId}");
            }
            var supplier = await uow.SupplierRepository.GetByIdAsync(supplierId);
            if(supplier is null)
            {
                return BadRequest($"Supplier with id:{supplierId} is not found");
            }
            uow.SupplierRepository.Delete(supplier);
            await uow.CommitAsync();

            return Ok($"supplier with id: {supplier.Id} deleted successfully");
        }
        [HttpPut]
        [Route("UpdateSupplier/{supplierId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSupplier(int supplierId, SupplierDTO dto_supplier)
        {
            var validate = new SupplierValidator();
            var result =  validate.Validate(dto_supplier);
            if (!result.IsValid)
            {
                return BadRequest(result.ToString());
            }
            if (!ModelState.IsValid || supplierId <= 0)
            {
                throw new Exception($"Invalid data for supplier: {dto_supplier.ToString()} or id:{supplierId}");
            }
            var existingsupplier = await uow.SupplierRepository.GetByIdAsync(supplierId);
            if(existingsupplier is null)
            {
                return BadRequest($"Supplier with id:{dto_supplier.Id} is not found");
            }
            var supplier = mapper.Map<Supplier>(dto_supplier);
            await uow.SupplierRepository.UpdateAsync(supplierId, supplier);
            await uow.CommitAsync();

            return Ok($"Supplier {supplierId} updated successfully\n Body:{supplier.ToString()}");
        }
        [HttpPatch]
        [Route("UpdatePatch/{supplierId:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePatch(int supplierId, [FromBody] JsonPatchDocument<SupplierDTO> patch)
        {
            if(!ModelState.IsValid || supplierId <= 0)
            {
                throw new Exception($"ERROR Invalid model:{patch.ToString()} or Id:{supplierId} values");
            }
            var existingSupplier = await uow.SupplierRepository.GetByIdAsync(supplierId);
            if(existingSupplier is null)
            {
                return BadRequest($"Supplier with Id: {supplierId} is not found");
            }
            var dto_supplier = mapper.Map<SupplierDTO>(existingSupplier);
            patch.ApplyTo(dto_supplier, ModelState);

            var validate = new SupplierValidator();
            var result = validate.Validate(dto_supplier);
            if (!result.IsValid)
            {
                return BadRequest(result.ToString());
            }

            var supplier = mapper.Map<Supplier>(dto_supplier);
            await uow.SupplierRepository.UpdatePatchAsync(supplierId, supplier);
            await uow.CommitAsync();

            return Ok($"Supplier with Id:{supplier.Id} patch updated successfully");
        }
    }
}
