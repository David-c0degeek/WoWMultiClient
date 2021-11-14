namespace WowMultiBox.Core;

public class Settings
{
    public string GamePath { get; set; }
    public string GameExe { get; set; }
    public string GameName { get; set; }
    public List<string> CharacterNames { get; set; }

    public string FullPath => $"{GamePath}{GameExe}";
    
    public int SlaveHeight { get; set; }
    public int SlaveWidth { get; set; }
    public int Slave1PosX { get; set; }
    public int Slave1PosY { get; set; }
    public int SlaveFocusHeight { get; set; }
    public int SlaveFocusWidth { get; set; }
    public int SlaveFocusPosX { get; set; }
    public int SlaveFocusPosY { get; set; }
    public int MasterPosX { get; set; }
    public int MasterPosY { get; set; }
    public int MasterWidth { get; set; }
    public int MasterHeight { get; set; }
}