using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetSuppliersByFilterQuery:IRequest<IEnumerable<SupplierDTO>>
    {
        public SupplierRequestParamater Param {  get; set; }

        public GetSuppliersByFilterQuery(SupplierRequestParamater Param)
        {
            this.Param = Param;
        }
    }
}
