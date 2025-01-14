using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Quale progetto vuoi filtrare? (1)-react  (2)-.net  (3)-entrambi");
            string risposta = Console.ReadLine().Trim();

            string reactPath = "C:/Users/claud/OneDrive/Desktop/pizzeriaWebApp-React";
            string dotnetPath = "C:/Users/claud/source/repos/pizzeriaWebApp/pizzeriaWebApp";
            List<string> sourcePaths = new List<string>();

            string[] reactFolders = { "src" };
            string[] dotnetFolders = { "Controllers", "Data", "DTOs", "Models", "Profiles", "Services" };
            string[] dotnetFiles = { "Program.cs", "appsettings.json", "appsettings.Development.json", "appsettings.Production.json", "Dockerfile" };

            switch (risposta)
            {
                case "1":
                    sourcePaths.Add(reactPath);
                    break;
                case "2":
                    sourcePaths.Add(dotnetPath);
                    break;
                case "3":
                    sourcePaths.Add(reactPath);
                    sourcePaths.Add(dotnetPath);
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
                    string[] foldersToCopy;
                    string[] filesToCopy;

                    if (sourcePath == reactPath)
                    {
                        foldersToCopy = reactFolders;
                        filesToCopy = new string[] { };
                    }
                    else if (sourcePath == dotnetPath)
                    {
                        foldersToCopy = dotnetFolders;
                        filesToCopy = dotnetFiles;
                    }
                    else
                    {
                        foldersToCopy = new string[] { };
                        filesToCopy = new string[] { };
                    }

                    // Copy folders
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

                    // Copy individual files
                    foreach (string fileName in filesToCopy)
                    {
                        string filePath = Path.Combine(sourcePath, fileName);
                        if (File.Exists(filePath))
                        {
                            logWriter.WriteLine($"File: {fileName} (da {sourcePath})");
                            CopyFileToDestination(filePath, destinationPath, logWriter);
                        }
                        else
                        {
                            Console.WriteLine($"File non trovato: {fileName} in {sourcePath}");
                            logWriter.WriteLine($"File non trovato: {fileName} in {sourcePath}");
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

    static void CopyFileToDestination(string sourceFile, string destDir, StreamWriter logWriter)
    {
        try
        {
            string fileName = Path.GetFileName(sourceFile);
            string destFilePath = Path.Combine(destDir, fileName);

            if (File.Exists(destFilePath))
            {
                string uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
                destFilePath = Path.Combine(destDir, uniqueFileName);
            }

            File.Copy(sourceFile, destFilePath, overwrite: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la copia del file {sourceFile}: {ex.Message}");
        }
    }
}