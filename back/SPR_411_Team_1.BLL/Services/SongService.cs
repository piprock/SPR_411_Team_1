using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.BLL.Models;
using SPR_411_Team_1.DAL.Data.Entities;
using SPR_411_Team_1.DAL.Repositories;

namespace SPR_411_Team_1.BLL.Services
{
    public class SongService
    {
        private readonly SongRepository _songRepository;
        private readonly ArtistRepository _artistRepository;
        private readonly AlbumRepository _albumRepository;
        private readonly GenreRepository _genreRepository;
        private readonly SongGenreRepository _songGenreRepository;

        public SongService(
            SongRepository songRepository,
            ArtistRepository artistRepository,
            AlbumRepository albumRepository,
            GenreRepository genreRepository,
            SongGenreRepository songGenreRepository)
        {
            _songRepository = songRepository;
            _artistRepository = artistRepository;
            _albumRepository = albumRepository;
            _genreRepository = genreRepository;
            _songGenreRepository = songGenreRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await GetSongDtos()
                .ToListAsync();

            return ServiceResponse.Success("Список пісень отримано", entities);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await GetSongDtos()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Пісня з id '{id}' не знайдена");
            }

            return ServiceResponse.Success("Пісню отримано", entity);
        }

        public async Task<ServiceResponse> CreateAsync(Song entity, IEnumerable<int>? genreIds = null)
        {
            if (await _songRepository.IsExistsAsync(entity.Title))
            {
                return ServiceResponse.Error($"Пісня '{entity.Title}' вже існує");
            }

            var validation = await ValidateRelationsAsync(entity.ArtistId, entity.AlbumId, genreIds);
            if (!validation.IsSuccess)
            {
                return validation;
            }

            entity.CreatedAt = entity.CreatedAt == default ? DateTime.UtcNow : entity.CreatedAt;

            bool res = await _songRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати пісню");
            }

            if (genreIds != null)
            {
                var songGenres = genreIds
                    .Distinct()
                    .Select(genreId => new SongGenre
                    {
                        SongId = entity.Id,
                        GenreId = genreId
                    });

                await _songGenreRepository.CreateRangeAsync(songGenres);
            }

            var created = await GetSongDtos()
                .FirstOrDefaultAsync(s => s.Id == entity.Id);

            return ServiceResponse.Success(
                $"Пісня '{entity.Title}' успішно додана",
                created ?? new SongDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    ArtistId = entity.ArtistId,
                    AlbumId = entity.AlbumId,
                    Duration = entity.Duration,
                    AudioUrl = entity.AudioUrl,
                    CreatedAt = entity.CreatedAt
                });
        }

        public async Task<ServiceResponse> UpdateAsync(Song entity, IEnumerable<int>? genreIds = null)
        {
            var current = await _songRepository.GetByIdAsync(entity.Id);

            if (current == null)
            {
                return ServiceResponse.Error($"Пісня з id '{entity.Id}' не знайдена");
            }

            if (await _songRepository.IsExistsAsync(entity.Title, entity.Id))
            {
                return ServiceResponse.Error($"Пісня '{entity.Title}' вже існує");
            }

            var validation = await ValidateRelationsAsync(entity.ArtistId, entity.AlbumId, genreIds);
            if (!validation.IsSuccess)
            {
                return validation;
            }

            string oldTitle = current.Title;

            current.Title = entity.Title;
            current.ArtistId = entity.ArtistId;
            current.AlbumId = entity.AlbumId;
            current.Duration = entity.Duration;
            current.AudioUrl = entity.AudioUrl;
            current.CreatedAt = entity.CreatedAt;

            bool res = await _songRepository.UpdateAsync(current);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося змінити пісню");
            }

            if (genreIds != null)
            {
                await UpdateGenresAsync(entity.Id, genreIds);
            }

            var updated = await GetSongDtos()
                .FirstOrDefaultAsync(s => s.Id == entity.Id);

            return ServiceResponse.Success(
                $"Пісня '{oldTitle}' успішно змінена",
                updated ?? new SongDto
                {
                    Id = current.Id,
                    Title = current.Title,
                    ArtistId = current.ArtistId,
                    AlbumId = current.AlbumId,
                    Duration = current.Duration,
                    AudioUrl = current.AudioUrl,
                    CreatedAt = current.CreatedAt
                });
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _songRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Пісня з id '{id}' не знайдена");
            }

            bool res = await _songRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося видалити пісню");
            }

            return ServiceResponse.Success($"Пісня '{entity.Title}' успішно видалена");
        }

        private IQueryable<SongDto> GetSongDtos()
        {
            return _songRepository.Songs.Select(s => new SongDto
            {
                Id = s.Id,
                Title = s.Title,
                ArtistId = s.ArtistId,
                AlbumId = s.AlbumId,
                Duration = s.Duration,
                AudioUrl = s.AudioUrl,
                CreatedAt = s.CreatedAt,
                Artist = new ArtistDto
                {
                    Id = s.Artist.Id,
                    Name = s.Artist.Name,
                    Bio = s.Artist.Bio,
                    ImageUrl = s.Artist.ImageUrl
                },
                Album = new AlbumBriefDto
                {
                    Id = s.Album.Id,
                    Title = s.Album.Title,
                    CoverUrl = s.Album.CoverUrl
                },
                Genres = s.SongGenres.Select(sg => new GenreDto
                {
                    Id = sg.Genre.Id,
                    Name = sg.Genre.Name
                }).ToList()
            });
        }

        private async Task<ServiceResponse> ValidateRelationsAsync(
            int artistId,
            int albumId,
            IEnumerable<int>? genreIds)
        {
            if (await _artistRepository.GetByIdAsync(artistId) == null)
            {
                return ServiceResponse.Error($"Артист з id '{artistId}' не знайдений");
            }

            if (await _albumRepository.GetByIdAsync(albumId) == null)
            {
                return ServiceResponse.Error($"Альбом з id '{albumId}' не знайдений");
            }

            if (genreIds != null)
            {
                foreach (int genreId in genreIds.Distinct())
                {
                    if (await _genreRepository.GetByIdAsync(genreId) == null)
                    {
                        return ServiceResponse.Error($"Жанр з id '{genreId}' не знайдений");
                    }
                }
            }

            return ServiceResponse.Success("Зв'язки перевірено");
        }

        private async Task UpdateGenresAsync(int songId, IEnumerable<int> genreIds)
        {
            var currentGenres = await _songGenreRepository.SongGenres
                .Where(sg => sg.SongId == songId)
                .ToListAsync();

            foreach (var currentGenre in currentGenres)
            {
                await _songGenreRepository.DeleteAsync(currentGenre);
            }

            var newGenres = genreIds
                .Distinct()
                .Select(genreId => new SongGenre
                {
                    SongId = songId,
                    GenreId = genreId
                });

            await _songGenreRepository.CreateRangeAsync(newGenres);
        }
    }
}
