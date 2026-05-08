using Microsoft.EntityFrameworkCore;
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
            var entities = await _songRepository.Songs
                .ToListAsync();

            return ServiceResponse.Success("Список пісень отримано", entities);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _songRepository.GetSongByIdAsync(id);

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

            var created = await _songRepository.GetSongByIdAsync(entity.Id);
            return ServiceResponse.Success($"Пісня '{entity.Title}' успішно додана", created ?? entity);
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

            var updated = await _songRepository.GetSongByIdAsync(entity.Id);
            return ServiceResponse.Success($"Пісня '{oldTitle}' успішно змінена", updated ?? current);
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
