using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Common;
using Compliance_Services.BoardResolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System.Security.Claims;

namespace PayGCompliance.Controllers.BoardResolution
{
    [Authorize]
    [ApiController]
    [Route("api/board-resolutions")]
    public class BoardResolutionController : ControllerBase
    {
        private readonly IBoardResolutionService _service;

        public BoardResolutionController(IBoardResolutionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
             [FromQuery(Name = "search")] string? searchKeyword,
     [FromQuery(Name = "status")] string? statusFilter,
     [FromQuery(Name = "page")] int pageNumber = 1,
     [FromQuery(Name = "pageSize")] int recordsPerPage = 10,
     [FromQuery(Name = "fromDate")] DateTime? fromDate = null,
     [FromQuery(Name = "toDate")] DateTime? toDate = null)
        {
            try
            {
                var result = await _service.GetPagedAsync(searchKeyword,
            statusFilter,
            pageNumber,
            recordsPerPage,
            fromDate,
            toDate);
                return Ok(new ApiResponse<PagedResult<BoardResolutionDto>>
                {
                    Success = true,
                    Message = "Board resolutions fetched successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound(new ApiResponse<BoardResolutionDto>
                    {
                        Success = false,
                        Message = "Board resolution not found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<BoardResolutionDto>
                {
                    Success = true,
                    Message = "Board resolution found.",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateBoardResolutionDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new { message = errorMessages });
            }
            try
            {
                byte[]? documentBytes = null;
                if (dto.AttachedDocument != null && dto.AttachedDocument.Length > 0)
                {
                    if (dto.AttachedDocument.Length > 500 * 1024) // Max 1MB
                        return BadRequest(new
                        {
                            success = false,
                            message = "Attached Document size should not exceed 500KB."

                        });

                    using var ms = new MemoryStream();
                    await dto.AttachedDocument.CopyToAsync(ms);
                    documentBytes = ms.ToArray();
                }
                var created_by = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var id = await _service.CreateAsync(dto, documentBytes, created_by!);

                return Ok(new 
                {
                    Success = true,
                    Message = "Board resolution created successfully.",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateBoardResolutionDto dto)
        {

            try
            {
                byte[]? documentBytes = null;
                var updatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(updatedBy))
                    return Unauthorized(new { message = "Invalid token or user ID missing" });

                if (Request.Form.Files.Count > 0)
                {
                    var hpf = Request.Form.Files[0]; // safely get the uploaded file

                    using var memory = new MemoryStream();
                    hpf.CopyTo(memory);
                    documentBytes = memory.ToArray();
                }
                var updated = await _service.UpdateAsync(dto, documentBytes, updatedBy);
                if (updated == -1) return NotFound();

                return Ok(new 
                {
                    Success = true,
                    Message = "Board resolution updated successfully.",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequestDto dto)
        {
            try
            {
                var updatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(updatedBy))
                    return Unauthorized(new { message = "Invalid token or user ID missing" });

                var deleted = await _service.DeleteAsync(dto, updatedBy);

                return deleted switch
                {
                    -1 => NotFound(new
                    {
                        success = false,
                        message = "Record not found."
                    }),

                    -2 => BadRequest(new
                    {
                        success = false,
                        message = "This record is already deleted."
                    }),

                    > 0 => Ok(new
                    {
                        success = true,
                        message = "Board Resolution Deleted successfully"
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        message = "Unexpected error occurred during deletion."
                    })
                };


            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging framework
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
