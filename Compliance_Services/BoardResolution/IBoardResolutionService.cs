using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Common;

public interface IBoardResolutionService
{
    Task<int> CreateAsync(CreateBoardResolutionDto dto, byte[]? documentBytes, string created_by);
    Task<int> UpdateAsync(UpdateBoardResolutionDto dto, byte[]? documentBytes, string updatedBy);
    Task<int> DeleteAsync(DeleteRequestDto dto, string updatedBy);
    Task<BoardResolutionDto?> GetByIdAsync(int id);
    Task<PagedResult<BoardResolutionDto>> GetPagedAsync(string? search, string? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
}

