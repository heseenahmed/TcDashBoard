using TCDashBoard.Dtos;
using TCDashBoard.Models;

namespace TCDashBoard.IRepository
{
    public interface ILogoRepository
    {
        Task<IEnumerable<Logos>> GetAllAsync();
        Task<Logos> GetByIdAsync(int id);
        Task<bool> CreateLogoAsync(LogoDto dto);
        Task<bool> UpdateLogoAsync(int id, LogoDto dto);
        Task<bool> DeleteLogoAsync(int id);
    }
}
