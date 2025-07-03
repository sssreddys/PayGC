using Compliance_Services.Regulator;
using Compliance_Services.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services
{
    public static class Register
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<RegulatorService>();
        }
    }

}
