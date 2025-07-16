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

        public Task<int> CreateAsync(BoardResolutionDto dto, byte[]? documentBytes) => _repo.CreateAsync(dto, documentBytes);
        public Task<int> UpdateAsync(BoardResolutionDto dto, byte[]? documentBytes) => _repo.UpdateAsync(dto, documentBytes);
        public Task<int> DeleteAsync(int id, string createdBy) => _repo.DeleteAsync(id, createdBy);
        public Task<BoardResolutionDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<PagedResult<BoardResolutionDto>> GetPagedAsync(string? search, bool? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate) =>
            _repo.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);
    }

}
