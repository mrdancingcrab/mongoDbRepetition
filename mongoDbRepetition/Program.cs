using mongoDbRepetition.Data;
using mongoDbRepetition.Models;

namespace mongoDbRepetition
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            MongoCrud db =  new MongoCrud("FootballPlayers");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //Create player
            app.MapPost("/player", async (Players player) =>
            {
                var playerDB = await db.AddPlayer("Players", player);
                return Results.Ok(playerDB);
            });

            //Get all players
            app.MapGet("/players", async () =>
            {
                var players = await db.GetAllPlayers("Players");
                return Results.Ok(players);
            });

            //Get player by id
            app.MapGet("/player/{id}", async (string id) =>
            {
                var player = await db.GetPlayerById("Players", id);
                return Results.Ok(player);
            });

            //Update player by id
            app.MapPut("/player/{id}", async (string id, Players updatePlayer) =>
            {
                var updatedPlayer = await db.UpdatePlayerById("Players", id, updatePlayer);

                if (updatedPlayer == null)
                {
                    return Results.NotFound("Could not find player");
                }
                else
                {
                    return Results.Ok(updatedPlayer);
                }
            });

            //Delete player by id
            app.MapDelete("/player({id}", async (string id) =>
            {
                var player = await db.DeletePlayer("Players", id);
                return Results.Ok(player);
            });



            app.Run();
        }
    }
}
