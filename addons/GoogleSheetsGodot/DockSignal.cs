using System;
using System.Threading.Tasks;
using Godot;

[Tool]
public partial class DockSignal : Control
{
    [Export]
    private TextEdit _TextEditGoogleSheetsLink;

    [Export]
    private TextEdit _TextEditResourceScriptUid;

    [Export]
    private Button _buttonConvert;

    public override void _Ready()
    {
        _buttonConvert.Pressed += _on_button_convert_pressed;
    }

    private async void _on_button_convert_pressed()
    {
        // Creation du json avec l'url Google Sheets et le Resource Script Correspondant
        var gsheetsConnectToResource = new GsheetsConnectToResource();
        gsheetsConnectToResource.SaveToJson(
            _TextEditGoogleSheetsLink.Text,
            _TextEditResourceScriptUid.Text
        );

        //Import GoogleSheets vers CSV registered in json
        var import = new Import();
        await import.ImportGoogleSheetsFromJson();

        // Convertit tous les CSV en Resources
        var conversion = new Conversion();
        conversion.Convert();
    }
}
