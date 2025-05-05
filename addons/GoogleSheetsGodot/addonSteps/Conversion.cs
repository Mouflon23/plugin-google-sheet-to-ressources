using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;

[Tool]
public partial class Conversion : Node
{
    private string filePath = "res://Resources/sheets_connection.json";
    JsonUtility jsonUtility = new();
    FilesDirectoryUtility filesDirectoryUtility = new();
    private Dictionary<string, Dictionary<string, string>> dataRead;

    private (string[], string[]) GetCSV() // Get all CSV in the folder and return all file paths and all file names
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

    // Create the resource from the info contained in CSV
    private void MakeResources(
        Godot.Collections.Array<string[]> headers,
        Godot.Collections.Array<string[]> resources,
        string directoryName
    )
    {
        GD.Print("Making Resources");
        string resourceScriptPath = GetResourceScriptPath(directoryName);
        var resourceScript = GD.Load<Script>(resourceScriptPath);

        foreach (string[] r in resources)
        {
            string resourcePath = $"res://Resources/{directoryName}/{r[0]}.tres";
            GD.Print($"Searching for res://Resources/{directoryName}/{r[0]}.tres");

            if (Godot.FileAccess.FileExists(resourcePath))
            {
                GD.Print("File exists.");
                var res = ResourceLoader.Load($"res://Resources/{directoryName}/{r[0]}.tres");
                GD.Print("Resource Found !");
                res.SetScript(resourceScript);

                foreach (string[] header in headers)
                {
                    int i = 0;
                    foreach (string h in header)
                    {
                        res.Set(h, r[i]);
                        GD.Print($"Variable {header[i]} set to {r[i]}");
                        i++;
                    }
                }
                ResourceSaver.Save(res, resourcePath);
            }
            else
            {
                GD.Print("File does not exist.");
                GD.Print("No Resource Found");
                GD.Print("Creating Resource...");
                var resource = new Resource();
                resource.SetScript(resourcePath);
                int l = 0;
                foreach (string[] header in headers)
                {
                    foreach (string h in header)
                    {
                        resource.Set(h, r[l]);
                        GD.Print($"Variable {h} set to {r[l]}");
                        l++;
                    }
                }
                ResourceSaver.Save(resource, resourcePath);
            }
        }
    }

    public Task Convert()
    {
        (string[] filePaths, string[] fileNames) = GetCSV();

        //erase .csv frmo fileNames
        for (int i = 0; i < fileNames.Length; i++)
        {
            fileNames[i] = fileNames[i].Replace(".csv", "");
        }

        int k = 0;
        foreach (string filePath in filePaths)
        {
            var headers = new Godot.Collections.Array<string[]>();
            var resources = new Godot.Collections.Array<string[]>();

            using (var file = filesDirectoryUtility.ReadFile(filePath))
            {
                string[] headerDatas = file.GetLine().Split(",");
                headers.Add(headerDatas);
                while (!file.EofReached())
                {
                    string[] resourceDatas = file.GetLine().Split(",");
                    resources.Add(resourceDatas);
                }
            }

            filesDirectoryUtility.CreateDirectory(fileNames[k]);
            GD.Print("Current Directory : " + fileNames[k]);
            MakeResources(headers, resources, fileNames[k]);
            k++;
        }

        return Task.CompletedTask;
    }

    private string GetResourceScriptPath(string directoryName)
    {
        dataRead = jsonUtility.ReadJson();
        if (dataRead.TryGetValue(directoryName, out var listAnima))
        {
            if (listAnima.TryGetValue("resourceScriptPath", out var path))
            {
                return path;
            }
            else
            {
                GD.PrintErr("Key 'resourceScriptPath' not found.");
                return null;
            }
        }
        else
        {
            GD.PrintErr("Key 'List_Anima' not found.");
            return null;
        }
    }
}
