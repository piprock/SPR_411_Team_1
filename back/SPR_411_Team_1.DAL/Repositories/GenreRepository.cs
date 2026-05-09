using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.DAL.Repositories
{
    public class GenreRepository : GenericRepository<Genre>
    {
        private readonly AppDbContext _context;

        public GenreRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Genre> Genres => GetAll();

        public async Task<Genre?> GetByNameAsync(string name)
        {
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == name.ToLower());
        }

        public IQueryable<Genre> GetByName(string name)
        {
            return _context.Genres
                .Where(g => g.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsExistsAsync(string name)
        {
            return await GetByNameAsync(name) != null;
        }
    }
}
