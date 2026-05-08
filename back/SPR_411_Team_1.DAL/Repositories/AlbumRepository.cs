using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.DAL.Repositories
{
    public class AlbumRepository : GenericRepository<Album>
    {
        private readonly AppDbContext _context;

        public AlbumRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Album> Albums => _context.Albums
            .Include(a => a.Artist)
            .AsNoTracking();

        public async Task<Album?> GetAlbumByIdAsync(int id)
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Album?> GetByTitleAsync(string title)
        {
            return await _context.Albums
                .FirstOrDefaultAsync(a => a.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> IsExistsAsync(string title)
        {
            return await _context.Albums
                .AsNoTracking()
                .AnyAsync(a => a.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> IsExistsAsync(string title, params int[] exceptionIds)
        {
            return await _context.Albums
                .AsNoTracking()
                .AnyAsync(a => a.Title.ToLower() == title.ToLower()
                    && !exceptionIds.Contains(a.Id));
        }
    }
}
