using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using Godot;
using Godot.Collections;

public partial class GsheetsConnectToResource : Node
{
    private string filePath = "res://Resources/sheets_connection.json";

    public async void SaveToJson(string googleSheetUrl, string resourceScriptUID)
    {
        GoogleSheetsUtility googleSheetsUtility = new GoogleSheetsUtility();
        string documentId = googleSheetsUtility.ExtractDocumentId(googleSheetUrl);
        string sheetTitle = await googleSheetsUtility.GetSheetTitle(documentId);

        System.Collections.Generic.Dictionary<
            string,
            System.Collections.Generic.Dictionary<string, string>
        > existingData = new System.Collections.Generic.Dictionary<
            string,
            System.Collections.Generic.Dictionary<string, string>
        >();

        // Lire le fichier existant s'il existe
        if (Godot.FileAccess.FileExists(filePath))
        {
            using (var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read))
            {
                string existingJson = file.GetAsText();
                existingData =
                    JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<
                        string,
                        System.Collections.Generic.Dictionary<string, string>
                    >>(existingJson)
                    ?? new System.Collections.Generic.Dictionary<
                        string,
                        System.Collections.Generic.Dictionary<string, string>
                    >();
            }
        }

        // Ajouter ou mettre à jour la nouvelle entrée
        existingData[sheetTitle] = new System.Collections.Generic.Dictionary<string, string>
        {
            { "googleSheetUrl", googleSheetUrl },
            { "resourceScriptPath", resourceScriptUID },
        };

        // Sauvegarder le fichier mis à jour
        string newJsonString = JsonSerializer.Serialize(
            existingData,
            new JsonSerializerOptions { WriteIndented = true }
        );

        using (var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write))
        {
            file.StoreString(newJsonString);
        }
    }
}
