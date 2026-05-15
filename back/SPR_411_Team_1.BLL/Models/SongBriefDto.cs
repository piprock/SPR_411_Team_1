using System;

namespace SPR_411_Team_1.BLL.Models
{
    public class SongBriefDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
