using System;
using System.IO;
using System.Resources;
using Godot;

[Tool]
public partial class Conversion : Node
{
    private string[] Read()
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

        return filePaths.ToArray();
    }

    private void MakeResources(Godot.Collections.Array<string[]> resources) { }

    public void Convert()
    {
        string[] filePaths = Read();
        foreach (string filePath in filePaths)
        {
            Godot.Collections.Array<string[]> resources = new();
            using Godot.FileAccess file = Godot.FileAccess.Open(
                filePath,
                Godot.FileAccess.ModeFlags.Read
            );
            // --> "using" appelle automatiquement ".Dispose()" à la fin de la fonction
            // --> Permet d'éviter automatiquement les problèmes d'utilisation des fichiers avec "Godot.FileAccess"
            // --> Dont Godot qui empêche de réimporter (ici) le fichier .csv
            while (!file.EofReached())
            {
                string[] resourcesDatas = file.GetLine().Split(",");
                resources.Add(resourcesDatas);
                GD.Print(resources);
            }
            // --> Permet de s'arrêter à la dernière ligne du document
        }
    }

    public void OldFunction()
    {
        Godot.Collections.Array<string[]> characters = new();
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

                Chara chara = res as Chara;
                chara.CharaName = s[0];
                chara.Health = s[1].ToInt();
                chara.Damage = s[2].ToInt();
                ResourceSaver.Save(chara, $"res://Resources/{chara.CharaName}.tres");
            }
            else
            {
                GD.Print("Access new");
                Chara newChara = new()
                {
                    CharaName = s[0],
                    Health = s[1].ToInt(),
                    Damage = s[2].ToInt(),
                };
                ResourceSaver.Save(newChara, $"res://Resources/{newChara.CharaName}.tres");
            }
        }

        file.Close();
    }
}
