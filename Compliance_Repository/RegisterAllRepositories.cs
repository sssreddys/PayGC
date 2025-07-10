using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Repository.Agencies;
using Compliance_Repository.NetWorthCertificates;
using Compliance_Repository.Policies;
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
            services.AddScoped< IAgenciesRepository,AgenciesRepository>();
            services.AddScoped<IPolicyRepository,PolicyRepository>();
            services.AddScoped<INetWorthRepository, NetWorthRepository>();

        }
    }
}
