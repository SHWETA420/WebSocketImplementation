using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Chat
{
    public class ChatViewModel
    {
        public List<ChatMessage> Messages { get; set; }
        public string CurrentUser { get; set; }
        public string NewMessage { get; set; }
    }
}
