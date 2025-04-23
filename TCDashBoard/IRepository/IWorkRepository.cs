using TCDashBoard.Dtos;
using TCDashBoard.Models;

namespace TCDashBoard.IRepository
{
    public interface IWorkRepository
    {
        Task<IEnumerable<Works>> GetAllWorksAsync();
        Task<Works> GetWorkByIdAsync(int id);
        Task<bool> CreateWorkAsync(WorkDto dto);
        Task<bool> UpdateWorkAsync(int id, WorkDto dto);
        Task<bool> DeleteWorkAsync(int id);
    }
}
