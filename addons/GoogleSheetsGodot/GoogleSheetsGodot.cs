#if TOOLS
using Godot;
using System;

[Tool]
public partial class GoogleSheetsGodot : EditorPlugin
{
    private Control _dock;

    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        GD.Print("Plugin is activated");
        GD.Print("Put all your csv files inside res://Resources/CSV");
        // Vérifie si le dossier Resources existe
        if (!DirAccess.DirExistsAbsolute("res://Resources"))
        {
            DirAccess.MakeDirAbsolute("res://Resources");
        }
        // Vérifie si le dossier CSV existe dans Resources
        if (!DirAccess.DirExistsAbsolute("res://Resources/CSV"))
        {
            DirAccess.MakeDirAbsolute("res://Resources/CSV");
        }
        _dock = GD.Load<PackedScene>("uid://dvuaevc3mhlep").Instantiate<Control>();
        AddControlToDock(DockSlot.RightBl, _dock);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        // Remove the dock.
        RemoveControlFromDocks(_dock);
        // Erase the control from the memory.
        _dock.Free();
    }
}
#endif
