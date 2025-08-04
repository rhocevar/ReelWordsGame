using System.Collections.Generic;

namespace ReelWords.Data;

/// <summary>
/// Trie data structure that allows for fast indexing of words that can be quickly searched.
/// Implements 3 operations: Insert, Search and Delete.
/// In the ReelWords game, it is used for verifying if a word exists in a given dictionary. 
/// </summary>
public class Trie
{
    //------------------------------------------------------------------------------------------------------------------
    // Types
    //------------------------------------------------------------------------------------------------------------------
    private class Node
    {
        //--------------------------------------------------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------------------------------------------------
        public Dictionary<char, Node> Children { get; }
            
        //--------------------------------------------------------------------------------------------------------------
        // Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly char m_character;

        //--------------------------------------------------------------------------------------------------------------
        // Methods
        //--------------------------------------------------------------------------------------------------------------
        public Node(char c)
        {
            m_character = c;
            Children = new Dictionary<char, Node>();
        }
            
        //--------------------------------------------------------------------------------------------------------------
        public void AddChild(Node newChild)
        {
            Children.Add(newChild.m_character, newChild);
        }
    }
       
    //------------------------------------------------------------------------------------------------------------------
    // Properties
    //------------------------------------------------------------------------------------------------------------------
    public int Count => m_numberOfNodes;
    
    //------------------------------------------------------------------------------------------------------------------
    // Constants
    //------------------------------------------------------------------------------------------------------------------
    private const char c_rootChar = '^';  // Root node symbol
    private const char c_endOfWordChar = '='; // End of word symbol
        
    //------------------------------------------------------------------------------------------------------------------
    // Variables
    //------------------------------------------------------------------------------------------------------------------
    private readonly Node m_root = new (c_rootChar);
    private readonly Node m_endOfWord = new (c_endOfWordChar); // Only one end of word node (can be referenced multiple times)
    private int m_numberOfNodes = 1; // Only considering the root
        
    //------------------------------------------------------------------------------------------------------------------
    // Methods
    //------------------------------------------------------------------------------------------------------------------
    public void Insert(string word)
    {
        Node currentNode = m_root;
        foreach (char c in word)
        {
            if (currentNode.Children.TryGetValue(c, out Node child))
            {
                currentNode = child;
            }
            else
            {
                Node newChild = new Node(c);
                currentNode.AddChild(newChild);
                currentNode = newChild;

                ++m_numberOfNodes;
            }
        }
            
        currentNode.AddChild(m_endOfWord);
    }
        
    //------------------------------------------------------------------------------------------------------------------
    public bool Search(string word)
    {
        Node currentNode = m_root;
        foreach (char c in word)
        {
            if (currentNode.Children.TryGetValue(c, out Node child))
            {
                currentNode = child;
            }
            else
            {
                return false;
            }
        }

        if (currentNode.Children.ContainsKey(c_endOfWordChar))
        {
            return true;
        }
            
        return false;
    }
        
    //------------------------------------------------------------------------------------------------------------------
    public void Delete(string word)
    {
        DeleteRecursive(word, m_root, index:0);
    }
        
    //------------------------------------------------------------------------------------------------------------------
    private bool DeleteRecursive(string word, Node node, int index)
    {
        // If we reached the end of the word
        if (index == word.Length)
        {
            // Delete the end of word node if there is one
            node.Children.Remove(c_endOfWordChar);

            // Only delete the current node if it has no other children
            if (node.Children.Count == 0 && node != m_root)
            {
                return true;
            }
                
            return false;
        }

        char childKey = word[index];
        if (node.Children.TryGetValue(childKey, out Node child))
        {
            bool shouldDeleteWord = DeleteRecursive(word, child, ++index);
            if (shouldDeleteWord)
            {
                node.Children.Remove(childKey);
                --m_numberOfNodes;

                // Delete the current node if it has no other children
                if (node.Children.Count == 0 && node != m_root)
                {
                    return true;
                }
            }
        }
            
        return false;
    }
}