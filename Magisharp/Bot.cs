using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Magisharp;

public class Bot
{
    public Bot()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMembers
        });
    }

    private readonly DiscordSocketClient _client;

    private readonly IServiceCollection _services;

    private void AddServices()
    {
        _services.AddSingleton(_client);
    }

    public async Task StartAsync()
    {
        
    }

    public async Task RunAndBlockAsync()
    {
        Task.Delay(-1);
    }
}