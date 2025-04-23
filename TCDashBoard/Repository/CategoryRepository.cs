using Microsoft.EntityFrameworkCore;
using TCDashBoard.IRepository;
using TCDashBoard.Models;

namespace TCDashBoard.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                return await _context.Categories.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return Enumerable.Empty<Category>();
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category with ID {Id}", id);
                return null;
            }
        }

        public async Task<Category> AddAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                throw;
            }
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            try
            {
                if (!_context.Categories.Any(c => c.Id == category.Id))
                    return null;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID {Id}", category.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) return false;

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID {Id}", id);
                throw;
            }
        }
    }
}
