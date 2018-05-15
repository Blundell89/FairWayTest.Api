using AutoMapper;
using FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank.Responses;

namespace FairWayTest.Api.Infrastructure.AccountProviders.Bizfibank
{
    public class BizfiBankAccountToAccountProfile : Profile
    {
        public BizfiBankAccountToAccountProfile()
        {
            CreateMap<Account, Features.V1.Accounts.Account>();
        }
    }
}