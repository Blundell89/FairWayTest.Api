using System;
using System.Reflection;
using AutoFixture.Kernel;
using FairWayTest.Api.Features.V1.Users.Requests;

namespace FairWayTest.Api.FunctionalTests
{
    public class AccountNumberBuilder : ISpecimenBuilder
    {
        private static readonly Random _random = new Random();

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi != null &&
                pi.Name == nameof(BankDetails.AccountNumber) &&
                pi.PropertyType == typeof(string))

                return _random.Next(10000000, 99999999).ToString();

            return new NoSpecimen();
        }
    }
}