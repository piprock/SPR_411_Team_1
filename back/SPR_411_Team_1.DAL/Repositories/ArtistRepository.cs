using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.DAL.Repositories
{
    public class ArtistRepository : GenericRepository<Artist>
    {
        private readonly AppDbContext _context;

        public ArtistRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Artist> Artists => GetAll();

        public async Task<Artist?> GetByNameAsync(string name)
        {
            return await _context.Artists
                .FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsExistsAsync(string name)
        {
            return await Artists
                .AnyAsync(a => a.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsExistsAsync(string name, params int[] exceptionIds)
        {
            return await Artists
                .AnyAsync(a => a.Name.ToLower() == name.ToLower()
                    && !exceptionIds.Contains(a.Id));
        }
        public IQueryable<Artist> SearchByName(string query)
        {
            return _context.Artists.Where(a => EF.Functions.ILike(a.Name, $"%{query}%"));
        }
    }
}
