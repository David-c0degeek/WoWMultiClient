using System.Runtime.InteropServices;

namespace WowMultiBox;

public static class User32
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern void SetWindowText(IntPtr hWnd, string windowName);
    
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

}