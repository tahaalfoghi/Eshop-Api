using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;
using Microsoft.CodeAnalysis;

namespace Eshop.Api.Handlers
{
    public class GetPatchUpdateProductHandler : IRequestHandler<UpdatePatchProductRequest,bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetPatchUpdateProductHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdatePatchProductRequest request, CancellationToken cancellationToken)
        {
            if (request.productId <= 0)
                throw new BadRequestException($"Invalid Id:{request.productId}");

            var existingProduct = await uow.ProductRepository.GetByIdAsync(request.productId, includes: "Category");
            if (existingProduct is null)
                throw new NotFoundException($"Product:[{request.productId}] doesn't exists");

            var dto_product = mapper.Map<ProductPostDTO>(existingProduct);
            request.patch.ApplyTo(dto_product);

            string? ImgUrl = string.Empty;
            if (dto_product.ImageUrl is null)
            {
               /* var validate = new ProductValidator();
                var result = validate.Validate(dto_product);
                if (!result.IsValid)
                    throw new InvalidModelException($"{string.Join(",", result.Errors)}");*/

                var product = new Product
                {
                    Name = dto_product.Name,
                    Description = dto_product.Description,
                    Price = dto_product.Price,
                    ImageUrl = ImgUrl,
                    CategoryId = dto_product.CategoryId,
                };
                await uow.ProductRepository.UpdatePatch(request.productId, product);
            }
            else
            {
                var path = Path.Combine("wwwroot", "images", dto_product.ImageUrl.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto_product.ImageUrl.CopyToAsync(stream);
                }
                ImgUrl = $"/images/{dto_product.ImageUrl.FileName}";

                var validate = new ProductValidator();
                var result = validate.Validate(dto_product);
                if (!result.IsValid)
                    throw new InvalidModelException($"{string.Join(",", result.Errors)}");

                var product = new Product
                {
                    Name = dto_product.Name,
                    Description = dto_product.Description,
                    Price = dto_product.Price,
                    ImageUrl = ImgUrl,
                    CategoryId = dto_product.CategoryId,
                };
                await uow.ProductRepository.UpdatePatch(request.productId, product);
            }

            await uow.CommitAsync();

            return true;
        }
    }
}
