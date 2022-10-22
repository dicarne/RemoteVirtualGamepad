
using ConsoleInput;
var client = false;
var ip = "";

if(args.Length >= 2 && args[0] == "client")
{
    client = true;
    ip = args[1];
}

if (client)
{
    var master = new Controller(ip);
    master.Run();
}
else
{
    var handle = new HandleController();
    await handle.Run();
}



