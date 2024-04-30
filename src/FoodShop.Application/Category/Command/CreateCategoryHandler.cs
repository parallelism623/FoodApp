using AutoMapper;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.Category.Command
{
    public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, string>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public CreateCategoryHandler(ICommandRepository commandRepository, IQueryRepository queryRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var sqlString = $"SELECT Id FROM Categories WHERE Title = {model.Title}";
            var checkCategory = await _queryRepository.QuerySingleAsync<FoodShop.Domain.Entities.Category>(sqlString);
            if (checkCategory is not null)
                throw new BadRequestException("Category exists");
            var newCategory = _mapper.Map<FoodShop.Domain.Entities.Category>(model);
            _commandRepository.AddAsync<FoodShop.Domain.Entities.Category>(newCategory);
            return "Add Category success";
        }
    }
}
