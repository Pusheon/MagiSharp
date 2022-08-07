using System.Reflection;

namespace Magisharp;

internal class InteractionHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _services;
    #if DEBUG
    private const bool DebugMode = true;
    #else
    private const bool DebugMode = false;
    #endif
    private readonly Logger _logger;
    private readonly ulong _debugGuild;
    private bool _firstClientReady = true;

    public event Func<LogMessage, Task> Log;

    public InteractionHandler(DiscordSocketClient client, InteractionService interaction, IServiceProvider service,
        Credentials credentials)
    {
        _logger = new("Command Handler", LogSeverity.Debug);
        _logger.LogFired += x => Log.Invoke(x);
        _client = client;
        _interactionService = interaction;
        _services = service;
        _debugGuild = credentials.DevGuildId;
        client.Ready += ClientReady;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogVerbose("Adding TypeConverters");
            // Add Typeconverters here, format:
            // _interactionService.AddGenericTypeConverter<Poll>(typeof(PollTypeConverter<>));


            await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;

            _interactionService.InteractionExecuted += InteractionExecuted;
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Failed to start interactions service.", ex);
        }
    }

    private async Task InteractionExecuted(ICommandInfo command, IInteractionContext context, IResult result)
    {
        if (result.IsSuccess)
        {
            _logger.LogDebug($"Successfully executed the interaction {command.Name}");
        }
        else
        {
            await InteractionFailHandle(command.Name, result, context);
        }
    }

    private static async Task InteractionFailHandle(string name, IResult result, IInteractionContext context)
    {
        var interaction = context.Interaction as SocketInteraction;
        await interaction.RespondAsync($"Command failed for the following reason:\n{result.ErrorReason}", ephemeral: true);

        // TODO: Notify user that a command failed.
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        var context = new SocketInteractionContext(_client, interaction);
        await _interactionService.ExecuteCommandAsync(context, _services);
    }

    private async Task ClientReady()
    {
        // make the function only run once
        if (!_firstClientReady) return;
        _firstClientReady = false;

        var guild = await _client.Rest.GetGuildAsync(_debugGuild);
        
        if (!DebugMode)
        {
            await _interactionService.RegisterCommandsGloballyAsync();
        }
        else await _interactionService.RegisterCommandsToGuildAsync(guild.Id);
    }
}
