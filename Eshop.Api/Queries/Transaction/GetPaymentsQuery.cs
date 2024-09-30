using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries.Transaction
{
    public class GetPaymentsQuery : IRequest<IEnumerable<PaymentDTO>>
    {
        public RequestParameter requestParameter { get; set; }

        public GetPaymentsQuery(RequestParameter requestParameter)
        {
            this.requestParameter = requestParameter;
        }
    }
}
