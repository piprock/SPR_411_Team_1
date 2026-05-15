using SPR_411_Team_1.BLL.DTOs.Genre;
using SPR_411_Team_1.BLL.DTOs.Song;

namespace SPR_411_Team_1.BLL.DTOs.SongGenre
{
    public class SongGenreDto
    {
        public int SongId { get; set; }
        public int GenreId { get; set; }
        public SongBriefDto Song { get; set; } = new();
        public GenreDto Genre { get; set; } = new();
    }
}
