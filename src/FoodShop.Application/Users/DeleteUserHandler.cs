using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities.Identity;
using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Users
{
    public class DeleteUserHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly ICommandRepository _commandRepository;
        public DeleteUserHandler(IQueryRepository queryRepository, ICommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var querySql = $"SELECT * FROM {nameof(AppUser)} WHERE Id = {request.Id}";
            var result = await _queryRepository.QueryFirstOrDefaultAsync<AppUser>(querySql);
            if (result == null) 
            {
                throw new NotFoundException($"User not found by id is {request.Id}");
            }
            _commandRepository.Delete<AppUser>(result);
            return Result.Success();
        }
    }
}
