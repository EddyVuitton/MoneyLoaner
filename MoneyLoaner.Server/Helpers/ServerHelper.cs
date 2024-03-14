using MoneyLoaner.Server.Extentions;
using MudBlazor.Services;

namespace MoneyLoaner.Server.Helpers
{
    public static class ServerHelper
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddMudServices();

            builder.Services.AddServerServices();
            builder.Services.AddAuthServices();
        }
    }
}