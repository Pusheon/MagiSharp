namespace Magisharp.Commands;

public class Pokemon : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("sample", "an example command")]
    public async Task Command
    (
        [Summary("option-1", "the first option")]
        string optionOne,
        [Summary("option-2", "the second option")]
        int optionTwo = 3
    )
    {
        var e = new EmbedBuilder()
            .WithTitle("Options!")
            .WithDescription(
                $"This embed was built using the {(optionTwo == 3 ? "one option" : "two options")} you provided.")
            .AddField("option 1", optionOne)
            .AddField("option 2", optionTwo.ToString())
            .WithColor(Color.Magenta)
            .Build();
        await RespondAsync(embed:e, ephemeral:true);
    }
}
