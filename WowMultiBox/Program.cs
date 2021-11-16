using System.Diagnostics;
using System.Text.Json;
using WowMultiBox.Core;

namespace WowMultiBox;

static class Program
{
    private static Settings _settings = null!;

    private static readonly Dictionary<string, Process> RunningClients = new();

    public static void Main(string[] args)
    {
        _settings = GetSettings() ?? throw new Exception("Must specify settings");

        RunProcesses();
        
        Spi.SetActiveWindowTracking(true);
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

            // Master
            if (isFirst)
            {
                User32.MoveWindow(process.MainWindowHandle, _settings.MasterPosX, _settings.MasterPosY,
                    _settings.MasterWidth, _settings.MasterHeight, false);

                isFirst = false;
                continue;
            }

            var slavePosY = _settings.Slave1PosY + slavePosIndex * _settings.SlaveHeight;
            
            User32.MoveWindow(process.MainWindowHandle, _settings.Slave1PosX, slavePosY,
                _settings.SlaveWidth, _settings.SlaveHeight, false);

            slavePosIndex++;
        }
    }

    private static Settings? GetSettings()
    {
        var text = File.ReadAllText("settings.json");
        return JsonSerializer.Deserialize<Settings>(text);
    }
}

