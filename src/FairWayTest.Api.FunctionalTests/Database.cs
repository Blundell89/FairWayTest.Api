using System;
using System.Collections.Generic;
using System.Text;
using FairWayTest.Api.Features.V1.Users;
using MongoDB.Driver;

namespace FairWayTest.Api.FunctionalTests
{
    public static class Database
    {
        private static readonly IMongoDatabase _database;

        static Database()
        {
            var mongoUrl = new MongoUrl(Configuration.ConnectionString);
            _database = new MongoClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName);
        }

        private static IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);

        public static IMongoCollection<CreateUser.Command> Users => GetCollection<CreateUser.Command>("users");
    }
}