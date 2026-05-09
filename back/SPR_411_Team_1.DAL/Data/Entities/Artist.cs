using System;
using System.Collections.Generic;
using System.Text;

namespace SPR_411_Team_1.DAL.Data.Entities
{
    public class Artist : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}
