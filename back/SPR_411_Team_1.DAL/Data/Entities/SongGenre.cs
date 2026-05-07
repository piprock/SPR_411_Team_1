using System;
using System.Collections.Generic;
using System.Text;

namespace SPR_411_Team_1.DAL.Data.Entities
{
    public class SongGenre
    {
        public int SongId { get; set; }
        public int GenreId { get; set; }

        // Navigation
        public Song Song { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}
