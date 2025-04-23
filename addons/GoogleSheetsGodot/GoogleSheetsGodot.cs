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
        GD.Print(
            "Copy/Paste the link of your GoogleSheet, link share with Editor mode from your Drive."
        );
        GD.Print(
            "Copy/paste the UID of your Resource Script that need to be used to generate Resource with the GoogleSheet."
        );
        _dock = GD.Load<PackedScene>("uid://dvuaevc3mhlep").Instantiate<Control>();
        AddControlToDock(DockSlot.LeftBr, _dock);
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
