using FoodShop.Domain.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!IsCommand(request))
            {
                return await next();
            }
            using var transaction = await _unitOfWork.BeginTransaction();
            var respone = await next();
            await _unitOfWork.SaveChangesAsync();
            transaction.Commit();
            return respone;
        }
        private bool IsCommand(TRequest request)
        {
            return nameof(request).EndsWith("Command");
        }
    }
}
