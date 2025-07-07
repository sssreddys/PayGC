using Compliance_Services.JWT;
using Compliance_Services.RbiNotifications;
using Compliance_Services.Regulator;
using Compliance_Services.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services
{
    public static class RegisterAllServices
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<RegulatorService>();
            services.AddScoped<IJwtTokenService ,JwtTokenService>();
            services.AddScoped<IRbiNotificationService,RbiNotificationService>();
        }
    }

}
