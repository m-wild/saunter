﻿using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal interface IServiceProviderBuilder
{
    IServiceProvider BuildServiceProvider(string startupAssembly);
}

internal class ServiceProviderBuilder(ILogger<ServiceProviderBuilder> logger) : IServiceProviderBuilder
{
    public IServiceProvider BuildServiceProvider(string startupAssembly)
    {
        var fullPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), startupAssembly));
        logger.LogInformation($"Loading startup assembly: {fullPath}");
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);
        var nswagCommandsAssembly = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(ServiceProviderBuilder).Assembly.Location), "NSwag.Commands.dll")));
        var nswagServiceProvider = nswagCommandsAssembly.GetType("NSwag.Commands.ServiceProviderResolver");
        var serviceProvider = (IServiceProvider)nswagServiceProvider.InvokeMember("GetServiceProvider", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, null, [assembly]);
        return serviceProvider;
    }
}
