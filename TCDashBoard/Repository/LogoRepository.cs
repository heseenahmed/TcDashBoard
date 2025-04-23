using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TCDashBoard.Dtos;
using TCDashBoard.IRepository;
using TCDashBoard.Models;

namespace TCDashBoard.Repository
{
    public class LogoRepository : ILogoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileRepository _fileService;
        public LogoRepository(ApplicationDbContext context , IFileRepository fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IEnumerable<Logos>> GetAllAsync()
        {
            try
            {
                return await _context.Logos
                    .Select(l => new Logos
                    {
                        Id = l.Id,
                        Order = l.Order,
                        LogoImage = l.LogoImage
                    })
                    .OrderBy(x=>x.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all logos.", ex);
            }
        }
        public async Task<Logos?> GetByIdAsync(int id)
        {
            try
            {
                var logo = await _context.Logos.FindAsync(id);
                if (logo == null) return null;

                return new Logos
                {
                    Id = logo.Id,
                    Order = logo.Order,
                    LogoImage = logo.LogoImage
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the logo with ID {id}.", ex);
            }
        }
        public async Task<bool> CreateLogoAsync(LogoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var imagePath = await _fileService.SaveImageAsync(dto.LogoImage);
                int maxOrder = await _context.Logos.MaxAsync(l => (int?) l.Order ) ?? 0;

                var logo = new Logos
                {
                    Order = ++maxOrder  ,
                    LogoImage = imagePath
                };

                _context.Logos.Add(logo);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("An error occurred while creating the logo.", ex);
            }
        }
        public async Task<bool> UpdateLogoAsync(int id, LogoDto dto)
        {
            try
            {
                var logo = await _context.Logos.FindAsync(id);
                if (logo == null) return false;

                if (!string.IsNullOrEmpty(logo.LogoImage))
                {
                    await _fileService.DeleteImageAsync(logo.LogoImage); 
                }

                var newImagePath = await _fileService.SaveImageAsync(dto.LogoImage);

                logo.LogoImage = newImagePath;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the logo.", ex);
            }
        }
        public async Task<bool> DeleteLogoAsync(int id)
        {
            try
            {
                var logo = await _context.Logos.FindAsync(id);
                if (logo == null) return false;

                if (!string.IsNullOrEmpty(logo.LogoImage))
                {
                    await _fileService.DeleteImageAsync(logo.LogoImage); 
                }

                _context.Logos.Remove(logo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the logo with ID {id}.", ex);
            }
        }

    }
}
