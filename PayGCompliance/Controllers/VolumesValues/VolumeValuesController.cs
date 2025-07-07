

using Compliance_Dtos.VolumesValues;
using Compliance_Services.VolumesValues;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayGCompliance.Controllers.VolumesValues
{
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
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            try
            {
                var data = await _service.GetAllAsync(pageNumber, pageSize, searchTerm);

                return Ok(new ApiResponse<IEnumerable<VolumeValueDto>>
                {
                    Success = true,
                    Message = "Volume values fetched successfully.",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error fetching volume values: {ex.Message}",
                    Data = null
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
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"No volume value found with ID = {id}",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<VolumeValueDto>
                {
                    Success = true,
                    Message = "Volume value retrieved successfully.",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error retrieving volume value: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVolumeValueDto dto)
        {
            try
            {
                var id = await _service.CreateAsync(dto);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Volume value created successfully.",
                    Data = new { Id = id }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error creating volume value: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVolumeValueDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"No volume value found with ID = {id} to update.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Volume value updated successfully.",
                    Data = updated
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error updating volume value: {ex.Message}",
                    Data = null
                });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"No volume value found with ID = {id} to delete.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Volume value deleted successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error deleting volume value: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}
