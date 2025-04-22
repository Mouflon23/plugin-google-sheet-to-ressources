using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

[Tool]
public partial class Import : Node
{
    public async Task ImportGoogleSheetsFromJson()
    {
        string filePath = "res://Resources/sheets_connection.json";
        if (Godot.FileAccess.FileExists(filePath))
        {
            using (var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read))
            {
                string jsonContent = file.GetAsText();
                var data = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<
                    string,
                    System.Collections.Generic.Dictionary<string, string>
                >>(jsonContent);

                foreach (var entry in data)
                {
                    string googleSheetUrl = entry.Value["googleSheetUrl"];

                    var googleSheetsUtility = new GoogleSheetsUtility();
                    await googleSheetsUtility.ImportGoogleSheet(googleSheetUrl);
                }
            }
        }
    }
}
