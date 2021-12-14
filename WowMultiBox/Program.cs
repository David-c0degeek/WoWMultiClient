using System.Diagnostics;
using System.Text.Json;
using PInvoke;
using WowMultiBox.Core;
using WowMultiBox.Keyboard;
using System.Windows.Forms;

namespace WowMultiBox;

internal static class Program
{
    private static Settings _settings = null!;

    private static readonly Dictionary<string, Process> RunningClients = new();
    public static IntPtr HookId;

    private static bool _isRunning;

    public static void Main(string[] args)
    {
        Spi.SetActiveWindowTracking(false);
        
        var doWork = Task.Run(Runner);

        HookId = InterceptKeys.SetHook(InterceptKeys.HookCallback);

        Application.Run();

        InterceptKeys.UnhookWindowsHookEx(HookId);
        Spi.SetActiveWindowTracking(false);
    }

    private static void Runner()
    {
        _settings = GetSettings() ?? throw new Exception("Must specify settings");
        
        RunProcesses();
        
        while (!_isRunning)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        
        InterceptKeys.UnhookWindowsHookEx(HookId);
        Spi.SetActiveWindowTracking(false);
    }


    private static void RunProcesses()
    {
        var isFirst = true;
        var slavePosIndex = 0;
        foreach (var characterName in _settings.CharacterNames)
        {
            var process = new Process();

            process.StartInfo.FileName = _settings.FullPath;
            process.StartInfo.Arguments = $"-config Config-{characterName}.WTF";

            process.Start();
            while (process.MainWindowHandle == IntPtr.Zero)
                Thread.Sleep(10);
            User32.SetWindowText(process.MainWindowHandle, characterName);

            if (!RunningClients.TryAdd(characterName, process))
                throw new Exception($"Failed to add process {characterName} to running list");

            if (SetWindowSizeAndLocation(process, slavePosIndex, ref isFirst)) continue;

            slavePosIndex++;
        }
    }

    private static bool SetWindowSizeAndLocation(Process process, int slavePosIndex, ref bool isFirst)
    {
        // Master
        if (isFirst)
        {
            User32.MoveWindow(process.MainWindowHandle, _settings.MasterPosX, _settings.MasterPosY,
                _settings.MasterWidth, _settings.MasterHeight, false);

            isFirst = false;
            return true;
        }

        var slavePosY = _settings.Slave1PosY + slavePosIndex * _settings.SlaveHeight;

        User32.MoveWindow(process.MainWindowHandle, _settings.Slave1PosX, slavePosY,
            _settings.SlaveWidth, _settings.SlaveHeight, false);

        return false;
    }

    private static Settings? GetSettings()
    {
        var text = File.ReadAllText("settings.json");
        return JsonSerializer.Deserialize<Settings>(text);
    }

    private static void OnProcessExit()
    {
        Spi.SetActiveWindowTracking(false);
        InterceptKeys.UnhookWindowsHookEx(HookId);
    }
}