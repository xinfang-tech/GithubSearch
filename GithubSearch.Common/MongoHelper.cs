using System;
using GithubSearch.Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GithubSearch.Common
{
    public class MongoContext
    {
        public MongoContext(string connectionName = "")
        {
            if (string.IsNullOrWhiteSpace(connectionName))
            {
                connectionName = ApplicationEnvironments.Site.MongoDB;
            }
            Client = new MongoClient(connectionName);
        }

        public MongoClient Client { get; set; }

        public IMongoDatabase DataBase { get => Client.GetDatabase("GithubSearch"); }

        public IMongoCollection<T> DbSet<T>(string name) where T : BaseMongoModel
        {
            return DataBase.GetCollection<T>(name);
        }
    }
}
