using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.DAL.Repositories
{
    public class SongGenreRepository
    {
        private readonly AppDbContext _context;

        public SongGenreRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<SongGenre> SongGenres => _context.SongGenres
            .Include(sg => sg.Song)
            .Include(sg => sg.Genre)
            .AsNoTracking();

        public async Task<bool> CreateAsync(SongGenre entity)
        {
            await _context.SongGenres.AddAsync(entity);
            int res = await _context.SaveChangesAsync();
            return res != 0;
        }

        public async Task<bool> CreateRangeAsync(IEnumerable<SongGenre> entities)
        {
            await _context.SongGenres.AddRangeAsync(entities);
            int res = await _context.SaveChangesAsync();
            return res != 0;
        }

        public async Task<bool> DeleteAsync(SongGenre entity)
        {
            _context.SongGenres.Remove(entity);
            int res = await _context.SaveChangesAsync();
            return res != 0;
        }

        public async Task<SongGenre?> GetByIdsAsync(int songId, int genreId)
        {
            return await _context.SongGenres
                .FirstOrDefaultAsync(sg => sg.SongId == songId && sg.GenreId == genreId);
        }
    }
}
