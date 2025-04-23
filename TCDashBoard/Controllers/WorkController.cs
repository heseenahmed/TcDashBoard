using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCDashBoard.Dtos;
using TCDashBoard.IRepository;
using TCDashBoard.Models;

namespace TCDashBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly IWorkRepository _workRepository;
        public WorkController(IWorkRepository workRepository)
        {
            _workRepository = workRepository;
        }
        [HttpGet("GetAllWorks")]
        public async Task<IActionResult> GetAllWorks() 
        {
            try
            {
                var works = await _workRepository.GetAllWorksAsync();
                return Ok(ApiResponse<IEnumerable<Works>>.Success(works));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<WorkDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("GetWorkById/{id}")]
        public async Task<IActionResult> GetWorkById(int id)
        {
            try {
                var work = await _workRepository.GetWorkByIdAsync(id);
                if (work == null)
                {
                    return NotFound(ApiResponse<WorkDto>.Fail("Work not found"));
                }
                return Ok(ApiResponse<Works>.Success(work));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<WorkDto>.Fail(ex.Message));
            }
        }

        [HttpPost("AddNewWork")]
        public async Task<IActionResult> AddNewWork([FromForm] WorkDto work)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Fail("Validation failed"));
                var result = await _workRepository.CreateWorkAsync(work);
                return Ok(ApiResponse<string>.Success("work added successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<WorkDto>.Fail(ex.Message));
            }
        }

        [HttpPut("UpdateWork/{id}")]
        public async Task<IActionResult> UpdateWork(int id , [FromForm] WorkDto work)
        {
            try 
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Fail("Validation failed"));
                var result = await _workRepository.UpdateWorkAsync(id, work);
                if (result)
                {
                    return Ok(ApiResponse<string>.Success("Work updated successfully"));
                }
                return NotFound(ApiResponse<WorkDto>.Fail("Work not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<WorkDto>.Fail(ex.Message));
            }
        }

        [HttpDelete("DeleteWork/{id}")]
        public async Task<IActionResult> DeleteWork(int id)
        {
            try
            {
                var result = await _workRepository.DeleteWorkAsync(id);
                if (result)
                {
                    return Ok(ApiResponse<string>.Success("Work deleted successfully"));
                }
                return NotFound(ApiResponse<WorkDto>.Fail("Work not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<WorkDto>.Fail(ex.Message));
            }
        }
    }
}
