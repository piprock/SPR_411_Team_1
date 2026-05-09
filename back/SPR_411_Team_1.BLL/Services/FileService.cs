using Microsoft.AspNetCore.Http;
using SPR_411_Team_1.BLL.Settings;

namespace SPR_411_Team_1.BLL.Services
{
    public class FileService
    {
        private readonly string _storagePath;

        public FileService()
        {
            string root = Directory.GetCurrentDirectory();
            _storagePath = Path.Combine(root, StaticFilesSettings.Storage);
        }

        public async Task<ServiceResponse> SaveImageAsync(IFormFile image, string destPath)
        {
            var types = image.ContentType.Split("/");

            if (types.Length != 2 || types[0] != "image")
            {
                return ServiceResponse.Error("Файл не є зображенням");
            }

            string folderPath = Path.Combine(_storagePath, destPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string ext = Path.GetExtension(image.FileName);
            string imageName = Guid.NewGuid() + ext;
            string imagePath = Path.Combine(folderPath, imageName);

            await using var stream = File.Create(imagePath);
            await image.CopyToAsync(stream);

            return ServiceResponse.Success("Зображення успішно збережене", imageName);
        }

        public async Task<ServiceResponse[]> SaveImagesAsync(IEnumerable<IFormFile> images, string destPath)
        {
            var tasks = new List<Task<ServiceResponse>>();

            foreach (var image in images)
            {
                tasks.Add(SaveImageAsync(image, destPath));
            }

            return await Task.WhenAll(tasks);
        }

        public ServiceResponse DeleteImage(string relativePath)
        {
            string imagePath = Path.Combine(_storagePath, relativePath);

            if (!File.Exists(imagePath))
            {
                return ServiceResponse.Error("Зображення не знайдено");
            }

            File.Delete(imagePath);
            return ServiceResponse.Success("Зображення успішно видалене");
        }

        public ServiceResponse DeleteFolder(string relativeFolderPath)
        {
            string fullPath = Path.Combine(_storagePath, relativeFolderPath);

            if (!Directory.Exists(fullPath))
            {
                return ServiceResponse.Error("Папка не знайдена");
            }

            Directory.Delete(fullPath, recursive: true);
            return ServiceResponse.Success("Папку видалено");
        }
    }
}
