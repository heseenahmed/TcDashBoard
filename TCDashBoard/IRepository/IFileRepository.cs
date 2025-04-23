namespace TCDashBoard.IRepository
{
    public interface IFileRepository
    {
        Task<string> SaveImageAsync(IFormFile file, string folder = "uploads");
        Task DeleteImageAsync(string relativePath);
    }
}
