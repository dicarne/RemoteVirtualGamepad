using STTech.BytesIO.Tcp;
using Vortice.XInput;

namespace ConsoleInput;
public class Controller
{
    public int userIndex { get; set; } = 0;
    public bool ok { get; set; } = false;
    public bool connectd { get; set; } = false;
    TcpClient client { get; set; }
    public Controller(string ip)
    {
        client = new TcpClient();
        client.Port = 54391;
        client.Host = ip;
        client.OnDataReceived += Client_OnDataReceived;
        client.OnConnectedSuccessfully += Client_OnConnectedSuccessfully;
        client.OnDisconnected += Client_OnDisconnected;
    }
    void Connect()
    {
        var res = client.Connect();
        if (!res.IsSuccess)
        {
            Console.WriteLine(res.Exception.ToString());
            Thread.Sleep(1000);
            Connect();
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
                ClearLine(); Console.WriteLine($"Connected: {connectd}");
                ClearLine(); Console.WriteLine($"=========================================================================");
                ClearLine(); Console.WriteLine($"Press 1-4 to select gamepad, triggers control rumble                     ");
                ClearLine(); Console.WriteLine($"=========================================================================");
                ClearLine(); Console.WriteLine($"Gamepad       : {userIndex + 1} {(ok ? "(ok)" : "(not ok)")}");
                ClearLine(); Console.WriteLine($"Buttons       : {state.Gamepad.Buttons}");
                ClearLine(); Console.WriteLine($"Left Thumb    : X = {state.Gamepad.LeftThumbX} Y = {state.Gamepad.LeftThumbY}");
                ClearLine(); Console.WriteLine($"Left Trigger  : {state.Gamepad.LeftTrigger}");
                ClearLine(); Console.WriteLine($"Right Thumb   : X = {state.Gamepad.RightThumbX} Y = {state.Gamepad.RightThumbY}");
                ClearLine(); Console.WriteLine($"Right Trigger : {state.Gamepad.RightTrigger}");
                if (connectd)
                {
                    var binary = Sync.GetBytes(state.Gamepad);
                    client.Send(binary);
                }

                Thread.Sleep(10);
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
        connectd = false;
        Thread.Sleep(100);
        Console.WriteLine("Reconnecting...");
        Connect();
    }

    private void Client_OnConnectedSuccessfully(object? sender, STTech.BytesIO.Core.ConnectedSuccessfullyEventArgs e)
    {
        connectd = true;
    }

    private void Client_OnDataReceived(object? sender, STTech.BytesIO.Core.DataReceivedEventArgs e)
    {

    }

    static void ClearLine()
    {
        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
    }
}

