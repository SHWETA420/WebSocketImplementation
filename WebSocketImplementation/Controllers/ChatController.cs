// WebApi/Controllers/ChatController.cs
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Chat;
using Service.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSocketImplementation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<ChatMessage>>> GetChatHistory()
        {
            return Ok(await _chatService.GetChatHistoryAsync());
        }

        [HttpGet("new/{lastReceived}")]
        public async Task<ActionResult<List<ChatMessage>>> GetNewMessages(DateTime lastReceived)
        {
            return Ok(await _chatService.GetNewMessagesAsync(lastReceived));
        }

        [HttpPost]
        public async Task<ActionResult<ChatMessage>> SendMessage([FromBody] ChatMessage message)
        {
            if (string.IsNullOrEmpty(message.Sender) || string.IsNullOrEmpty(message.Message))
            {
                return BadRequest("Sender and Message are required.");
            }

            var savedMessage = await _chatService.SaveMessageAsync(message.Sender, message.Message);
            return Ok(savedMessage);
        }
    }
}