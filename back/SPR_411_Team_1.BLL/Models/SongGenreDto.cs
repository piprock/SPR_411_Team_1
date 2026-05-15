namespace SPR_411_Team_1.BLL.Models
{
    public class SongGenreDto
    {
        public int SongId { get; set; }
        public int GenreId { get; set; }
        public SongBriefDto Song { get; set; } = new();
        public GenreDto Genre { get; set; } = new();
    }
}
