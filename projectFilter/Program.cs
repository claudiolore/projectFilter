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

            string reactPath = @"C:\Users\claud\OneDrive\Desktop\pizzeriaWebApp-React";
            string dotnetPath = @"C:\Users\claud\source\repos\pizzeriaWebApp\pizzeriaWebApp";
            List<string> sourcePaths = new List<string>();

            string[] reactFolders = { "src" };
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
                                MergeFolder(folderPath, mergedWriter, logWriter);
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
                                MergeFile(filePath, mergedWriter, logWriter);
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
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void MergeFolder(string sourceDir, StreamWriter mergedWriter, StreamWriter logWriter)
    {
        try
        {
            foreach (string filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDir, filePath);
                logWriter.WriteLine($"    File: {relativePath}");
                MergeFile(filePath, mergedWriter, logWriter);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading files from {sourceDir}: {ex.Message}");
        }
    }

    static void MergeFile(string sourceFile, StreamWriter mergedWriter, StreamWriter logWriter)
    {
        try
        {
            mergedWriter.WriteLine($"// File: {Path.GetFileName(sourceFile)}");
            mergedWriter.WriteLine($"// Path: {sourceFile}");
            mergedWriter.WriteLine();

            using (StreamReader reader = new StreamReader(sourceFile))
            {
                string content = reader.ReadToEnd();
                mergedWriter.WriteLine(content);
            }

            mergedWriter.WriteLine();
            mergedWriter.WriteLine("// END OF FILE");
            mergedWriter.WriteLine();

            logWriter.WriteLine($"        File content from {Path.GetFileName(sourceFile)} has been merged into AllCode.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file {sourceFile}: {ex.Message}");
        }
    }
}