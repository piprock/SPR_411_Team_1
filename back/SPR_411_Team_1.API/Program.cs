using Microsoft.EntityFrameworkCore;
using SPR_411_Team_1.BLL.Services;
using SPR_411_Team_1.DAL.Data;
using SPR_411_Team_1.DAL.Initialization;
using SPR_411_Team_1.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

// Add repositories
builder.Services.AddScoped<ArtistRepository>();
builder.Services.AddScoped<AlbumRepository>();
builder.Services.AddScoped<SongRepository>();
builder.Services.AddScoped<GenreRepository>();
builder.Services.AddScoped<SongGenreRepository>();

// Add services
builder.Services.AddScoped<ArtistService>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<SongService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<SongGenreService>();
builder.Services.AddScoped<FileService>();

// Add Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Use Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.SeedAsync();

app.Run();
