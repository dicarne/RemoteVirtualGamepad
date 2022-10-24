using STTech.BytesIO.Core;
using System.Net;
using Vortice.XInput;
using TcpClient = STTech.BytesIO.Tcp.TcpClient;
using UdpClient = System.Net.Sockets.UdpClient;

namespace ConsoleInput;
public class Controller
{
    public int userIndex { get; set; } = 0;
    public bool ok { get; set; } = false;
    public bool connected { get; set; } = false;
    bool UseTCP { get; set; } = true;
    BytesClient? tcpClient { get; set; }
    UdpClient? udpClient { get; set; }
    public Controller(string ip, bool useTcp)
    {
        UseTCP = useTcp;
        if (UseTCP)
        {
            var iclient = new TcpClient();
            iclient.Port = Sync.Port;
            iclient.Host = ip;
            iclient.OnDataReceived += Client_OnDataReceived;
            iclient.OnConnectedSuccessfully += Client_OnConnectedSuccessfully;
            iclient.OnDisconnected += Client_OnDisconnected;
            tcpClient = iclient;
        }
        else
        {
            IPAddress locateIp = IPAddress.Parse(ip);
            udpRemoteIp = new IPEndPoint(locateIp, Sync.Port);
            udpClient = new UdpClient(Sync.Port);
            connected = true;
        }
    }
    IPEndPoint? udpRemoteIp { get; set; }
    void Connect()
    {
        if(UseTCP)
        {
            var res = tcpClient!.Connect();
            if (!res.IsSuccess)
            {
                Console.WriteLine(res.Exception.ToString());
                Thread.Sleep(100);
                Connect();
            }
            else
            {
                connected = true;
            }
        }
    }
    public void Run()
    {
        Connect();
        while (true)
        {
            try
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).KeyChar)
                    {
                        case char c when c >= '1' && c <= '4':
                            userIndex = c - '1';
                            break;
                    }
                }

                ok = XInput.GetState(userIndex, out State state);

                if (!ok)
                {
                    state = new State();    // empty state variable if GetState failed
                }

                Console.SetCursorPosition(0, 0);
                ClearLine(); Console.WriteLine($"Connected: {connected}");
                ClearLine(); Console.WriteLine($"=========================================================================");
                ClearLine(); Console.WriteLine($"Press 1-4 to select gamepad, triggers control rumble                     ");
                ClearLine(); Console.WriteLine($"=========================================================================");
                ClearLine(); Console.WriteLine($"Gamepad       : {userIndex + 1} {(ok ? "(ok)" : "(not ok)")}");
                ClearLine(); Console.WriteLine($"Buttons       : {state.Gamepad.Buttons}");
                ClearLine(); Console.WriteLine($"Left Thumb    : X = {state.Gamepad.LeftThumbX} Y = {state.Gamepad.LeftThumbY}");
                ClearLine(); Console.WriteLine($"Left Trigger  : {state.Gamepad.LeftTrigger}");
                ClearLine(); Console.WriteLine($"Right Thumb   : X = {state.Gamepad.RightThumbX} Y = {state.Gamepad.RightThumbY}");
                ClearLine(); Console.WriteLine($"Right Trigger : {state.Gamepad.RightTrigger}");
                if (connected)
                {
                    var binary = Sync.GetBytes(state.Gamepad);
                    if (UseTCP)
                    {
                        tcpClient!.Send(binary);
                    }
                    else
                    {
                        udpClient!.Send(binary, binary.Length, udpRemoteIp);
                    }

                }

                Thread.Sleep(20);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Thread.Sleep(1000);
            }
        }
    }

    private void Client_OnDisconnected(object? sender, STTech.BytesIO.Core.DisconnectedEventArgs e)
    {
        connected = false;
        Thread.Sleep(100);
        Console.WriteLine("Reconnecting...");
        Connect();
    }

    private void Client_OnConnectedSuccessfully(object? sender, STTech.BytesIO.Core.ConnectedSuccessfullyEventArgs e)
    {
        connected = true;
    }

    private void Client_OnDataReceived(object? sender, STTech.BytesIO.Core.DataReceivedEventArgs e)
    {

    }

    static void ClearLine()
    {
        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
    }
}

