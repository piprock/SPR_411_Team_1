using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.API.Models
{
    public class SongRequest
    {
        public Song Song { get; set; } = null!;

        public List<int>? GenreIds { get; set; }
    }
}
