// See https://aka.ms/new-console-template for more information
using Altairis.Services.StaticContent.Benchmarks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var config = DefaultConfig.Instance.With(ConfigOptions.DisableOptimizationsValidator);
BenchmarkDotNet.Running.BenchmarkRunner.Run<StoresBenchmarks>(config);
