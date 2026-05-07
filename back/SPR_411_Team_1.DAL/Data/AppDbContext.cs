using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<SongGenre> SongGenres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Primary Keys
        modelBuilder.Entity<Artist>().HasKey(x => x.Id);
        modelBuilder.Entity<Song>().HasKey(x => x.Id);
        modelBuilder.Entity<Album>().HasKey(x => x.Id);
        modelBuilder.Entity<Genre>().HasKey(x => x.Id);

        modelBuilder.Entity<SongGenre>()
            .HasKey(x => new { x.SongId, x.GenreId });

        //Artist -> Songs (1:M)
        modelBuilder.Entity<Song>()
       .HasOne(s => s.Artist)
       .WithMany(a => a.Songs)
       .HasForeignKey(s => s.ArtistId)
       .OnDelete(DeleteBehavior.Restrict);

        //Album -> Songs
        modelBuilder.Entity<Song>()
            .HasOne(s => s.Album)
            .WithMany(a => a.Songs)
            .HasForeignKey(s => s.AlbumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Album>()
            .HasOne(a => a.Artist)
            .WithMany(ar => ar.Albums)
            .HasForeignKey(a => a.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        //SongGenre (M:M)
        modelBuilder.Entity<SongGenre>()
            .HasOne(sg => sg.Song)
            .WithMany(s => s.SongGenres)
            .HasForeignKey(sg => sg.SongId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SongGenre>()
            .HasOne(sg => sg.Genre)
            .WithMany(g => g.SongGenres)
            .HasForeignKey(sg => sg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        //Required + Length

        modelBuilder.Entity<Artist>()
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Genre>()
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Song>()
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Album>()
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Song>()
            .Property(x => x.ArtistId)
            .IsRequired();

        modelBuilder.Entity<Song>()
            .Property(x => x.AlbumId)
            .IsRequired();
    }
}
