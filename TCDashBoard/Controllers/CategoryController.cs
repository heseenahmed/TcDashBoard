//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using TCDashBoard.IRepository;
//using TCDashBoard.Models;

//namespace TCDashBoard.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CategoryController : ControllerBase
//    {
//        private readonly ICategoryRepository _repository;
//        private readonly ILogger<CategoryController> _logger;

//        public CategoryController(ICategoryRepository repository, ILogger<CategoryController> logger)
//        {
//            _repository = repository;
//            _logger = logger;
//        }

//        [HttpGet("GetAllCategory")]
//        public async Task<IActionResult> GetAllCategories()
//        {
//            var categories = await _repository.GetAllAsync();
//            return Ok(categories);
//        }

//        [HttpGet("GetCategoryById/{id}")]
//        public async Task<IActionResult> GetCategoryById(int id)
//        {
//            var category = await _repository.GetByIdAsync(id);
//            if (category == null) return NotFound();
//            return Ok(category);
//        }

//        [HttpPost("CreateCategory")]
//        public async Task<IActionResult> Create([FromForm]Category category)
//        {
//            try
//            {
//                var created = await _repository.AddAsync(category);
//                return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, created);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Create operation failed");
//                return StatusCode(500, "An error occurred while creating the category.");
//            }
//        }

//        [HttpPut("UpdateCategory/{id}")]
//        public async Task<IActionResult> UpdateCategory(int id, [FromForm]Category category)
//        {
//            if (id != category.Id) return BadRequest("ID mismatch");

//            try
//            {
//                var updated = await _repository.UpdateAsync(category);
//                if (updated == null) return NotFound();

//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Update operation failed");
//                return StatusCode(500, "An error occurred while updating the category.");
//            }
//        }

//        [HttpDelete("DeleteCategory/{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                var deleted = await _repository.DeleteAsync(id);
//                if (!deleted) return NotFound();

//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Delete operation failed");
//                return StatusCode(500, "An error occurred while deleting the category.");
//            }
//        }
//    }
//}
