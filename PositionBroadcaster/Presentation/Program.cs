// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Presentation;

Console.WriteLine($"Broadcaster ProcessId: {Process.GetCurrentProcess().Id}");

var serviceProvider = DependencyContainer.Register();
var broadcastService = serviceProvider.GetRequiredService<IBroadcasterService>();

broadcastService.StartBroadcasting();

Console.ReadLine();


