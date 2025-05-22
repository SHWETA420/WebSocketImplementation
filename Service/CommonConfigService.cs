using Microsoft.Extensions.DependencyInjection;
using Service.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CommonConfigService
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
        }
    }
}
