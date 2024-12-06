// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Application;
using Microsoft.Extensions.DependencyInjection;
using Presentation;

Console.WriteLine($"Broadcaster ProcessId: {Process.GetCurrentProcess().Id}");

var dependencyContainer = new DependencyContainer();
var serviceProvider = dependencyContainer.Register();
var broadcastService = serviceProvider.GetRequiredService<IBroadcasterService>();

broadcastService.StartBroadcasting();


