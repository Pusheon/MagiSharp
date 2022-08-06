using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Magisharp;

public class Credentials
{
    public ulong[] TrustedContributors { get; set; }
    public string Token { get; set; }
    public ulong DevGuildId { get; set; }

    public static async Task<Credentials> Load(string path = "creds.yml")
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var content = await File.ReadAllTextAsync(path);
        return deserializer.Deserialize<Credentials>(content);
    }
}
