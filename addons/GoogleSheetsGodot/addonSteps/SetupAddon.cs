using System;
using Godot;

public partial class SetupAddons : Node
{
    FilesDirectoryUtility filesDirectoryUtility = new();

    public void MakeRequiredDirectory()
    {
        filesDirectoryUtility.CreateDirectory("");
        filesDirectoryUtility.CreateDirectory("CSV");
    }
}
