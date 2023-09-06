﻿using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HackerNewsProxy.Business.UnitTests;

public class DependencyInjectionContainerTest
{
    [Fact]
    public void ShouldValidateDiContainer()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.ConfigureServices();

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