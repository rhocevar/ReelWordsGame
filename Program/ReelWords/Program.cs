using System;

namespace ReelWords
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Trie englishWords = new Trie();
            
            // bool playing = true;

            // while (playing)
            // {
                //string input = Console.ReadLine();
                englishWords.Insert("can");
                englishWords.Insert("car");
                englishWords.Insert("cats");
                englishWords.Insert("cat");
                englishWords.Insert("cart");
                
                Console.WriteLine("Search 'can': " + englishWords.Search("can"));
                Console.WriteLine("Search 'car': " + englishWords.Search("car"));
                Console.WriteLine("Search 'cats': " + englishWords.Search("cats"));
                Console.WriteLine("Search 'cat': " + englishWords.Search("cat"));
                Console.WriteLine("Search 'cart': " + englishWords.Search("cart"));
                
                englishWords.Delete("can");
                englishWords.Delete("car");
                englishWords.Delete("cats");
                englishWords.Delete("cat");
                englishWords.Delete("cart");
                
                Console.WriteLine("Search 'can': " + englishWords.Search("can"));
                Console.WriteLine("Search 'car': " + englishWords.Search("car"));
                Console.WriteLine("Search 'cats': " + englishWords.Search("cats"));
                Console.WriteLine("Search 'cat': " + englishWords.Search("cat"));
                Console.WriteLine("Search 'cart': " + englishWords.Search("cart"));

                // TODO:  Run game logic here using the user input string

                // TODO:  Create simple unit tests to test your code in the ReelWordsTests project,
                // don't worry about creating tests for everything, just important functions as
                // seen for the Trie tests
            // }
        }
    }
}