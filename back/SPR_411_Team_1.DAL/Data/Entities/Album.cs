using System;
using System.Collections.Generic;
using System.Text;

namespace SPR_411_Team_1.DAL.Data.Entities
{
    public class Album : BaseEntity
    {
        public string Title { get; set; } = null!;
        public int ArtistId { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Artist Artist { get; set; } = null!;
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
