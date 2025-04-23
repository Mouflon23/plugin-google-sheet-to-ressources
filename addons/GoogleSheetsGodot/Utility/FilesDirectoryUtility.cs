using Godot;
using System;
using System.IO;

public partial class FilesDirectoryUtility : Node
{
    
    public Godot.FileAccess ReadFile(string filePath) {
        using Godot.FileAccess file = Godot.FileAccess.Open(
                filePath,
                Godot.FileAccess.ModeFlags.Read);
        return file;
    }

    public void CreateDirectory(string directoryName)
    {
        string directoryPath = Path.Combine("res://Resources", directoryName);

        if (!DirAccess.DirExistsAbsolute(directoryPath))
        {
            DirAccess.MakeDirRecursiveAbsolute(directoryPath);
            GD.Print($"Le répertoire {directoryPath} a été créé avec succès.");
        }
        else
        {
            GD.Print($"Le répertoire {directoryPath} existe déjà.");
        }
    }
    
}
