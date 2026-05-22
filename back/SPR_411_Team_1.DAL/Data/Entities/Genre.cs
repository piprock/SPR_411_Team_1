using System;
using System.Collections.Generic;
using System.Text;

namespace SPR_411_Team_1.DAL.Data.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<SongGenre> SongGenres { get; set; } = new List<SongGenre>();
    }
}
