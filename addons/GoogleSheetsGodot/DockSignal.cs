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
        // Make required directory
        SetupAddons setupAddons = new();
        setupAddons.MakeRequiredDirectory();
        // Creation du json avec l'url Google Sheets et le Resource Script Correspondant
        var gsheetsConnectToResource = new GsheetsConnectToResource();
        await gsheetsConnectToResource.SaveToJson(
            _TextEditGoogleSheetsLink.Text,
            _TextEditResourceScriptUid.Text
        );
        GD.Print("Informations Saved !");

        //Import GoogleSheets vers CSV registered in json
        var import = new Import();
        await import.ImportGoogleSheetsFromJson();
        GD.Print("Import Finished !");

        // Convertit tous les CSV en Resources
        var conversion = new Conversion();
        await conversion.Convert();
        GD.Print("Conversion Done !");
    }
}
