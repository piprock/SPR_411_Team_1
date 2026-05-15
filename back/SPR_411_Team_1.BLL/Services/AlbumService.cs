using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.BLL.Models;
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
            var entities = await GetAlbumDtos()
                .ToListAsync();

            return ServiceResponse.Success("Список альбомів отримано", entities);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await GetAlbumDtos()
                .FirstOrDefaultAsync(a => a.Id == id);

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

            var created = await GetAlbumDtos()
                .FirstOrDefaultAsync(a => a.Id == entity.Id);

            return ServiceResponse.Success(
                $"Альбом '{entity.Title}' успішно доданий",
                created ?? new AlbumDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    ArtistId = entity.ArtistId,
                    CoverUrl = entity.CoverUrl,
                    CreatedAt = entity.CreatedAt
                });
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

            var updated = await GetAlbumDtos()
                .FirstOrDefaultAsync(a => a.Id == entity.Id);

            return ServiceResponse.Success(
                $"Альбом '{oldTitle}' успішно змінений",
                updated ?? new AlbumDto
                {
                    Id = current.Id,
                    Title = current.Title,
                    ArtistId = current.ArtistId,
                    CoverUrl = current.CoverUrl,
                    CreatedAt = current.CreatedAt
                });
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

        private IQueryable<AlbumDto> GetAlbumDtos()
        {
            return _albumRepository.Albums.Select(a => new AlbumDto
            {
                Id = a.Id,
                Title = a.Title,
                ArtistId = a.ArtistId,
                CoverUrl = a.CoverUrl,
                CreatedAt = a.CreatedAt,
                Artist = new ArtistDto
                {
                    Id = a.Artist.Id,
                    Name = a.Artist.Name,
                    Bio = a.Artist.Bio,
                    ImageUrl = a.Artist.ImageUrl
                },
                Songs = a.Songs.Select(s => new SongBriefDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    ArtistId = s.ArtistId,
                    AlbumId = s.AlbumId,
                    Duration = s.Duration
                }).ToList()
            });
        }
    }
}
