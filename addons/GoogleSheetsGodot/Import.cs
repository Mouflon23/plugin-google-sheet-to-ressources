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
        GD.Print("Tentative d'accès au fichier JSON...");

        if (Godot.FileAccess.FileExists(filePath))
        {
            GD.Print("Fichier JSON trouvé !");
            using (var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read))
            {
                string jsonContent = file.GetAsText();
                GD.Print("Contenu du JSON lu avec succès");

                var data = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<
                    string,
                    System.Collections.Generic.Dictionary<string, string>
                >>(jsonContent);

                if (data != null)
                {
                    GD.Print($"Nombre d'entrées trouvées dans le JSON : {data.Count}");
                    foreach (var entry in data)
                    {
                        GD.Print($"Traitement de l'entrée : {entry.Key}");
                        string googleSheetUrl = entry.Value["googleSheetUrl"];
                        GD.Print($"URL Google Sheets trouvée : {googleSheetUrl}");

                        var googleSheetsUtility = new GoogleSheetsUtility();
                        await googleSheetsUtility.ImportGoogleSheet(googleSheetUrl);
                        GD.Print($"Importation terminée pour : {entry.Key}");
                    }
                }
                else
                {
                    GD.PrintErr("Erreur : Le JSON n'a pas pu être désérialisé correctement");
                }
            }
        }
        else
        {
            GD.PrintErr($"Erreur : Le fichier {filePath} n'existe pas");
        }
    }
}
