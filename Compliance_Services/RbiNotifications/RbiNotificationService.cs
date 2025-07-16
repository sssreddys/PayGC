using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using Compliance_Dtos.RbiNotifications;
using Compliance_Dtos.Regulator;
using Compliance_Repository.RbiNotifications;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.RbiNotifications
{
    public class RbiNotificationService : IRbiNotificationService
    {
        private readonly IRbiNotificationRepository _repository;

        public RbiNotificationService(IRbiNotificationRepository repository)
        {
            _repository = repository;
        }
        public Task<int> CreateAsync(CreateRbiNotificationDto dto, byte[]? documentBytes, string created_by) => _repository.CreateAsync(dto, documentBytes, created_by);
        public Task<PagedResult<RbiNotificationDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
          => _repository.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);

        public Task<int> UpdateAsync(UpdateRbiNotificationDto dto, byte[]? documentBytes, string updatedBy) => _repository.UpdateAsync(dto, documentBytes,  updatedBy);
        public Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy) => _repository.DeleteAsync(dto, updatedBy);
        public Task<RbiNotificationDto> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    }
}

