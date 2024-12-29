using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Inserisci il percorso della cartella del progetto C#: ");
            string sourcePath = Console.ReadLine().Trim();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationPath = Path.Combine(desktopPath, "Progetto_Filtrato");

            Directory.CreateDirectory(destinationPath);

            string[] foldersToCopy = { "Controllers", "Data", "DTOs", "Models", "Profiles", "Services" };

            foreach (string folderName in foldersToCopy)
            {
                string folderPath = Path.Combine(sourcePath, folderName);
                if (Directory.Exists(folderPath))
                {
                    CopyFilesToSingleFolder(folderPath, destinationPath);
                }
                else
                {
                    Console.WriteLine($"Cartella non trovata: {folderName}");
                }
            }

            Console.WriteLine($"Operazione completata! I file sono stati copiati in: {destinationPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Si è verificato un errore: {ex.Message}");
        }
    }

    static void CopyFilesToSingleFolder(string sourceDir, string destDir)
    {
        try
        {
            foreach (string filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
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
