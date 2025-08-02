using System.IO;

namespace ReelWords.Utilities;

public static class Utils
{
    public static DirectoryInfo TryGetDirectoryInfo(string folderName)
    {
        // Get the current directory where the assembly is being executed
        string currentDirectoryPath = Directory.GetCurrentDirectory();
        DirectoryInfo currentDirectory = new DirectoryInfo(currentDirectoryPath);
        
        // Traverse up the directory tree until the folder is found
        while (currentDirectory != null)
        {
            DirectoryInfo[] targetDirectory = currentDirectory.GetDirectories(folderName);
            if (targetDirectory.Length > 0)
            {
                return targetDirectory[0];
            }
            
            currentDirectory = currentDirectory.Parent;
        }

        return null; // No directory was found
    }
}