namespace Infrastructure.Configurations;

public class AuthorizationConfig
{
    public const string Authorization = "Authorization";

    public string Authority { get; set; } = null!;

    public string SiteAudience { get; set; } = null!;
}
