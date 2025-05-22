using Model.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Chat
{
    public interface IChatRepository
    {
        Task<List<ChatMessage>> GetMessagesAsync();
        Task<ChatMessage> AddMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetMessagesAfterDateAsync(DateTime date);
    }
}
