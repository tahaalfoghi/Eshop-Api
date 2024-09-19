using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands.Create;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers.Create
{
    public class GetCreateProductHandler : IRequestHandler<CreateProductRequest, ProductDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetCreateProductHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<ProductDTO> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var validate = new ProductValidator();
            var result = validate.Validate(request.Product);
            if (!result.IsValid)
                throw new InvalidModelException($"Invalid model {result.Errors.ToString()}");

            string? ImgUrl = string.Empty;
            if (request.Product.ImageUrl is not null)
            {
                var path = Path.Combine("wwwroot", "images", request.Product.ImageUrl.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await request.Product.ImageUrl.CopyToAsync(stream);
                ImgUrl = $"/images/{request.Product.ImageUrl.FileName}";
            }
            var product = new Product
            {
                Name = request.Product.Name,
                Description = request.Product.Description,
                Price = request.Product.Price,
                ImageUrl = ImgUrl,
                CategoryId = request.Product.CategoryId,
            };
            await uow.ProductRepository.CreateAsync(product);
            await uow.CommitAsync();

            return mapper.Map<ProductDTO>(product);
        }
    }
}
