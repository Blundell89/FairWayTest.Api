using System;
using AutoMapper;

namespace FairWayTest.Api.Features.V1.Users
{
    public class CreateUserRequestToCreateUserCommandProfile : Profile
    {
        public CreateUserRequestToCreateUserCommandProfile()
        {
            CreateMap<Requests.CreateUserRequest, CreateUser.Command>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<Requests.BankDetails, CreateUser.BankDetails>();
        }
    }
}
