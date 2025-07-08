using Compliance_Services.JWT;
using Compliance_Services.RbiNotifications;
using Compliance_Services.Regulator;
using Compliance_Services.User;
using Compliance_Services.AuditedFincancial;
using Compliance_Services.BoardResolution;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compliance_Services.VolumesValues;

namespace Compliance_Services
{
    public static class RegisterAllServices
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<RegulatorService>();
            services.AddScoped<IAuditedFinancialService, AuditedFinancialService>();
            services.AddScoped<IVolumesValuesService, VolumesValuesService>(); 
            services.AddScoped<IBoardResolutionService, BoardResolutionService>();
            services.AddScoped<IJwtTokenService ,JwtTokenService>();
            services.AddScoped<IRbiNotificationService,RbiNotificationService>();
        }
    }

}
