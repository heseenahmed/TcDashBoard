using Microsoft.AspNetCore.Mvc;
using TCDashBoard.Dtos;
using TCDashBoard.IRepository;
using TCDashBoard.Models;

namespace TCDashBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoController : ControllerBase
    {
        public ILogoRepository _logoRepository { get; set; }
        public LogoController(ILogoRepository logoRepository)
        {
            _logoRepository = logoRepository;
        }

        [HttpGet("GetAllLogos")]
        public async Task<IActionResult> GetAllLogos()
        {
            try
            {
                var logos = await _logoRepository.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<Logos>>.Success(logos));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<LogoDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("GetLogoById/{id}")]
        public async Task<IActionResult> GetLogoById(int id)
        {
            try
            {
                var logo = await _logoRepository.GetByIdAsync(id);
                return Ok(ApiResponse<Logos>.Success(logo));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LogoDto>.Fail(ex.Message));
            }
        }

        [HttpPost("InsertLogo")]
        public async Task<IActionResult> InsertLogo([FromForm] LogoDto logo)
        {
            try
            {
                var result = await _logoRepository.CreateLogoAsync(logo);
                return Ok(ApiResponse<string>.Success("logo added successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<LogoDto>.Fail(ex.Message));
            }
        }

        [HttpPut("UpdateLogo/{id}")]
        public async Task<IActionResult> UpdateLogo(int id, [FromForm] LogoDto logo)
        {
            try
            {
                var result = await _logoRepository.UpdateLogoAsync(id, logo);
                if (!result)
                    return NotFound(ApiResponse<string>.Fail("Logo not found."));

                return Ok(ApiResponse<string>.Success("logo Updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpDelete("DeleteLogo/{id}")]
        public async Task<IActionResult> DeleteLogo(int id)
        {
            try
            {
                var result = await _logoRepository.DeleteLogoAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Fail("Logo not found."));

                return Ok(ApiResponse<string>.Success("logo Deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }
    }
}
