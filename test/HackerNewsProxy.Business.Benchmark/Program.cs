// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using HackerNewsProxy.Business.Benchmark.Cases;

var summary = BenchmarkRunner.Run<ParallelFetchingItemsTest>();