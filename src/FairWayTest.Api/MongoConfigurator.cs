using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Conventions;

namespace FairWayTest.Api
{
    public class MongoConfigurator
    {
        public void Configure()
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("FW Conventions", pack, t => t.FullName.StartsWith("FairWayTest.Api"));
        }
    }
}
