using Microsoft.AspNetCore.Mvc;
using Model.Chat;

namespace WebUI.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("WebSocketImplementation");
            var response = await client.GetAsync("api/chat/history");

            var messages = new List<ChatMessage>();
            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadFromJsonAsync<List<ChatMessage>>();
            }

            var viewModel = new ChatViewModel
            {
                Messages = messages,
                CurrentUser = User.Identity?.Name ?? "Anonymous"
            };

            return View(viewModel);
        }
    }
}
