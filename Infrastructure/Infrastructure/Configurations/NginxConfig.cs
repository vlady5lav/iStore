namespace Infrastructure.Configurations;

public class NginxConfig
{
    public const string Nginx = "Nginx";

    public string UseNginx { get; set; } = null!;

    public string UseInitFile { get; set; } = null!;

    public string UseUnixSocket { get; set; } = null!;

    public string UsePort { get; set; } = null!;

    public string InitFilePath { get; set; } = null!;

    public string UnixSocketPath { get; set; } = null!;

    public string Port { get; set; } = null!;
}
