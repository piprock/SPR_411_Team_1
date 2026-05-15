using SPR_411_Team_1.BLL.DTOs.Album;
using SPR_411_Team_1.BLL.DTOs.Artist;
using SPR_411_Team_1.BLL.DTOs.Genre;

namespace SPR_411_Team_1.BLL.DTOs.Song
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public TimeSpan Duration { get; set; }
        public string? AudioUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public ArtistDto Artist { get; set; } = new();
        public AlbumBriefDto Album { get; set; } = new();
        public List<GenreDto> Genres { get; set; } = new();
    }
}
