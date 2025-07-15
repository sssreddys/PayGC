using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Common;
using Compliance_Services.BoardResolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;

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
            [FromQuery] string? search = null,
            [FromQuery] bool? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var result = await _service.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);
                return Ok(new ApiResponse<PagedResult<BoardResolutionDto>>
                {
                    Success = true,
                    Message = "Board resolutions fetched successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error fetching data: {ex.Message}",
                    Data = null
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
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error retrieving board resolution: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BoardResolutionDto dto)
        {
            try
            {
                dto.CreatedAt = DateTime.UtcNow;
                var id = await _service.CreateAsync(dto, dto.AttachedDocument);

                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = "Board resolution created successfully.",
                    Data = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error creating board resolution: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BoardResolutionDto dto)
        {
            if (id != dto.BoardResolutionId)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "ID in URL and body do not match.",
                    Data = null
                });
            }

            try
            {
                dto.UpdatedAt = DateTime.UtcNow;
                var result = await _service.UpdateAsync(dto, dto.AttachedDocument);

                if (result < 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Board resolution not found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Board resolution updated successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error updating board resolution: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string performedBy)
        {
            try
            {
                var result = await _service.DeleteAsync(id, performedBy);
                if (result < 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Board resolution not found or could not be deleted.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Board resolution deleted successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error deleting board resolution: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}
