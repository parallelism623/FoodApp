using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
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
    public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public GetUserByIdHandler(IQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            string querySql = $"SELECT * FROM {nameof(AppUser)} WHERE Id = {request.Id}";
            var result = await _queryRepository.QuerySingleAsync<AppUser>(querySql);
            if (result == null)
            {
                throw new NotFoundException($"User not found by Id is {request.Id}");
            }
            var finalResult = _mapper.Map<UserResponse>(result);
            return finalResult;
        }
    }
}
