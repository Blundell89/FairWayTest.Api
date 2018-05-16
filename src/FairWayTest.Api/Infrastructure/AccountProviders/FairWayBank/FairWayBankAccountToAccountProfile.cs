using AutoMapper;
using FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses;

namespace FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank
{
    public class FairWayBankAccountToAccountProfile : Profile
    {
        public FairWayBankAccountToAccountProfile()
        {
            CreateMap<Account, Features.V1.Accounts.Account>()
                .ForMember(x => x.AccountNumber, opt => opt.MapFrom(x => x.Identifier.AccountNumber))
                .ForMember(x => x.SortCode, opt => opt.MapFrom(x => x.Identifier.SortCode))
                .ForMember(x => x.AvailableBalance, opt => opt.Ignore())
                .ForMember(x => x.Balance, opt => opt.Ignore())
                .ForMember(x => x.Overdraft, opt => opt.Ignore());

            CreateMap<Balance, Features.V1.Accounts.Account>()
                .ForMember(x => x.Overdraft, opt => opt.MapFrom(x => x.Overdraft.Amount))
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.AccountNumber, opt => opt.Ignore())
                .ForMember(x => x.SortCode, opt => opt.Ignore())
                .ForMember(x => x.Balance, opt => opt.ResolveUsing((balance, account) =>
                {
                    var isInDebit = balance.Type == "Debit";

                    return isInDebit ? -balance.Amount : balance.Amount;
                }))
                .ForMember(x => x.AvailableBalance, opt => opt.ResolveUsing((balance, account) => balance.Amount + balance.Overdraft.Amount));
        }
    }
}