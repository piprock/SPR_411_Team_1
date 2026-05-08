using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data.Entities;
using SPR_411_Team_1.DAL.Repositories;

namespace SPR_411_Team_1.BLL.Services
{
    public class AlbumService
    {
        private readonly AlbumRepository _albumRepository;
        private readonly ArtistRepository _artistRepository;

        public AlbumService(AlbumRepository albumRepository, ArtistRepository artistRepository)
        {
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _albumRepository.Albums
                .ToListAsync();

            return ServiceResponse.Success("Список альбомів отримано", entities);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _albumRepository.GetAlbumByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Альбом з id '{id}' не знайдений");
            }

            return ServiceResponse.Success("Альбом отримано", entity);
        }

        public async Task<ServiceResponse> CreateAsync(Album entity)
        {
            if (await _albumRepository.IsExistsAsync(entity.Title))
            {
                return ServiceResponse.Error($"Альбом '{entity.Title}' вже існує");
            }

            if (await _artistRepository.GetByIdAsync(entity.ArtistId) == null)
            {
                return ServiceResponse.Error($"Артист з id '{entity.ArtistId}' не знайдений");
            }

            entity.CreatedAt = entity.CreatedAt == default ? DateTime.UtcNow : entity.CreatedAt;

            bool res = await _albumRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати альбом");
            }

            return ServiceResponse.Success($"Альбом '{entity.Title}' успішно доданий", entity);
        }

        public async Task<ServiceResponse> UpdateAsync(Album entity)
        {
            var current = await _albumRepository.GetByIdAsync(entity.Id);

            if (current == null)
            {
                return ServiceResponse.Error($"Альбом з id '{entity.Id}' не знайдений");
            }

            if (await _albumRepository.IsExistsAsync(entity.Title, entity.Id))
            {
                return ServiceResponse.Error($"Альбом '{entity.Title}' вже існує");
            }

            if (await _artistRepository.GetByIdAsync(entity.ArtistId) == null)
            {
                return ServiceResponse.Error($"Артист з id '{entity.ArtistId}' не знайдений");
            }

            string oldTitle = current.Title;

            current.Title = entity.Title;
            current.ArtistId = entity.ArtistId;
            current.CoverUrl = entity.CoverUrl;
            current.CreatedAt = entity.CreatedAt;

            bool res = await _albumRepository.UpdateAsync(current);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося змінити альбом");
            }

            return ServiceResponse.Success($"Альбом '{oldTitle}' успішно змінений", current);
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _albumRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Альбом з id '{id}' не знайдений");
            }

            bool res = await _albumRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося видалити альбом");
            }

            return ServiceResponse.Success($"Альбом '{entity.Title}' успішно видалений");
        }
    }
}
