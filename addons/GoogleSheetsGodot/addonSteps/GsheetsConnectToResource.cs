using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class GsheetsConnectToResource : Node
{
    private string filePath = "res://Resources/sheets_connection.json";
    JsonUtility jsonUtility = new JsonUtility();

    private async Task EnsureFileExists(string path)
    {
        int attempts = 0;
        while (!Godot.FileAccess.FileExists(path) && attempts < 5)
        {
            await Task.Delay(100);
            attempts++;
        }
        if (!Godot.FileAccess.FileExists(path))
        {
            GD.PrintErr(
                $"Erreur : Le fichier {path} n'existe toujours pas après {attempts} tentatives"
            );
        }
    }

    private void CreateInitialJsonFile()
    {
        // Créer le répertoire s'il n'existe pas
        string dirPath = "res://Resources";
        if (!Godot.DirAccess.DirExistsAbsolute(dirPath))
        {
            Godot.DirAccess.MakeDirAbsolute(dirPath);
        }

        // Créer le fichier avec un dictionnaire vide
        var initialData = new System.Collections.Generic.Dictionary<
            string,
            System.Collections.Generic.Dictionary<string, string>
        >();
        string initialJson = JsonSerializer.Serialize(
            initialData,
            new JsonSerializerOptions { WriteIndented = true }
        );

        jsonUtility.StoreJson(initialJson);
    }

    public async Task SaveToJson(string googleSheetUrl, string resourceScriptUID)
    {
        System.Collections.Generic.Dictionary<
            string,
            System.Collections.Generic.Dictionary<string, string>
        > existingData = new System.Collections.Generic.Dictionary<
            string,
            System.Collections.Generic.Dictionary<string, string>
        >();

        // Créer le fichier initial s'il n'existe pas
        if (!Godot.FileAccess.FileExists(filePath))
        {
            CreateInitialJsonFile();
        }

        GoogleSheetsUtility googleSheetsUtility = new GoogleSheetsUtility();
        string documentId = googleSheetsUtility.ExtractDocumentId(googleSheetUrl);
        string sheetTitle = await googleSheetsUtility.GetSheetTitle(documentId);

        existingData = jsonUtility.ReadJson();

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

        jsonUtility.StoreJson(newJsonString);

        // Attendre que le fichier soit créé
        await EnsureFileExists(filePath);
    }
}
