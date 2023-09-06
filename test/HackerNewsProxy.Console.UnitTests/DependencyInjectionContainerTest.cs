using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using HackerNewsProxy.Console.Infrastructure;
using Xunit;

namespace HackerNewsProxy.Console.UnitTests;

public class DependencyInjectionContainerTest
{
    [Fact]
    public void ShouldValidateDiContainer()
    {
        var serviceCollection = new ServiceCollection();
        DependencyInjectionModule.ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        using (new AssertionScope())
        {
            foreach (var serviceDescriptor in serviceCollection)
            {
                if (serviceDescriptor.ServiceType.ContainsGenericParameters)
                {
                    continue;
                }

                serviceProvider.GetRequiredService(serviceDescriptor.ServiceType);
            }
        }
    }
}