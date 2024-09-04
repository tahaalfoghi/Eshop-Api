using Eshop.Models;
using MediatR;

namespace Eshop.Api.Queries
{
    public class GetUserQuery:IRequest<UserModelDTO>
    {
        public string userId { get; set; }

        public GetUserQuery(string userId)
        {
            this.userId = userId;
        }
    }
}
