using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data;
using SPR_411_Team_1.DAL.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace SPR_411_Team_1.DAL.Initialization;

public static class Seeder
{
    public static async Task SeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        // якщо вже є дані — не додаємо ще раз
        if (await context.Artists.AnyAsync())
            return;

        // =======================
        // The Beatles
        // =======================
        var beatles = new Artist
        {
            Name = "The Beatles",
            Bio = "Legendary British band",
            ImageUrl = ""
        };

        var abbeyRoad = new Album
        {
            Title = "Abbey Road",
            Artist = beatles,
            CreatedAt = DateTime.UtcNow,
            CoverUrl = ""
        };

        var beatlesSongs = new List<Song>
{
    new Song { Title = "Come Together", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(4), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Something", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Maxwell's Silver Hammer", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Oh! Darling", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Octopus's Garden", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "I Want You (She's So Heavy)", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(7), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Here Comes the Sun", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Because", Artist = beatles, Album = abbeyRoad, Duration = TimeSpan.FromMinutes(2), CreatedAt = DateTime.UtcNow }
};

        // =======================
        // Nirvana
        // =======================
        var nirvana = new Artist
        {
            Name = "Nirvana",
            Bio = "Grunge band",
            ImageUrl = ""
        };

        var nevermind = new Album
        {
            Title = "Nevermind",
            Artist = nirvana,
            CreatedAt = DateTime.UtcNow,
            CoverUrl = ""
        };

        var nirvanaSongs = new List<Song>
{
    new Song { Title = "Smells Like Teen Spirit", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(5), CreatedAt = DateTime.UtcNow },
    new Song { Title = "In Bloom", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(4), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Come As You Are", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Breed", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Lithium", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(4), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Polly", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(2), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Territorial Pissings", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(2), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Drain You", Artist = nirvana, Album = nevermind, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow }
};

        // =======================
        // Stevie Wonder
        // =======================
        var stevie = new Artist
        {
            Name = "Stevie Wonder",
            Bio = "Soul legend",
            ImageUrl = ""
        };

        var songsInKey = new Album
        {
            Title = "Songs in the Key of Life",
            Artist = stevie,
            CreatedAt = DateTime.UtcNow,
            CoverUrl = ""
        };

        var stevieSongs = new List<Song>
{
    new Song { Title = "Love's in Need of Love Today", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(7), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Have a Talk with God", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(2), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Village Ghetto Land", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Sir Duke", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(4), CreatedAt = DateTime.UtcNow },
    new Song { Title = "I Wish", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(4), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Knocks Me Off My Feet", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Pastime Paradise", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(3), CreatedAt = DateTime.UtcNow },
    new Song { Title = "Isn't She Lovely", Artist = stevie, Album = songsInKey, Duration = TimeSpan.FromMinutes(6), CreatedAt = DateTime.UtcNow }
};
        // =======================
        // Genres
        // =======================

        var genres = new List<Genre>
{
    new Genre { Name = "Rock" },
    new Genre { Name = "Grunge" },
    new Genre { Name = "Pop" },
    new Genre { Name = "Soul" }
};

        await context.Genres.AddRangeAsync(genres);
        await context.SaveChangesAsync();

        // =======================
        // SongGenres
        // =======================

        var songGenres = new List<SongGenre>
{
    new SongGenre { Song = beatlesSongs[0], Genre = genres[0] },
    new SongGenre { Song = beatlesSongs[1], Genre = genres[0] },

    new SongGenre { Song = nirvanaSongs[0], Genre = genres[1] },
    new SongGenre { Song = nirvanaSongs[1], Genre = genres[1] },

    new SongGenre { Song = stevieSongs[0], Genre = genres[3] },
    new SongGenre { Song = stevieSongs[1], Genre = genres[3] }
};

        await context.SongGenres.AddRangeAsync(songGenres);

        // =======================
        // Save
        // =======================

        await context.Artists.AddRangeAsync(beatles, nirvana, stevie);
        await context.Albums.AddRangeAsync(abbeyRoad, nevermind, songsInKey);
        await context.Songs.AddRangeAsync(
            beatlesSongs.Concat(nirvanaSongs).Concat(stevieSongs)
        );

        await context.SaveChangesAsync();
    }
}

    
    