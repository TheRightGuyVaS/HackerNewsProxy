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

## Main enhancements

- Using ``MemoryCache`` to cache fetched items for 30 minutes, so next calls with less items to fetch than before will use values from cache. For calls with number of items to get application will fetch from Hacker New API only missing ones.
- Using ``AutoMapper`` gives us full controll on mapping entieties, with veryfiyng correct mapping and getting notified when code modified and mapping should not work without changes.
- Using ``Parallel`` library allows us to fetch data from Hacker New API in most efficient way, using as many threads as available on the server.
- Using ``BenchmarkDotNet`` allows us to understand which approach can give us more efficiency and how it depends on data scale.
- Using **DependencyInjectionContainerTest** for each project can notify us about missing service registrations, which reduced time, cost and chance of fail of CI/CD process.
- Using ``Polly`` retry policies allows us to prevent failing due to from time to time Hacker New API method failing with Timeouts or other exceptions.

## Sources of knowledge

 - https://github.com/HackerNews/API

## Additional thoughts

- Throwing exception when Hacker New API returns us **null** during fetching item by id can be changed into handling it. When we need to fetch **n** best stories we fetch them, check if they are not **null**, and stop not only after predefined number of iterations (NASA would say it is awful) but when either we ran out of item ids or we fetched needed amount of items.