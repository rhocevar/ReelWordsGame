using System;
using System.Collections.Generic;
using System.IO;

namespace ReelWords.Utilities;

public static class Utils
{
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
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
    
    //------------------------------------------------------------------------------------------------------------------
    // Fisher-Yates (Knuth) Shuffle Algorithm
    public static void Shuffle<T>(this IList<T> list)  
    {
        Random random = new Random(); 
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = random.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}