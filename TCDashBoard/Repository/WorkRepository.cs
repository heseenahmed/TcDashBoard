using Microsoft.EntityFrameworkCore;
using TCDashBoard.Dtos;
using TCDashBoard.IRepository;
using TCDashBoard.Models;

namespace TCDashBoard.Repository
{
    public class WorkRepository : IWorkRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileService;
        public WorkRepository(ApplicationDbContext context , IFileRepository fileRepository)
        {
            _context = context;
            _fileService = fileRepository;
        }
        public async Task<bool> CreateWorkAsync(WorkDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var category = await _context.Categories.FindAsync(dto.CategoryId);
                if (category == null)
                    throw new ApplicationException($"Category with ID {dto.CategoryId} does not exist.");

                var imagePath = await _fileService.SaveImageAsync(dto.WorkImage);

                var work = new Works
                {
                    Title=dto.title,
                    Description = dto.Description,
                    Image = imagePath ,
                    CategoryId = dto.CategoryId
                };

                _context.Works.Add(work);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("An error occurred while creating the work.", ex);
            }
        }

        public async Task<bool> DeleteWorkAsync(int id)
        {
            try
            {
                var work = await _context.Works.FindAsync(id);
                if (work == null) return false;

                if (!string.IsNullOrEmpty(work.Image))
                {
                    await _fileService.DeleteImageAsync(work.Image);
                }

                _context.Works.Remove(work);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the work with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Works>> GetAllWorksAsync()
        {
            try
            {
                return await _context.Works.Include(c => c.Category)
                    .Select(l => new Works
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Description = l.Description , 
                        Image  = l.Image , 
                        Category= l.Category ,
                        CategoryId = l.CategoryId 
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all Works.", ex);
            }
        }

        public async Task<Works> GetWorkByIdAsync(int id)
        {
            try
            {
                var work = await _context.Works.Include(x=>x.Category)
                    .Select(l => new Works
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Description = l.Description,
                        Image = l.Image,
                        Category = l.Category,
                        CategoryId = l.CategoryId
                    })
                    .FirstOrDefaultAsync(w => w.Id == id);
                    return work ?? null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the work.", ex);
            }
        }

        public async Task<bool> UpdateWorkAsync(int id, WorkDto dto)
        {
            try
            {
                var work = await _context.Works.FindAsync(id);
                if (work == null) return false;

                if (!string.IsNullOrEmpty(work.Image))
                {
                    await _fileService.DeleteImageAsync(work.Image);
                }
                var category = await _context.Categories.FindAsync(dto.CategoryId);
                if (category == null)
                    throw new ApplicationException($"Category with ID {dto.CategoryId} does not exist.");

                var newImagePath = await _fileService.SaveImageAsync(dto.WorkImage);

                work.Title =dto.title;
                work.Description = dto.Description;
                work.Image = newImagePath;
                work.CategoryId = dto.CategoryId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating work.", ex);
            }
        }
    }
}
