using MoneyLoaner.Server.Services;
using MudBlazor.Services;

namespace MoneyLoaner.Server.Helpers
{
    public static class ServerHelper
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMudServices();
            builder.Services.AddDebcior();
        }
    }
}