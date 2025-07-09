using Compliance_Services.JWT;
using Compliance_Services.RbiNotifications;
using Compliance_Services.Regulator;
using Compliance_Services.User;
using Compliance_Services.AuditedFincancial;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compliance_Services.Agencies;
using Compliance_Services.Policies;

namespace Compliance_Services
{
    public static class RegisterAllServices
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<RegulatorService>();
            services.AddScoped<IAuditedFinancialService, AuditedFinancialService>();
            services.AddScoped<IJwtTokenService ,JwtTokenService>();
            services.AddScoped<IRbiNotificationService,RbiNotificationService>();
            services.AddScoped<AgenciesService>();
            services.AddScoped<IPolicyService,PolicyService>();
        }
    }

}
