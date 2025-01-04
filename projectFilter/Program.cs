using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Quale progetto vuoi filtrare? (1)-react  (2)-.net  (3)-entrambi");
            string risposta = Console.ReadLine().Trim();

            string reactPath = "C:/Users/claud/OneDrive/Desktop/corso-react";
            string dotnetPath = "C:/Users/claud/source/repos/pizzeriaWebApp/pizzeriaWebApp";
            string[] sourcePaths;

            string[] reactFolders = { "src" };
            string[] dotnetFolders = { "Controllers", "Data", "DTOs", "Models", "Profiles", "Services" };

            switch (risposta)
            {
                case "1":
                    sourcePaths = new[] { reactPath };
                    break;
                case "2":
                    sourcePaths = new[] { dotnetPath };
                    break;
                case "3":
                    sourcePaths = new[] { reactPath, dotnetPath };
                    break;
                default:
                    Console.WriteLine("Scelta non valida. Uscita dal programma.");
                    return;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationPath = Path.Combine(desktopPath, "Progetto_Filtrato");

            Directory.CreateDirectory(destinationPath);

            using (StreamWriter logWriter = new StreamWriter(Path.Combine(destinationPath, "StrutturaProgetto.txt")))
            {
                logWriter.WriteLine("Struttura del progetto copiata:");
                logWriter.WriteLine();

                foreach (string sourcePath in sourcePaths)
                {
                    string[] foldersToCopy = sourcePath == reactPath ? reactFolders : dotnetFolders;

                    foreach (string folderName in foldersToCopy)
                    {
                        string folderPath = Path.Combine(sourcePath, folderName);
                        if (Directory.Exists(folderPath))
                        {
                            logWriter.WriteLine($"Cartella: {folderName} (da {sourcePath})");
                            CopyFilesToSingleFolder(folderPath, destinationPath, logWriter);
                            logWriter.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine($"Cartella non trovata: {folderName} in {sourcePath}");
                            logWriter.WriteLine($"Cartella non trovata: {folderName} in {sourcePath}");
                        }
                    }
                }
            }

            Console.WriteLine($"Operazione completata! I file sono stati copiati in: {destinationPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Si è verificato un errore: {ex.Message}");
        }
    }

    static void CopyFilesToSingleFolder(string sourceDir, string destDir, StreamWriter logWriter)
    {
        try
        {
            foreach (string filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDir, filePath);
                logWriter.WriteLine($"    File: {relativePath}");

                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destDir, fileName);

                if (File.Exists(destFilePath))
                {
                    string uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
                    destFilePath = Path.Combine(destDir, uniqueFileName);
                }

                File.Copy(filePath, destFilePath, overwrite: true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la copia dei file da {sourceDir}: {ex.Message}");
        }
    }
}
