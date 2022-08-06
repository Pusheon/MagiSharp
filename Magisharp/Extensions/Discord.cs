namespace Discord;

public static partial class DiscordExtensions
{
    public static string GetShortName(this LogSeverity severity) => severity switch
    {
        LogSeverity.Critical => "crt",
        LogSeverity.Error => "err",
        LogSeverity.Warning => "wrn",
        LogSeverity.Info => "inf",
        LogSeverity.Verbose => "vrb",
        LogSeverity.Debug => "dbg",
        _ => throw new ArgumentException("severity must be a valid LogSeverity.")
    };

    public static Color GetColor(this LogSeverity severity) => severity switch
    {
        LogSeverity.Critical => Color.DarkRed,
        LogSeverity.Error => Color.Red,
        LogSeverity.Warning => Color.Orange,
        LogSeverity.Info => (Color)System.Drawing.Color.White,
        LogSeverity.Verbose => Color.LightGrey,
        LogSeverity.Debug => Color.LighterGrey,
        _ => throw new ArgumentException("severity must be a valid LogSeverity.")
    };
}
