// See https://aka.ms/new-console-template for more information

using Application.Interfaces;
using Presentation;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine($"Consumer ProcessId: {Process.GetCurrentProcess().Id}");
Console.WriteLine($"Please check the log file in the bin folder to see the received positions and the aggregates.");
Console.WriteLine($"Press enter to stop/exit.");

var serviceProvider = DependencyContainer.Register();
var positionReceiverProcess = serviceProvider.GetRequiredService<IPositionConsumerProcess>();

await positionReceiverProcess.StartAsync();

Console.ReadLine();