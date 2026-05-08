using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data.Entities;
using SPR_411_Team_1.DAL.Repositories;

namespace SPR_411_Team_1.BLL.Services
{
    public class ArtistService
    {
        private readonly ArtistRepository _artistRepository;

        public ArtistService(ArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _artistRepository.Artists
                .ToListAsync();

            return ServiceResponse.Success("Список артистів отримано", entities);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _artistRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Артист з id '{id}' не знайдений");
            }

            return ServiceResponse.Success("Артиста отримано", entity);
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            var entity = await _artistRepository.GetByNameAsync(name);

            if (entity == null)
            {
                return ServiceResponse.Error($"Артист '{name}' не знайдений");
            }

            return ServiceResponse.Success("Артиста отримано", entity);
        }

        public async Task<ServiceResponse> CreateAsync(Artist entity)
        {
            if (await _artistRepository.IsExistsAsync(entity.Name))
            {
                return ServiceResponse.Error($"Артист '{entity.Name}' вже існує");
            }

            bool res = await _artistRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати артиста");
            }

            return ServiceResponse.Success($"Артист '{entity.Name}' успішно доданий", entity);
        }

        public async Task<ServiceResponse> UpdateAsync(Artist entity)
        {
            var current = await _artistRepository.GetByIdAsync(entity.Id);

            if (current == null)
            {
                return ServiceResponse.Error($"Артист з id '{entity.Id}' не знайдений");
            }

            if (await _artistRepository.IsExistsAsync(entity.Name, entity.Id))
            {
                return ServiceResponse.Error($"Артист '{entity.Name}' вже існує");
            }

            string oldName = current.Name;

            current.Name = entity.Name;
            current.Bio = entity.Bio;
            current.ImageUrl = entity.ImageUrl;

            bool res = await _artistRepository.UpdateAsync(current);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося змінити артиста");
            }

            return ServiceResponse.Success($"Артист '{oldName}' успішно змінений", current);
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _artistRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Артист з id '{id}' не знайдений");
            }

            bool res = await _artistRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося видалити артиста");
            }

            return ServiceResponse.Success($"Артист '{entity.Name}' успішно видалений");
        }
    }
}
