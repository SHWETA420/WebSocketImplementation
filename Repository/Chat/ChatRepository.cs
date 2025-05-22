// Repository/Implementations/ChatRepository.cs
using MongoDB.Driver;
using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Chat;

namespace Repository.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<ChatMessage> _messages;

        public ChatRepository(MongoDBRepository database)
        {
            _messages = database.Message;
        }

        public async Task<List<ChatMessage>> GetMessagesAsync()
        {
            return await _messages.Find(_ => true).ToListAsync();
        }

        public async Task<ChatMessage> AddMessageAsync(ChatMessage message)
        {
            await _messages.InsertOneAsync(message);
            return message;
        }

        public async Task<List<ChatMessage>> GetMessagesAfterDateAsync(DateTime date)
        {
            return await _messages.Find(m => m.Timestamp > date).ToListAsync();
        }
    }
}