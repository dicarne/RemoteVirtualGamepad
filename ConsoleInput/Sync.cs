using System.Runtime.InteropServices;

namespace ConsoleInput;

public class Sync
{
    public static byte[] GetBytes<TStruct>(TStruct data) where TStruct : struct
    {
        int structSize = Marshal.SizeOf(typeof(TStruct));
        byte[] buffer = new byte[structSize];
        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        Marshal.StructureToPtr(data, handle.AddrOfPinnedObject(), false);
        handle.Free();
        return buffer;
    }

    public static TStruct GetStruct<TStruct>(byte[] buffer) where TStruct : struct
    {
        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var str = Marshal.PtrToStructure<TStruct>(handle.AddrOfPinnedObject());
        handle.Free();
        return str;
    }
}