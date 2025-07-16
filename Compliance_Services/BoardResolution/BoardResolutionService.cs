using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Services.BoardResolution
{
    public class BoardResolutionService : IBoardResolutionService
    {
        private readonly IBoardResolutionRepository _repo;
        public BoardResolutionService(IBoardResolutionRepository repo) => _repo = repo;

        public Task<int> CreateAsync(CreateBoardResolutionDto dto, byte[]? documentBytes, string created_by) => _repo.CreateAsync(dto, documentBytes, created_by);
        public Task<int> UpdateAsync(UpdateBoardResolutionDto dto, byte[]? documentBytes, string updatedBy) => _repo.UpdateAsync(dto, documentBytes, updatedBy);
        public Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy) => _repo.DeleteAsync(dto, updatedBy);
        public Task<BoardResolutionDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<PagedResult<BoardResolutionDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate) =>
            _repo.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);
    }

}
