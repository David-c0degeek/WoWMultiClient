using System.Diagnostics;
using System.Runtime.InteropServices;
using WowMultiBox.Core;

namespace WowMultiBox.Keyboard;

public class InterceptKeys
{
    public delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    public const int WH_KEYBOARD_LL = 13;
    public const int WM_KEYDOWN = 0x0100;
    public static LowLevelKeyboardProc _proc = HookCallback;
    public static IntPtr _hookID = IntPtr.Zero;

    public static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;

        return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
            GetModuleHandle(curModule.ModuleName), 0);
    }

    public static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode < 0 || wParam != (IntPtr)WM_KEYDOWN)
            return CallNextHookEx(_hookID, nCode, wParam, lParam);

        var vkCode = Marshal.ReadInt32(lParam);
        Console.WriteLine((Keys)vkCode);

        switch (vkCode)
        {
            case (int)Keys.Escape:
                Environment.Exit(0);
                UnhookWindowsHookEx(Program.HookId);
                Spi.SetActiveWindowTracking(false);
                break;
            case (int)Keys.LControlKey + (int)Keys.F5:
                Spi.SetActiveWindowTracking(true);
                Console.WriteLine("Window tracking activated");
                break;
            case (int)Keys.LControlKey + (int)Keys.F6:
                Spi.SetActiveWindowTracking(false);
                Console.WriteLine("Window tracking deactivated");
                break;
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
}