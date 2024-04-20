using AutoMapper;
using FoodShop.Application.Abstraction.Messaging;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Respone.V1;
using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodShop.Application.Users.Authentication
{
    public class RegisterHandler : ICommandHandler<RegisterCommand, UserAuthResponse>
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IMapper _mapper;
        public RegisterHandler(IAuthenticationServices authenticationServices,
                                      IMapper mapper)
        {
            _mapper = mapper;
            _authenticationServices = authenticationServices;
        }
        public async Task<Result<UserAuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var result = await _authenticationServices.Register(model);
            if (result == null)
            {
                throw new BadRequestException(MessengerResult.EmailExit);
            }
            var userReturn = _mapper.Map<UserAuthResponse>(result);
            return userReturn;
        }
    }
}
