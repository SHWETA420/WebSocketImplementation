using Model.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Chat
{
    public interface IChatService
    {
        Task<List<ChatMessage>> GetChatHistoryAsync();
        Task<ChatMessage> SaveMessageAsync(string sender, string message);
        Task<List<ChatMessage>> GetNewMessagesAsync(DateTime lastReceived);
    }
}
