using AutoMapper;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Category.Command
{
    public class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand, string>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public DeleteCategoryHandler(ICommandRepository commandRepository, IQueryRepository queryRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var sqlString = $"SELECT Id FROM Categories WHERE Title = {request.CategoryId}";
            var oldCategory = await _queryRepository.QuerySingleAsync<FoodShop.Domain.Entities.Category>(sqlString);
            if (oldCategory is null)
                throw new NotFoundException("Category doesn't exists");
            _commandRepository.Delete(oldCategory);
            return "Delete Category success";
        }
    }
}
