using Microsoft.Extensions.DependencyInjection;

namespace NetSendGridEmailClient.Functions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceLifetimeAttribute : Attribute
{
    public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }

    public ServiceLifetime ServiceLifetime { get; }
}
