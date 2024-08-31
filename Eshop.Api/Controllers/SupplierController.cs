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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
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
        public async Task<IActionResult> GetAllSuppliers()
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
        [Route("Supplier/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                throw new Exception($"id:{id} not a valid value");
            }
            var supplier =  await uow.SupplierRepository.GetByIdAsync(id,includes:"Categories");
            if(supplier is null)
            {
                return NotFound($"Supplier with id:{id} not found");
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

            return Ok(supplier.Id+ " New Supplier Created");
        }
        [HttpDelete]
        [Route("DeleteSupplier/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSupplier([FromRoute]int id)
        {
            if (id <= 0)
            {
                throw new Exception($"Invalid id value:{id}");
            }
            var supplier = await uow.SupplierRepository.GetByIdAsync(id);
            if(supplier is null)
            {
                return BadRequest($"Supplier with id:{id} is not found");
            }
            uow.SupplierRepository.DeleteAsync(supplier);
            await uow.CommitAsync();

            return Ok($"supplier with id: {supplier.Id} deleted successfully");
        }
        [HttpPut]
        [Route("UpdateSupplier/{Id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSupplier(int Id,SupplierDTO dto_supplier)
        {
            var validate = new SupplierValidator();
            var result =  validate.Validate(dto_supplier);
            if (!result.IsValid)
            {
                return BadRequest(result.ToString());
            }
            if (!ModelState.IsValid || Id<=0)
            {
                throw new Exception($"Invalid data for supplier: {dto_supplier.ToString()} or id:{Id}");
            }
            var existingsupplier = await uow.SupplierRepository.GetByIdAsync(Id);
            if(existingsupplier is null)
            {
                return BadRequest($"Supplier with id:{dto_supplier.Id} is not found");
            }
            var supplier = mapper.Map<Supplier>(dto_supplier);
            await uow.SupplierRepository.UpdateAsync(Id,supplier);
            await uow.CommitAsync();

            return Ok($"Supplier {Id} updated successfully\n Body:{supplier.ToString()}");
        }
        [HttpPatch]
        [Route("UpdatePatch/{Id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePatch(int Id,[FromBody] JsonPatchDocument<SupplierDTO> patch)
        {
            if(!ModelState.IsValid || Id <= 0)
            {
                throw new Exception($"ERROR Invalid model:{patch.ToString()} or Id:{Id} values");
            }
            var existingSupplier = await uow.SupplierRepository.GetByIdAsync(Id);
            if(existingSupplier is null)
            {
                return BadRequest($"Supplier with Id: {Id} is not found");
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
            await uow.SupplierRepository.UpdatePatchAsync(Id,supplier);
            await uow.CommitAsync();

            return Ok($"Supplier with Id:{supplier.Id} patch updated successfully");
        }
    }
}
