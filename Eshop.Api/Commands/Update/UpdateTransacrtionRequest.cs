﻿using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands.Update
{
    public class UpdateTransacrtionRequest : IRequest<bool>
    {
        public TransactionPostDTO TransactionDto { get; }
        public UpdateTransacrtionRequest(TransactionPostDTO TransactionDto)
        {
            this.TransactionDto = TransactionDto;
        }
    }
}
