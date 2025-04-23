using TCDashBoard.IRepository;

namespace TCDashBoard.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileRepository(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> SaveImageAsync(IFormFile file, string folder = "uploads")
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" , ".svg"};
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only .jpg, .jpeg, .svg and .png files are allowed.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + extension;
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";
            var relativePath = Path.Combine(folder, uniqueFileName).Replace("\\", "/");

            return $"{baseUrl}/{relativePath}";
        }
        public async Task DeleteImageAsync(string relativePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }
        }

    }
}
