﻿using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Product
{
    public class GetProductQuery : IRequest<ProductDTO>
    {
        public int ProductId { get; set; }
        public GetProductQuery(int ProductId)
        {
            this.ProductId = ProductId;
        }
    }
}
