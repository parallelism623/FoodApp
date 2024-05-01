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
    public class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, string>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public UpdateCategoryHandler(ICommandRepository commandRepository, IQueryRepository queryRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var sqlString = $"SELECT Id FROM Categories WHERE Id = {model.Id}";
            var oldCategory = await _queryRepository.QuerySingleAsync<FoodShop.Domain.Entities.Category>(sqlString);
            if (oldCategory is null)
                throw new NotFoundException("Category doesn't exists");
            var newCategory = _mapper.Map<FoodShop.Domain.Entities.Category>(model);
            _commandRepository.Update(newCategory);
            return "Update Category Success";
        }
    }
}
