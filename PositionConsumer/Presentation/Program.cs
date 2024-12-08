// See https://aka.ms/new-console-template for more information

using Application.Interfaces;
using Presentation;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine($"Consumer ProcessId: {Process.GetCurrentProcess().Id}");

var serviceProvider = DependencyContainer.Register();
var positionReceiverService = serviceProvider.GetRequiredService<IPositionReceiverService>();

await positionReceiverService.StartAsync();

Console.ReadLine();