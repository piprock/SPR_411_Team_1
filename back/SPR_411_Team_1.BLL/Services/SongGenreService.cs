using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.BLL.Models;
using SPR_411_Team_1.DAL.Data.Entities;
using SPR_411_Team_1.DAL.Repositories;

namespace SPR_411_Team_1.BLL.Services
{
    public class SongGenreService
    {
        private readonly SongGenreRepository _songGenreRepository;
        private readonly SongRepository _songRepository;
        private readonly GenreRepository _genreRepository;

        public SongGenreService(
            SongGenreRepository songGenreRepository,
            SongRepository songRepository,
            GenreRepository genreRepository)
        {
            _songGenreRepository = songGenreRepository;
            _songRepository = songRepository;
            _genreRepository = genreRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await GetSongGenreDtos()
                .ToListAsync();

            return ServiceResponse.Success("Список зв'язків пісень і жанрів отримано", entities);
        }

        public async Task<ServiceResponse> CreateAsync(SongGenre entity)
        {
            if (await _songRepository.GetByIdAsync(entity.SongId) == null)
            {
                return ServiceResponse.Error($"Пісня з id '{entity.SongId}' не знайдена");
            }

            if (await _genreRepository.GetByIdAsync(entity.GenreId) == null)
            {
                return ServiceResponse.Error($"Жанр з id '{entity.GenreId}' не знайдений");
            }

            if (await _songGenreRepository.GetByIdsAsync(entity.SongId, entity.GenreId) != null)
            {
                return ServiceResponse.Error("Такий зв'язок пісні і жанру вже існує");
            }

            bool res = await _songGenreRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати зв'язок пісні і жанру");
            }

            var created = await GetSongGenreDtos()
                .FirstOrDefaultAsync(sg => sg.SongId == entity.SongId && sg.GenreId == entity.GenreId);

            return ServiceResponse.Success(
                "Зв'язок пісні і жанру успішно доданий",
                created ?? new SongGenreDto
                {
                    SongId = entity.SongId,
                    GenreId = entity.GenreId
                });
        }

        public async Task<ServiceResponse> DeleteAsync(int songId, int genreId)
        {
            var entity = await _songGenreRepository.GetByIdsAsync(songId, genreId);

            if (entity == null)
            {
                return ServiceResponse.Error("Зв'язок пісні і жанру не знайдений");
            }

            bool res = await _songGenreRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося видалити зв'язок пісні і жанру");
            }

            return ServiceResponse.Success("Зв'язок пісні і жанру успішно видалений");
        }

        private IQueryable<SongGenreDto> GetSongGenreDtos()
        {
            return _songGenreRepository.SongGenres.Select(sg => new SongGenreDto
            {
                SongId = sg.SongId,
                GenreId = sg.GenreId,
                Song = new SongBriefDto
                {
                    Id = sg.Song.Id,
                    Title = sg.Song.Title,
                    ArtistId = sg.Song.ArtistId,
                    AlbumId = sg.Song.AlbumId,
                    Duration = sg.Song.Duration
                },
                Genre = new GenreDto
                {
                    Id = sg.Genre.Id,
                    Name = sg.Genre.Name
                }
            });
        }
    }
}
