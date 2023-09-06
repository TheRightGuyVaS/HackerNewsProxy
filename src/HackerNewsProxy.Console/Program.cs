// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using HackerNewsProxy.Console.Helpers;
using HackerNewsProxy.Console.Infrastructure;

Console.Clear();
Console.WriteLine("Application started");
Console.WriteLine(new string('-', (int)(Console.WindowWidth * 0.6)) + Environment.NewLine);

var sw = new Stopwatch();
sw.Start();

try
{
    var serviceCollection = new ServiceCollection();
    serviceCollection.ConfigureServices();
    var serviceProvider = serviceCollection.BuildServiceProvider();

    //place debug/test code here
}
catch (Exception e)
{
    e.DisplayError();
}

sw.Stop();

Console.WriteLine(Environment.NewLine + new string('-', (int)(Console.WindowWidth * 0.6)));
Console.WriteLine($"Application finished in {sw.Elapsed}");