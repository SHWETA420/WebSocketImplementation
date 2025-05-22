// Services/Implementations/ChatService.cs
using Model;
using Model.Chat;
using Repository.Chat;
using Service.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Chat
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync()
        {
            return await _chatRepository.GetMessagesAsync();
        }

        public async Task<ChatMessage> SaveMessageAsync(string sender, string message)
        {
            var chatMessage = new ChatMessage
            {
                Sender = sender,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            return await _chatRepository.AddMessageAsync(chatMessage);
        }

        public async Task<List<ChatMessage>> GetNewMessagesAsync(DateTime lastReceived)
        {
            return await _chatRepository.GetMessagesAfterDateAsync(lastReceived);
        }
    }
}