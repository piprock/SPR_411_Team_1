using System;
using System.Collections.Generic;
using System.Text;

namespace SPR_411_Team_1.DAL.Data.Entities
{
    public class Song : BaseEntity
    {
        public string Title { get; set; } = null!;
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public TimeSpan Duration { get; set; }
        public string? AudioUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Artist Artist { get; set; } = null!;
        public Album Album { get; set; } = null!;
        public ICollection<SongGenre> SongGenres { get; set; } = new List<SongGenre>();
    }
}
