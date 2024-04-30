using AutoMapper;
using FoodShop.Application.Common.Auth;
using FoodShop.Application.Common.Caching;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.Cart.CartQuery
{
    public class GetCartHandler : IQueryHandler<GetCartQuery, List<ProductResponseList>>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        public GetCartHandler(
            ICacheServices cacheServices, 
            IQueryRepository queryRepository,
            IMapper mapper, 
            ICurrentUser currentUser )
        {

            _queryRepository = queryRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task<Result<List<ProductResponseList>>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {

            var sqlQueryCartId = $"SELECT Id FROM Cart WHERE UserId = { _currentUser.GetUserId()}"; 
            var resultQueryCartId = await _queryRepository.QueryFirstOrDefaultAsync<string>(sqlQueryCartId);
            if (resultQueryCartId is null)
                throw new BadRequestException("An Error has occurred");
            var sqlQuery = $"SELECT * FROM Product WHERE Id IN (SELECT ProductId FROM CartProduct WHERE CartId = {resultQueryCartId})";
            var resultQuery = await _queryRepository.QueryAsync<Product>(sqlQuery);
            var finalResult = _mapper.Map<List<ProductResponseList>>(resultQuery);
            return finalResult;
        }
    }
}
