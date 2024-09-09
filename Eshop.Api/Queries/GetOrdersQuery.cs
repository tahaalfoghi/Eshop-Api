using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetOrdersQuery:IRequest<IEnumerable<OrderDTO>>
    {
        public RequestParameter requestParameter { get; set; }

        public GetOrdersQuery(RequestParameter requestParameter)
        {
            this.requestParameter = requestParameter;
        }
    }
}
