// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Presentation;

Console.WriteLine($"Broadcaster ProcessId: {Process.GetCurrentProcess().Id}");

bool delaySimulate = false;
Console.Write(@"Do you want to simulate delay packet creation?y/n: ");
var key = Console.ReadKey();

if (key.KeyChar == 'y')
{
    delaySimulate = true;
}

Console.WriteLine();

var serviceProvider = DependencyContainer.Register();
var broadcastService = serviceProvider.GetRequiredService<IBroadcasterService>();

broadcastService.StartBroadcasting(delaySimulate);

Console.ReadLine();


