using AutoMapper;
using eshop.DataAccess.Services.UnitOfWork;
using Eshop.Api.Commands;
using Eshop.DataAccess.Services.Middleware;
using Eshop.DataAccess.Services.Validators;
using Eshop.Models.Models;
using MediatR;

namespace Eshop.Api.Handlers
{
    public class GetUpdateProductHandler:IRequestHandler<UpdateProductRequest,bool>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetUpdateProductHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            if (request.productId <= 0) 
                throw new BadRequestException($"Invalid id [{request.productId}]");

            var validate = new ProductValidator();
            var result = validate.Validate(request.productPostDTO);
            if (!result.IsValid)
                throw new InvalidModelException($"{result.Errors.ToString()}");

            var productInDb = await uow.ProductRepository.GetByIdAsync(request.productId);
            if (productInDb is null)
                throw new NotFoundException($"Product [{request.productId}] doesn't exists");

            string? ImgUrl = string.Empty;
            if (request.productPostDTO.ImageUrl is not null)
            {
                var path = Path.Combine("wwwroot", "images", request.productPostDTO.ImageUrl.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.productPostDTO.ImageUrl.CopyToAsync(stream);
                }
                ImgUrl = $"/images/{request.productPostDTO.ImageUrl.FileName}";
            }
            productInDb = new Product
            {
                Name = request.productPostDTO.Name,
                Description = request.productPostDTO.Description,
                Price = request.productPostDTO.Price,
                ImageUrl = ImgUrl,
                CategoryId = request.productPostDTO.CategoryId,
            };
            await uow.ProductRepository.UpdateAsync(request.productId,productInDb);
            await uow.CommitAsync();

            return true;
        }
    }
}
