using Microsoft.EntityFrameworkCore;
using TCDashBoard.Dtos;
using TCDashBoard.IRepository;

namespace TCDashBoard.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Login(LoginDto dto)
        {
            try 
            {
                var result = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber && x.Password == dto.Password);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while login this user.", ex);
            }
        }
    }
}
