using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.DAL.Repositories
{
    public class SongRepository : GenericRepository<Song>
    {
        private readonly AppDbContext _context;

        public SongRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Song> Songs => _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Album)
            .Include(s => s.SongGenres)
            .ThenInclude(sg => sg.Genre)
            .AsNoTracking();

        public async Task<Song?> GetSongByIdAsync(int id)
        {
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Include(s => s.SongGenres)
                .ThenInclude(sg => sg.Genre)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Song?> GetByTitleAsync(string title)
        {
            return await _context.Songs
                .FirstOrDefaultAsync(s => s.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> IsExistsAsync(string title)
        {
            return await _context.Songs
                .AsNoTracking()
                .AnyAsync(s => s.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> IsExistsAsync(string title, params int[] exceptionIds)
        {
            return await _context.Songs
                .AsNoTracking()
                .AnyAsync(s => s.Title.ToLower() == title.ToLower()
                    && !exceptionIds.Contains(s.Id));
        }
    }
}
