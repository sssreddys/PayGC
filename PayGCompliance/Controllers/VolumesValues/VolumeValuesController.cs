

using Compliance_Dtos.Common;
using Compliance_Dtos.VolumesValues;
using Compliance_Services.VolumesValues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PayGCompliance.Controllers.VolumesValues
{
    [Authorize]
    [ApiController]
    [Route("api/volumevalues")]
    public class VolumeValuesController : ControllerBase
    {
        private readonly IVolumesValuesService _service;

        public VolumeValuesController(IVolumesValuesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
             [FromQuery(Name = "search")] string? searchKeyword,
     [FromQuery(Name = "status")] string? statusFilter,
     [FromQuery(Name = "page")] int pageNumber = 1,
     [FromQuery(Name = "pageSize")] int recordsPerPage = 10,
     [FromQuery(Name = "fromDate")] DateTime? fromDate = null,
     [FromQuery(Name = "toDate")] DateTime? toDate = null
            )
        {
            try
            {
                var data = await _service.GetPagedAsync(searchKeyword,
            statusFilter,
            pageNumber,
            recordsPerPage,
            fromDate,
            toDate);

                return Ok(new ApiResponse<PagedResult<VolumeValueDto>>
                {
                    Success = true,
                    Message = "Volume values fetched successfully.",
                    Data = data
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
                var data = await _service.GetByIdAsync(id);
                if (data == null)
                {
                    return NotFound(new ApiResponse<VolumeValueDto>
                    {
                        Success = false,
                        Message = "Volumes and Values not found.",
                    });
                }

                return Ok(new ApiResponse<VolumeValueDto>
                {
                    Success = true,
                    Message = "Volumes and Values retrieved successfully.",
                    Data = data
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
        public async Task<IActionResult> Create([FromForm] CreateVolumeValueDto dto)
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
                var created_by = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // 2. Option A: Pass `createdBy` separately if service accepts it
                var id = await _service.CreateAsync(dto, created_by!);

                return Ok(new
                {
                    success = true,
                    message = "Volumes and Values record created successfully."
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
        public async Task<IActionResult> Update([FromForm] UpdateVolumeValueDto dto)
        {
            try
            {
                var updatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(updatedBy))
                    return Unauthorized(new { message = "Invalid token or user ID missing" });
                var updated = await _service.UpdateAsync(dto, updatedBy);

                if (updated == -1) return NotFound();
                return Ok(new
                {
                    success = true,
                    message = "Volumes and Values record Updated successfully."
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
                        message = "Volumes and Values Deleted successfully"
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
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
    }
