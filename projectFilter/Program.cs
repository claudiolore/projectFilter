using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Which project do you want to filter? (1)-react  (2)-.net  (3)-both");
            string response = Console.ReadLine().Trim();

            string reactPath = @"C:\Users\Claudio\Desktop\progetti\CresmeConsulting";
            string dotnetPath = @"C:\Users\claud\source\repos\pizzeriaWebApp\pizzeriaWebApp";
            List<string> sourcePaths = new List<string>();

            string[] reactFolders = { "app", "components", "context", "data", "models", "services" };
            string[] dotnetFolders = { "Controllers", "Data", "DTOs", "Models", "Profiles", "Services" };
            string[] dotnetFiles = { "Program.cs", "appsettings.json", "appsettings.Development.json", "appsettings.Production.json", "Dockerfile" };

            switch (response)
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
                    Console.WriteLine("Invalid choice. Exiting the program.");
                    return;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationPath = Path.Combine(desktopPath, "Progetto_Filtrato");

            Directory.CreateDirectory(destinationPath);

            string mergedFilePath = Path.Combine(destinationPath, "AllCode.txt");
            List<string> filteredFiles = new List<string>();

            using (StreamWriter mergedWriter = new StreamWriter(mergedFilePath))
            {
                using (StreamWriter logWriter = new StreamWriter(Path.Combine(destinationPath, "ProjectStructure.txt")))
                {
                    logWriter.WriteLine("Project structure copied:");
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

                        // Merge folders
                        foreach (string folderName in foldersToCopy)
                        {
                            string folderPath = Path.Combine(sourcePath, folderName);
                            if (Directory.Exists(folderPath))
                            {
                                logWriter.WriteLine($"Folder: {folderName} (from {sourcePath})");
                                MergeFolder(folderPath, mergedWriter, logWriter, filteredFiles);
                                logWriter.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine($"Folder not found: {folderName} in {sourcePath}");
                                logWriter.WriteLine($"Folder not found: {folderName} in {sourcePath}");
                            }
                        }

                        // Merge individual files
                        foreach (string fileName in filesToCopy)
                        {
                            string filePath = Path.Combine(sourcePath, fileName);
                            if (File.Exists(filePath))
                            {
                                logWriter.WriteLine($"File: {fileName} (from {sourcePath})");
                                MergeFile(filePath, mergedWriter, logWriter, filteredFiles);
                            }
                            else
                            {
                                Console.WriteLine($"File not found: {fileName} in {sourcePath}");
                                logWriter.WriteLine($"File not found: {fileName} in {sourcePath}");
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Operation completed! The merged file is created at: {mergedFilePath}");
            Console.WriteLine($"Total files filtered: {filteredFiles.Count}");
            Console.WriteLine("List of filtered files:");
            foreach (string file in filteredFiles)
            {
                Console.WriteLine(file);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void MergeFolder(string sourceDir, StreamWriter mergedWriter, StreamWriter logWriter, List<string> filteredFiles, string indent = "")
    {
        try
        {
            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(dir);
                logWriter.WriteLine($"{indent}- {dirName}");
                MergeFolder(dir, mergedWriter, logWriter, filteredFiles, indent + "    ");
            }

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                logWriter.WriteLine($"{indent}- {fileName}");
                MergeFile(filePath, mergedWriter, logWriter, filteredFiles);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading files from {sourceDir}: {ex.Message}");
        }
    }

    static void MergeFile(string sourceFile, StreamWriter mergedWriter, StreamWriter logWriter, List<string> filteredFiles)
    {
        try
        {
            mergedWriter.WriteLine($"// File: {Path.GetFileName(sourceFile)}");

            using (StreamReader reader = new StreamReader(sourceFile))
            {
                string content = reader.ReadToEnd();
                mergedWriter.WriteLine(content);
            }

            mergedWriter.WriteLine("// END OF FILE");

            logWriter.WriteLine($"        File content from {Path.GetFileName(sourceFile)} has been merged into AllCode.txt");
            filteredFiles.Add($"{Path.GetFileName(sourceFile)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file {sourceFile}: {ex.Message}");
        }
    }
}