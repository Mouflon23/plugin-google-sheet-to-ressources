#if TOOLS
using Godot;
using System;

[Tool]
public partial class GoogleSheetsGodot : EditorPlugin
{
    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        GD.Print("Plugin is activated");
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
    }
}
#endif
