﻿using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.BoardResolution;

public interface IBoardResolutionRepository
{
    Task<int> CreateAsync(BoardResolutionDto dto, byte[]? documentBytes);
    Task<int> UpdateAsync(BoardResolutionDto dto, byte[]? documentBytes);
    Task<int> DeleteAsync(int id, string createdBy);
    Task<BoardResolutionDto?> GetByIdAsync(int id);
    Task<PagedResult<BoardResolutionDto>> GetPagedAsync(string? search, bool? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate);
}

