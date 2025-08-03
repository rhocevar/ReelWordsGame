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
    // Shuffles a queue by rotating its elements X random times
    public static void Shuffle<T>(this Queue<T> queue)
    {
        Random random = new Random();
        int n = queue.Count;
        int rotations = random.Next(n + 1); // Exclusive upper bound

        for (int i = 0; i < rotations; i++)
        {
            T item = queue.Dequeue();
            queue.Enqueue(item);
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    public static string ToSubscript(this int number)
    {
        string subscriptNumber = "";
        
        string numberStr = number.ToString();
        foreach (char c in numberStr)
        {
            switch (c)
            {
                case '0': subscriptNumber += '₀'; break;
                case '1': subscriptNumber += '₁'; break;
                case '2': subscriptNumber += '₂'; break;
                case '3': subscriptNumber += '₃'; break;
                case '4': subscriptNumber += '₄'; break;
                case '5': subscriptNumber += '₅'; break;
                case '6': subscriptNumber += '₆'; break;
                case '7': subscriptNumber += '₇'; break;
                case '8': subscriptNumber += '₈'; break;
                case '9': subscriptNumber += '₉'; break;
            }
        }

        return subscriptNumber;
    }
    
}
