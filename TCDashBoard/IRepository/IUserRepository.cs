using TCDashBoard.Dtos;

namespace TCDashBoard.IRepository
{
    public interface IUserRepository
    {
        Task<bool> Login(LoginDto dto);
    }
}
