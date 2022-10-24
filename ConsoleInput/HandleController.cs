using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Vortice.XInput;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using STTech.BytesIO.Tcp;
using System.Net.Sockets;
using System.Net;

namespace ConsoleInput;
public class HandleController
{
    ViGEmClient client;
    IXbox360Controller control;
    Gamepad current;
    bool UseTCP { get; set; } = true;
    public HandleController(bool useTCP)
    {
        UseTCP = useTCP;
        client = new ViGEmClient();
        control = client.CreateXbox360Controller();
        control.Connect();
        control.AutoSubmitReport = false;
    }
    public async Task Run()
    {
        if (UseTCP)
        {
            var server = new TcpServer();
            server.Port = Sync.Port;
            server.Started += Server_Started;
            server.Closed += Server_Closed;
            server.ClientConnected += Server_ClientConnected;
            server.ClientDisconnected += Server_ClientDisconnected;
            await server.StartAsync();
        }
        else
        {
            var server2 = new UdpClient(Sync.Port);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                var receiveBytes = server2.Receive(ref RemoteIpEndPoint);
                receiveData(receiveBytes);
            }
        }
    }


    void SyncGamePad()
    {
        control.SetButtonState(Xbox360Button.A, current.Buttons.HasCode(GamepadButtons.A));
        control.SetButtonState(Xbox360Button.B, current.Buttons.HasCode(GamepadButtons.B));
        control.SetButtonState(Xbox360Button.X, current.Buttons.HasCode(GamepadButtons.X));
        control.SetButtonState(Xbox360Button.Y, current.Buttons.HasCode(GamepadButtons.Y));
        control.SetButtonState(Xbox360Button.Start, current.Buttons.HasCode(GamepadButtons.Start));
        control.SetButtonState(Xbox360Button.Back, current.Buttons.HasCode(GamepadButtons.Back));
        control.SetButtonState(Xbox360Button.LeftShoulder, current.Buttons.HasCode(GamepadButtons.LeftShoulder));
        control.SetButtonState(Xbox360Button.RightShoulder, current.Buttons.HasCode(GamepadButtons.RightShoulder));
        control.SetButtonState(Xbox360Button.LeftThumb, current.Buttons.HasCode(GamepadButtons.LeftThumb));
        control.SetButtonState(Xbox360Button.RightThumb, current.Buttons.HasCode(GamepadButtons.RightThumb));

        control.SetButtonState(Xbox360Button.Up, current.Buttons.HasCode(GamepadButtons.DPadUp));
        control.SetButtonState(Xbox360Button.Down, current.Buttons.HasCode(GamepadButtons.DPadDown));
        control.SetButtonState(Xbox360Button.Left, current.Buttons.HasCode(GamepadButtons.DPadLeft));
        control.SetButtonState(Xbox360Button.Right, current.Buttons.HasCode(GamepadButtons.DPadRight));

        control.SetButtonState(Xbox360Button.Guide, current.Buttons.HasCode(GamepadButtons.Guide));

        control.SetAxisValue(Xbox360Axis.LeftThumbY, current.LeftThumbY);
        control.SetAxisValue(Xbox360Axis.LeftThumbX, current.LeftThumbX);
        control.SetAxisValue(Xbox360Axis.RightThumbY, current.RightThumbX);
        control.SetAxisValue(Xbox360Axis.RightThumbY, current.RightThumbY);
        control.SubmitReport();
    }

    private void Server_ClientDisconnected(object? sender, STTech.BytesIO.Tcp.Entity.ClientDisconnectedEventArgs e)
    {
        Console.WriteLine($"Disconnected, from {e.Client.Host}");
    }

    private void Server_ClientConnected(object? sender, STTech.BytesIO.Tcp.Entity.ClientConnectedEventArgs e)
    {
        e.Client.OnDataReceived += Client_OnDataReceived;
        Console.WriteLine($"Connected! from {e.Client.Host}");
    }

    private void Client_OnDataReceived(object? sender, STTech.BytesIO.Core.DataReceivedEventArgs e)
    {
        
        receiveData(e.Data);
    }

    void receiveData(byte[] data)
    {
        current = Sync.GetStruct<Gamepad>(data);
        SyncGamePad();
    }

    private void Server_Closed(object? sender, EventArgs e)
    {

    }

    private void Server_Started(object? sender, EventArgs e)
    {
        Console.WriteLine("Start!");
        Console.WriteLine($"Ensure you have installed ViGEmBus driver！");
    }
}

public static class GamepadHelper
{
    public static bool HasCode(this GamepadButtons b, GamepadButtons target)
    {
        return (b & target) != 0;
    }
}