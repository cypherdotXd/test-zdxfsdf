using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Trie
{
    public readonly TrieNode Root = new();

    public void Insert(string word)
    {
        var node = Root;
        foreach (var letter in word)
        {
            if(!node.Children.ContainsKey(letter))
                node.Children[letter] = new TrieNode();
            node = node.Children[letter];
        }
        node.IsWord = true;
    }

    public bool ContainsWord(string word)
    {
        var node = Root;
        foreach (var letter in word)
        {
            if(!node.Children.TryGetValue(letter, out var child))
                return false;
            node = child;
        }
        return node.IsWord;
    }

    public bool ContainsSubstring(string word)
    {
        var node = Root;
        foreach (var letter in word)
        {
            if(!node.Children.TryGetValue(letter, out var child))
                return false;
            node = child;
        }
        return true;
    }

}

public class TrieNode
{
    public Dictionary<char, TrieNode> Children = new();
    public bool IsWord;

    public string GetTrieString()
    {
        return Children.Keys.Aggregate("", (current, c) => current + c);
    }
}
