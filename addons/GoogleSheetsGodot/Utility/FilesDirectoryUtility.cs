using System;
using System.IO;
using Godot;

public partial class FilesDirectoryUtility : Node
{
    public Godot.FileAccess ReadFile(string filePath)
    {
        return Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
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
