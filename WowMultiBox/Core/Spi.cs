using System.Runtime.InteropServices;
using PInvoke;
using Win32Exception = System.ComponentModel.Win32Exception;

namespace WowMultiBox.Core;

public static class Spi
{
    private static void Check(bool ok)
    {
        if (!ok)
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private static IntPtr ToIntPtr(this bool value)
    {
        return new IntPtr(value ? 1u : 0u);
    }

    public static void SetActiveWindowTracking(bool enabled)
    {
        Check(User32.SystemParametersInfo(User32.SystemParametersInfoAction.SPI_SETACTIVEWINDOWTRACKING, 0,
            enabled.ToIntPtr(), User32.SystemParametersInfoFlags.SPIF_SENDCHANGE));
    }
}