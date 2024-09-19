using Eshop.Models;
using MediatR;

namespace Eshop.Api.Commands.Update
{
    public class UpdateUserRequest : IRequest<bool>
    {

        public UserModelDTO userDto { get; set; }

        public UpdateUserRequest(UserModelDTO userDto)
        {
            this.userDto = userDto;
        }
    }
}
