using SPR_411_Team_1.BLL.DTOs.Artist;
using SPR_411_Team_1.BLL.DTOs.Song;

namespace SPR_411_Team_1.BLL.DTOs.Album
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public ArtistDto Artist { get; set; } = new ArtistDto();
        public List<SongBriefDto> Songs { get; set; } = new List<SongBriefDto>();
    }
}
