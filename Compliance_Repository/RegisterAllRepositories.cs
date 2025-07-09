using Compliance_Dtos.AuditedFinancial;
using Compliance_Repository.RbiNotifications;
using Compliance_Repository.Regulator;
using Compliance_Repository.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository
{
    public static class RegisterAllRepositories
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRegulatorRepository, RegulatorRepository>();
            services.AddScoped<IVolumesValuesRepository, VolumesValuesRepository>();
            services.AddScoped<IBoardResolutionRepository, BoardResolutionRepository>();
            services.AddScoped<ICertificationRepository, CertificationRepository>();
            services.AddScoped<IAuditedFinancialRepository, AuditedFinancialRepository>();
            services.AddScoped< IRbiNotificationRepository,RbiNotificationRepository>();

        }
    }
}
