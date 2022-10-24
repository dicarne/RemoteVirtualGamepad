using ConsoleInput;
using System.CommandLine;

var tcpOption = new Option<bool>(
        name: "--tcp",
        description: "use tcp instead of udp.",
        getDefaultValue: () => false
    );
var gamepadCmd = new Command("gamepad", "I have a gamepad!");
var hostArg = new Argument<string>("target_ip", "IP of your friends.");
gamepadCmd.Add(hostArg);
gamepadCmd.SetHandler((hostArgV, useTcp) =>
{
    Console.WriteLine($"Host: {hostArgV}");
    Console.WriteLine($"Tcp: {useTcp}");
    var master = new Controller(hostArgV, useTcp);
    master.Run();

}, hostArg, tcpOption);
var commands = new RootCommand();
commands.Add(gamepadCmd);
commands.AddGlobalOption(tcpOption);
commands.SetHandler(async (useTcp) =>
{
    Console.WriteLine($"Tcp: {useTcp}");
    var handle = new HandleController(useTcp);
    await handle.Run();
}, tcpOption);
return await commands.InvokeAsync(args);