# Hacker News Proxy

An application which provides method to fetch top ```n``` best stroies from Hacher News API.

The main feature of the application is caching which allows user to fetch a lot of items without trottling.

## How to run the application

To start API please navigate to **src/HackerNewsProxy.Api** and run application.

For example on PC with .NET Framework 6 installed you can use next command in Windows Command Prompt:

`dotnet run`

## Api definition

Application contains next controllers with methods:

- `/stories`
    - **GET** `/best/{n}` - returnes **n** ``ItemModel``s from Hacker News API with highest score in descending order by score

Models definition:
- `ItemModel`
    - title - string, nullable
    - uri - string(Uri), nullable
    - postedBy - string, nullable
    - time - string(DateTime)
    - score - integer(Int32)
    - commentCount - integer(Int32)

**GET** methods return data in ``JSON`` format.

## Solution projects

- src
    - **HackerNews.Api.SDK** - .NET6 library project which contain HttpClient for Hacker New API
    - **HackerNewsProxy.Api** - .NET6 Web Api project, solution entry point, contains main API
    - **HackerNewsProxy.Business** - .NET6 library project which contains main business logic functionality of the solution
    - **HackerNewsProxy.Console** - .NET6 console project which initializes HackerNewsProxy.Business DependencyInjection container and can be used for debugging/manual testing the application logic without API layer
- test
    - **HackerNews.Api.SDK.UnitTests** - xUnit Test Project for the **HackerNews.Api.SDK** project
    - **HackerNewsProxy.Api.UnitTests** - xUnit Test Project for the **HackerNewsProxy.Api** project
    - **HackerNewsProxy.Business.Benchmark** - .NET6 console project which contains infrastructure for executing Benchmark test on the **HackerNewsProxy.Business** project functionality using NuGet package ``BenchmarkDotNet``
    - **HackerNewsProxy.Business.UnitTests** - xUnit Test Project for the **HackerNewsProxy.Business** project
    - **HackerNewsProxy.Console.UnitTests** - xUnit Test Project for the **HackerNewsProxy.Console** project

## Sources of knowledge

 - https://github.com/HackerNews/API