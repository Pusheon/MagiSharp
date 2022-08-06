using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Magisharp.Commands;

namespace Magisharp;

public class Bot
{
    public Credentials Credentials { get; private set; }

    private readonly IServiceProvider _services;

    private readonly InteractionHandler _interactions;

    private readonly InteractionService _interactionService;

    private readonly DiscordSocketClient _client = new DiscordSocketClient(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.Guilds,
#if DEBUG
        DefaultRatelimitCallback = info =>
        {
            Console.WriteLine($"\x1b[95mREQUEST\x1b[0m to \x1b[42;30;7m{Regex.Replace(info.Endpoint, @"[^\/\\]{18,}", "x", RegexOptions.Compiled)}\x1b[0m completed.\n"
                              + $"   limit:     \x1b[42;30;7m{info.Limit?.ToString() ?? "?"}\x1b[0m\n"
                              + $"   remaining: \x1b[42;30;7m{info.Remaining?.ToString() ?? "?"}\x1b[0m\n"
                              + $"   reset at:  \x1b[42;30;7m{info.ResetAfter?.ToString() ?? "?"}\x1b[0m\n"
            );
            return Task.CompletedTask;
        },
        LogLevel = LogSeverity.Debug,
#endif
    });


    public Bot()
    {
        _services = new ServiceCollection().AddSingleton(_client).BuildServiceProvider();

        _interactionService = new(_client, new()
        {
            UseCompiledLambda = true,
            ExitOnMissingModalField = false
        });

        Credentials = Credentials.Load().GetAwaiter().GetResult();

        _interactions = new InteractionHandler(_client, _interactionService, _services, Credentials);

        _interactions.Log += LogHandler;
        _interactionService.Log += LogHandler;
        _client.Log += LogHandler;
    }

    /// <summary>
    ///     WARNING: <br/> This method will block the executing thread for the lifetime of the application.
    /// </summary>
    public async Task RunAsync()
    {
        await _interactions.InitializeAsync();
        await _client.LoginAsync(TokenType.Bot, Credentials.Token);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    public Task LogHandler(LogMessage message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}
