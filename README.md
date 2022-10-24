# RemoteVirtualGamepad
[中文](README_zhcn.md)

## Usage
For playing games with a gamepad in Remote Desktop.

Currently most remote desktops do not support gamepads, and the remote software that does support gamepads either doesn't work, is stuck, or has gone out of business. So this software was written to synchronize gamepad operation, and now you can use any remote desktop software you like to watch the screen and play games with the gamepad!

## How to use
1. First make sure you have installed the virtual gamepad driver: [ViGEmBus](https://github.com/ViGEm/ViGEmBus), just download the installer from Release and install it.
2. Make sure the two computers are on a LAN or virtual LAN (e.g. n2n, zerotier) and know the IP.
3. Download the executable program and put it in both computers.
4. On the computer without a gamepad, run `RemoteVirtualGamepad.exe` directly.
5. On the computer with the handle, run it from the command line or add command line parameters, such as `RemoteVirtualGamepad.exe gamepad 192.168.100.200`, where `192.168.100.200` is replaced by the IP of the computer without the handle.
6. Run it! If nothing happens, the command line of the computer without the handle will show the connection information; the command line on the computer with the handle will show `Connected: true`. If it fails, there will be an error message.

## TCP
UDP is now used by default. If problems occur, try switching to TCP.
```
RemoteVirtualGamepad.exe --tcp:true
```

```
RemoteVirtualGamepad.exe --tcp:true gamepad 192.168.100.200
```
## Be careful
The communication port is `54391`, be careful not to conflict. If you cannot connect, consider opening the firewall port manually.

## TODO
Maybe we can improve the frequency of transmission, add timestamps to discard obsolete state. But there is nothing wrong with the current test, so let's leave it for now.

## Thanks
XInput .net binding: https://github.com/amerkoleci/Vortice.Windows

Virtual gamepad: https://github.com/ViGEm/ViGEm.NET

Network lib: https://github.com/landriesnidis/STTech.BytesIO