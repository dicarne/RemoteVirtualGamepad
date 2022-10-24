# RemoteVirtualGamepad

## 用途
用于在远程桌面中使用手柄玩游戏。

目前大多数的远程桌面并不支持手柄，支持手柄的远程软件要么用不了，要么卡，要么倒闭了。于是写了这么一个软件，专门用于同步手柄的操作，现在你可以使用任何你喜欢的远程桌面软件观看画面，并使用手柄进行游戏了！

## 使用指南
1. 首先确保已经安装了虚拟手柄驱动：[ViGEmBus](https://github.com/ViGEm/ViGEmBus)，从Release中下载安装程序并安装即可。
2. 确保两台电脑在局域网或虚拟局域网中（如 n2n、zerotier ），并知道IP。
3. 下载可执行程序，放在两台电脑中。
4. 在没有手柄的电脑上，直接运行`RemoteVirtualGamepad.exe`。
5. 在有手柄的电脑上，通过命令行运行，或增加命令行参数运行，如`RemoteVirtualGamepad.exe gamepad 192.168.100.200`，其中`192.168.100.200`替换为没有手柄的那个电脑的IP。
6. 正常运行！不出意外的话，没有手柄的电脑命令行将显示连接信息；有手柄的电脑上命令行显示`Connected: true`。如果失败的话，会有错误信息。

## TCP
现在默认使用UDP进行通信。如果出现问题，尝试切换成TCP。
```
RemoteVirtualGamepad.exe --tcp:true
```

```
RemoteVirtualGamepad.exe --tcp:true gamepad 192.168.100.200
```

## 注意
通信端口是`54391`，注意不要冲突。如不能连接，考虑手动开放一下防火墙端口。

## TODO
或许可以改进一下传输的频率、增加时间戳丢弃过时的状态。但是目前测试没有什么问题，就先不做了。

## 感谢
XInput .net 绑定: https://github.com/amerkoleci/Vortice.Windows

虚拟手柄: https://github.com/ViGEm/ViGEm.NET

网络库: https://github.com/landriesnidis/STTech.BytesIO
