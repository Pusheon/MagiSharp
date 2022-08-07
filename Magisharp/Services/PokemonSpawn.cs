using System.Net.WebSockets;
using System.Runtime.InteropServices;
using PokeApiNet;

namespace Magisharp.Services;

public class PokemonSpawn
{
    private readonly PokeApiClient _pokeApiClient;

    public PokemonSpawn(DiscordSocketClient client, PokeApiClient pokeApiClient)
    {
        _pokeApiClient = pokeApiClient;
        client.MessageReceived += ClientOnMessageReceived;
    }

    private Task ClientOnMessageReceived(SocketMessage arg)
    {
        _ = Task.Run(async () =>
        {
            if (arg.Author.IsBot)
                return;
            var pokemonInfo = await _pokeApiClient.GetResourceAsync<Pokemon>("8");
            var eb = new EmbedBuilder()
                .WithDescription("A new pokemon has spawned! Type it's name to catch it!")
                .WithThumbnailUrl(pokemonInfo.Sprites.FrontDefault);
            await arg.Channel.SendMessageAsync(embed: eb.Build());
        });
        return Task.CompletedTask;
    }
}