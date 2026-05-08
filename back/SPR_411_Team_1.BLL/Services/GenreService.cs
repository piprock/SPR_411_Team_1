using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data.Entities;
using SPR_411_Team_1.DAL.Repositories;

namespace SPR_411_Team_1.BLL.Services
{
    public class GenreService
    {
        private readonly GenreRepository _genreRepository;

        public GenreService(GenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _genreRepository.Genres
                .ToListAsync();

            return ServiceResponse.Success("Список жанрів отримано", entities);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{id}' не знайдений");
            }

            return ServiceResponse.Success("Жанр отримано", entity);
        }

        public async Task<ServiceResponse> CreateAsync(Genre entity)
        {
            if (await _genreRepository.IsExistsAsync(entity.Name))
            {
                return ServiceResponse.Error($"Жанр '{entity.Name}' вже існує");
            }

            bool res = await _genreRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати жанр");
            }

            return ServiceResponse.Success($"Жанр '{entity.Name}' успішно доданий", entity);
        }

        public async Task<ServiceResponse> UpdateAsync(Genre entity)
        {
            var current = await _genreRepository.GetByIdAsync(entity.Id);

            if (current == null)
            {
                return ServiceResponse.Error($"Жанр з id '{entity.Id}' не знайдений");
            }

            var existing = await _genreRepository.GetByNameAsync(entity.Name);
            if (existing != null && existing.Id != entity.Id)
            {
                return ServiceResponse.Error($"Жанр '{entity.Name}' вже існує");
            }

            string oldName = current.Name;
            current.Name = entity.Name;

            bool res = await _genreRepository.UpdateAsync(current);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося змінити жанр");
            }

            return ServiceResponse.Success($"Жанр '{oldName}' успішно змінений", current);
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{id}' не знайдений");
            }

            bool res = await _genreRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося видалити жанр");
            }

            return ServiceResponse.Success($"Жанр '{entity.Name}' успішно видалений");
        }
    }
}
