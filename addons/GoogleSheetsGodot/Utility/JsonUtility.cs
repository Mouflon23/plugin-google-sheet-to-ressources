using System;
using System.Text.Json;
using Godot;

public partial class JsonUtility : Node
{
    private string filePath = "res://Resources/sheets_connection.json";

    public System.Collections.Generic.Dictionary<
        string,
        System.Collections.Generic.Dictionary<string, string>
    > ReadJson()
    {
        System.Collections.Generic.Dictionary<
            string,
            System.Collections.Generic.Dictionary<string, string>
        > existingData = new System.Collections.Generic.Dictionary<
            string,
            System.Collections.Generic.Dictionary<string, string>
        >();

        // Lire le fichier existant
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

        return existingData;
    }

    public void StoreJson(string initialJson)
    {
        using (var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write))
        {
            file.StoreString(initialJson);
        }
    }
}
