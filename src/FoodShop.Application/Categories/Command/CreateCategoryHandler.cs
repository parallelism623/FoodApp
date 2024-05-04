using AutoMapper;
using FoodShop.Application.Common.Notifications;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Notification;
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
        private readonly INotificationSender _notificationSender;
        public CreateCategoryHandler(
            ICommandRepository commandRepository, 
            IQueryRepository queryRepository, 
            IMapper mapper,
            INotificationSender notificationSender)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _mapper = mapper;
            _notificationSender = notificationSender;
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
            _notificationSender.SendToAllAsync(new BaseNotificationMessage(MessageNotification.CreateNewCategories(model.Title)));
            return "Add Category success";
        }
    }
}
