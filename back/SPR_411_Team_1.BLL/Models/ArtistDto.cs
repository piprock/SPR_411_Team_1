namespace SPR_411_Team_1.BLL.Models
{
    public class ArtistDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
    }
}
