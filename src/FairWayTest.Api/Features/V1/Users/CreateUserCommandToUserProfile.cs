using AutoMapper;

namespace FairWayTest.Api.Features.V1.Users
{
    public class CreateUserCommandToUserProfile : Profile
    {
        public CreateUserCommandToUserProfile()
        {
            CreateMap<CreateUser.Command, User>();

            CreateMap<CreateUser.BankDetails, BankDetails>();
        }
    }
}