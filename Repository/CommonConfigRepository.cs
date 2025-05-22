using Microsoft.Extensions.DependencyInjection;
using Repository.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CommonConfigRepository
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<MongoDBRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
        }
    }
}
