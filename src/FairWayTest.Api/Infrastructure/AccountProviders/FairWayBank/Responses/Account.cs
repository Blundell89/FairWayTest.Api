using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairWayTest.Api.Infrastructure.AccountProviders.FairWayBank.Responses
{
    public class Account
    {
        public string Name { get; set; }

        public Identifier Identifier { get; set; }
    }
}
