using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Common;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

public class BoardResolutionRepository : IBoardResolutionRepository
{
    private readonly string _conn;
    public BoardResolutionRepository(IConfiguration config) => _conn = config.GetConnectionString("DefaultConnection");

    public async Task<int> CreateAsync(BoardResolutionDto dto, byte[]? doc)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Date", dto.Date);
        p.Add("@Number", dto.Number);
        p.Add("@Purpose", dto.Purpose);
        p.Add("@Location", dto.Location);
        p.Add("@MeetingTime", dto.MeetingTime);
        p.Add("@ReviewedBy", dto.ReviewedBy);
        p.Add("@ReviewedDate", dto.ReviewedDate);
        p.Add("@ApprovedBy", dto.ApprovedBy);
        p.Add("@ApprovedDate", dto.ApprovedDate);
        p.Add("@AttachedDocument", doc, DbType.Binary);
        p.Add("@CreatedBy", dto.CreatedBy);
        p.Add("@CreatedAt", dto.CreatedAt);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_create_board_resolution", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }


    public async Task<int> UpdateAsync(BoardResolutionDto dto, byte[]? doc)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();

        p.Add("@Id", dto.BoardResolutionId, DbType.Int32);
        p.Add("@Date", dto.Date, DbType.Date);
        p.Add("@Number", dto.Number, DbType.String);
        p.Add("@Purpose", dto.Purpose, DbType.String);
        p.Add("@Location", dto.Location, DbType.String);
        p.Add("@MeetingTime", dto.MeetingTime, DbType.Time);
        p.Add("@ReviewedBy", dto.ReviewedBy, DbType.String);
        p.Add("@ReviewedDate", dto.ReviewedDate, DbType.Date);
        p.Add("@ApprovedBy", dto.ApprovedBy, DbType.String);
        p.Add("@ApprovedDate", dto.ApprovedDate, DbType.Date);
        p.Add("@AttachedDocument", doc, DbType.Binary);

        // NOTE: Use UpdatedBy parameter (you may need to change your dto or pass separately)
        p.Add("@UpdatedBy", dto.CreatedBy, DbType.String); // or pass UpdatedBy as a separate arg if different from CreatedBy

        p.Add("@UpdatedAt", DateTime.UtcNow, DbType.DateTime);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_update_board_resolution", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }


    public async Task<int> DeleteAsync(int id, string createdBy)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Id", id);
        p.Add("@CreatedBy", createdBy);
        p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("sp_delete_board_resolution", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ReturnVal");
    }

    public async Task<BoardResolutionDto?> GetByIdAsync(int id)
    {
        using var db = new SqlConnection(_conn);
        return await db.QueryFirstOrDefaultAsync<BoardResolutionDto>(
            "sp_get_board_resolution_by_id", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedResult<BoardResolutionDto>> GetPagedAsync(string? search, bool? status, int page, int pageSize, DateTime? fromDate, DateTime? toDate)
    {
        using var db = new SqlConnection(_conn);
        var p = new DynamicParameters();
        p.Add("@Search", search);
        p.Add("@Status", status);
        p.Add("@Page", page);
        p.Add("@PageSize", pageSize);
        p.Add("@FromDate", fromDate);
        p.Add("@ToDate", toDate);

        using var multi = await db.QueryMultipleAsync("sp_get_all_board_resolutions", p, commandType: CommandType.StoredProcedure);
        var data = await multi.ReadAsync<BoardResolutionDto>();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResult<BoardResolutionDto>
        {
            Data = data,
            TotalRecords = total,
            Page = page,
            PageSize = pageSize
        };
    }
}

