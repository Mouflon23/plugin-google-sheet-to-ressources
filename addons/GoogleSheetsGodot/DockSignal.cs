using System;
using Godot;

[Tool]
public partial class DockSignal : Control
{
    [Export]
    private Button _button;

    [Export]
    private CheckButton _checkButton;

    public override void _Ready()
    {
        _button.Pressed += _on_button_pressed;
        _checkButton.Toggled += _on_check_button_toggled;
    }

    private void _on_button_pressed()
    {
        GD.Print("Button pressed");
        var conversion = new Conversion();
        conversion.Convert();
    }

    private void _on_check_button_toggled(bool toggled_on)
    {
        if (toggled_on)
        {
            GD.Print(toggled_on);
        }
        else
        {
            GD.Print(toggled_on);
        }
    }
}
