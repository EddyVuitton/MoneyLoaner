using MoneyLoaner.WebAPI.Data;

namespace MoneyLoaner.WebAPI.Extensions;

public static class StateContainerExtensions
{
    public static int AddRoutingObjectParameter(this StateContainer stateContainer, object value, int hashCode)
    {
        stateContainer.ObjectTunnel[hashCode] = value;
        return hashCode;
    }

    public static T GetRoutingObjectParameter<T>(this StateContainer stateContainer, int hashCode)
    {
        return (T)stateContainer.ObjectTunnel.PopValue(hashCode);
    }
}