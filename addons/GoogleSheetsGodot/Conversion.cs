using System;
using System.IO;
using System.Resources;
using Godot;

[Tool]
public partial class Conversion : Node
{
    private (string[], string[]) GetCSV()
    {
        string csvDirectory = "res://Resources/CSV";
        using var dir = DirAccess.Open(csvDirectory);
        var filePaths = new System.Collections.Generic.List<string>();
        var fileNames = new System.Collections.Generic.List<string>();

        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();

            while (fileName != "")
            {
                if (!dir.CurrentIsDir() && fileName.EndsWith(".csv"))
                {
                    filePaths.Add(Path.Combine(csvDirectory, fileName));
                    fileNames.Add(fileName);
                }
                fileName = dir.GetNext();
            }
            dir.ListDirEnd();
        }
        else
        {
            GD.PrintErr($"Impossible d'ouvrir le répertoire : {csvDirectory}");
        }

        return (filePaths.ToArray(), fileNames.ToArray());
    }

    private void MakeResources(
        Godot.Collections.Array<string[]> headers,
        Godot.Collections.Array<string[]> resources
    )
    {
        foreach (string[] r in resources)
        {
            GD.Print($"Searching for res://Resources/{r[0]}.tres");
            var res = ResourceLoader.Load($"res://Resources/{r[0]}.tres");
            if (res != null)
            {
                GD.Print("Resource Found !");

                // Chara chara = res as Chara;
                int i = 0;
                foreach (string[] header in headers)
                {
                    if (r[i] == "true" || r[i] == "false")
                    {
                        // chara.header = r[i].ToLower();
                    }
                    if (int.TryParse(r[i], out int value))
                    {
                        // chara.header = r[i].ToInt();
                    }
                    if (float.TryParse(r[i], out float floatValue))
                    {
                        // chara.header = r[i].ToFloat();
                    }
                    else
                    {
                        // chara.header = r[i]
                    }
                    i++;
                }
                // ResourceSaver.Save(chara, $"res://Resources/r[0].tres");
            }
            else
            {
                GD.Print("No Resource Found");
                GD.Print("Creating Resource...");
                int j = 0;
                // Chara newChara = new()
                // {
                //     // foreach (string[] header in headers)
                //     // {
                //     // header = r[i];
                //     // }
                // };
                // ResourceSaver.Save(newChara, $"res://Resources/{newChara.CharaName}.tres");
            }
        }
    }

    public void Convert()
    {
        (string[] filePaths, string[] fileNames) = GetCSV();
        foreach (string filePath in filePaths)
        {
            var headers = new Godot.Collections.Array<string[]>();
            var resources = new Godot.Collections.Array<string[]>();
            using Godot.FileAccess file = Godot.FileAccess.Open(
                filePath,
                Godot.FileAccess.ModeFlags.Read
            );
            // --> "using" appelle automatiquement ".Dispose()" à la fin de la fonction
            // --> Permet d'éviter automatiquement les problèmes d'utilisation des fichiers avec "Godot.FileAccess"
            // --> Dont Godot qui empêche de réimporter (ici) le fichier .csv
            string[] headerDatas = file.GetLine().Split(",");
            headers.Add(headerDatas);
            while (!file.EofReached()) // --> Permet de s'arrêter à la dernière ligne du document
            {
                string[] resourceDatas = file.GetLine().Split(",");
                resources.Add(resourceDatas);
            }
            GD.Print(headers);
            GD.Print(resources);
            MakeResources(headers, resources);
        }
    }

    public void OldFunction()
    {
        var characters = new Godot.Collections.Array<string[]>();
        using Godot.FileAccess file = Godot.FileAccess.Open(
            "uid://vunwc72f4tm1",
            Godot.FileAccess.ModeFlags.Read
        );

        // file.GetCsvLine()

        file.GetLine();
        while (!file.EofReached())
        {
            string[] characterDatas = file.GetLine().Split(",");
            characters.Add(characterDatas);
        } // --> Permet de s'arrêter à la dernière ligne du document

        GD.Print(characters);

        foreach (string[] s in characters)
        {
            GD.Print($"res://Resources/{s[0]}.tres");
            var res = ResourceLoader.Load($"res://Resources/{s[0]}.tres");
            GD.Print(res);
            if (res != null)
            {
                GD.Print("Access != null");

                // Chara chara = res as Chara;
                // chara.CharaName = s[0];
                // chara.Health = s[1].ToInt();
                // chara.Damage = s[2].ToInt();
                // ResourceSaver.Save(chara, $"res://Resources/{chara.CharaName}.tres");
            }
            else
            {
                GD.Print("Access new");
                // Chara newChara = new()
                // {
                //     CharaName = s[0],
                //     Health = s[1].ToInt(),
                //     Damage = s[2].ToInt(),
                // };
                // ResourceSaver.Save(newChara, $"res://Resources/{newChara.CharaName}.tres");
            }
        }

        file.Close();
    }
}
