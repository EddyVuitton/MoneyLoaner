using MoneyLoaner.Components.Layout;
using System.Reflection;

namespace MoneyLoaner.Server;

public partial class App
{
    private readonly List<Assembly> _additionalAssemblies = new()
    {
        typeof(MainLayout).Assembly,
    };
}