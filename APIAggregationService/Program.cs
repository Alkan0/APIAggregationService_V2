using APIAggregationService.Services.Cat;
using APIAggregationService.Services.OpenWeatherMap;
using System.ComponentModel;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register HttpClient and NewsApiService
        builder.Services.AddHttpClient<IOpenWeatherMapService, OpenWeatherMapService>();
        builder.Services.AddSingleton<ISpotifyService, SpotifyService>();
        builder.Services.AddHttpClient<BreedService>();
        builder.Services.AddHttpClient<FavoriteService>();
        builder.Services.AddHttpClient<VoteService>();
        builder.Services.AddHttpClient<CatService>();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
