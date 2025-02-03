using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
const string GetGameEndpointName = "GetGame";
private static readonly List<GameDto> games = [
    new(1, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new(2, "Elden Ring", "Action RPG", 69.99m, new DateOnly(2022, 2, 25)),
    new(3, "God of War Ragnarök", "Action-Adventure", 69.99m, new DateOnly(2022, 11, 9))
    ];

    public static WebApplication MapGamesEndpoints(this WebApplication app) {
                // GET games
        app.MapGet("games", () => games);

        // Get games/1
        app.MapGet("games/{id}", (int id) => {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
            }).WithName(GetGameEndpointName);

        //Post games
        app.MapPost("games", (CreateGameDto newGame) => {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
        });

        // Put game
        app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) => {
            var index = games.FindIndex(game => game.Id == id);

            if(index == -1){
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // Delete
        app.MapDelete("games/{id}", (int id) => {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return app;
    }
}
