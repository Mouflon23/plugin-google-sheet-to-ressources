using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Godot;

public partial class GoogleSheetsUtility : Node
{
    public async Task<(string sheetUrl, string sheetTitle)> ImportGoogleSheet(string sheetUrl)
    {
        try
        {
            GD.Print("Début de l'importation...");
            GD.Print($"URL reçue: {sheetUrl}");

            string documentId = ExtractDocumentId(sheetUrl);
            string sheetTitle = await GetSheetTitle(documentId);
            GD.Print($"Titre du document: {sheetTitle}");

            string csvUrl = ConvertToCsvUrl(sheetUrl);
            GD.Print($"URL CSV générée: {csvUrl}");

            string csvContent = await DownloadCsv(csvUrl);
            GD.Print("Contenu CSV téléchargé avec succès");

            // Créer le dossier s'il n'existe pas
            string folderPath = "res://Resources/CSV";
            if (!DirAccess.DirExistsAbsolute(folderPath))
            {
                GD.Print($"Création du dossier: {folderPath}");
                DirAccess.MakeDirRecursiveAbsolute(folderPath);
            }

            // Utiliser le titre du document comme nom de fichier
            string fileName = $"{sheetTitle}.csv";
            string filePath = Path.Combine(folderPath, fileName);
            GD.Print($"Chemin du fichier: {filePath}");

            // Sauvegarder le fichier
            using (
                Godot.FileAccess file = Godot.FileAccess.Open(
                    filePath,
                    Godot.FileAccess.ModeFlags.Write
                )
            )
            {
                if (file == null)
                {
                    throw new Exception(
                        $"Impossible d'ouvrir le fichier: {Godot.FileAccess.GetOpenError()}"
                    );
                }
                file.StoreString(csvContent);
                GD.Print($"Fichier sauvegardé avec succès: {filePath}");
            }

            return (sheetUrl, sheetTitle);
        }
        catch (Exception e)
        {
            GD.PrintErr($"Erreur détaillée lors de l'importation du Google Sheet: {e.Message}");
            GD.PrintErr($"Stack trace: {e.StackTrace}");
            throw;
        }
    }

    private string ConvertToCsvUrl(string sheetUrl)
    {
        try
        {
            string documentId = ExtractDocumentId(sheetUrl);
            GD.Print($"ID du document extrait: {documentId}");
            return $"https://docs.google.com/spreadsheets/d/{documentId}/export?format=csv";
        }
        catch (Exception e)
        {
            GD.PrintErr($"Erreur lors de la conversion de l'URL: {e.Message}");
            throw;
        }
    }

    public string ExtractDocumentId(string sheetUrl)
    {
        if (!string.IsNullOrEmpty(sheetUrl))
        {
            try
            {
                // Nettoyer l'URL en supprimant les paramètres après ?
                string cleanUrl = sheetUrl.Split('?')[0];

                // Extraire l'ID du document
                string[] parts = cleanUrl.Split('/');
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    if (parts[i] == "d")
                    {
                        string documentId = parts[i + 1];
                        return documentId;
                    }
                }

                throw new Exception("Impossible de trouver l'ID du document dans l'URL");
            }
            catch (Exception e)
            {
                GD.PrintErr($"Erreur lors de l'extraction de l'ID: {e.Message}");
                throw;
            }
        }
        return null;
    }

    private async Task<string> DownloadCsv(string csvUrl)
    {
        try
        {
            GD.Print($"Tentative de téléchargement depuis: {csvUrl}");
            HttpResponseMessage response = await httpClient.GetAsync(csvUrl);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
        catch (Exception e)
        {
            GD.PrintErr($"Erreur lors du téléchargement: {e.Message}");
            throw;
        }
    }

    private System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

    public async Task<string> GetSheetTitle(string documentId)
    {
        try
        {
            string url =
                $"https://docs.google.com/spreadsheets/d/{documentId}/export?format=csv&gid=0";
            using (var request = new HttpRequestMessage(HttpMethod.Head, url))
            {
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Récupérer le nom du fichier depuis les en-têtes Content-Disposition
                if (response.Content.Headers.TryGetValues("Content-Disposition", out var values))
                {
                    string disposition = values.First();
                    int start = disposition.IndexOf("filename=\"") + 10;
                    int end = disposition.IndexOf("\"", start);
                    if (start > 9 && end > start)
                    {
                        string filename = disposition.Substring(start, end - start);
                        // Retirer l'extension .csv et -Feuille1
                        return filename.Replace(".csv", "").Replace("-Feuille1", "");
                    }
                }
            }
            return documentId; // Fallback si le titre n'est pas trouvé
        }
        catch (Exception e)
        {
            GD.PrintErr($"Erreur lors de la récupération du titre: {e.Message}");
            return documentId; // Fallback si une erreur survient
        }
    }
}
