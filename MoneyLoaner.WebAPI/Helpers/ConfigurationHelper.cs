namespace MoneyLoaner.WebAPI.Helpers;

public static class ConfigurationHelper
{
    public static IConfiguration? Config { get; set; }

    public static void Initialize(IConfiguration Configuration)
    {
        Config = Configuration;
    }
}