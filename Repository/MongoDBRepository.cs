using Microsoft.Extensions.Configuration;
using Model.Chat;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MongoDBRepository
    {
        private readonly IMongoDatabase _database;
        public MongoDBRepository(IConfiguration configuration)
        {

            var client = new MongoClient(configuration.GetSection("MongoDBSettings:ConnectionString").Value);

            _database = client.GetDatabase(configuration.GetSection("MongoDBSettings:DatabaseName").Value);
        }
        public IMongoCollection<ChatMessage> Message => _database.GetCollection<ChatMessage>("Message");
    }
}
