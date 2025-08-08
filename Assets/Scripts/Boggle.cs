using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Boggle
{
    public int Score;
    public readonly char[,] Board;
    public readonly int PossibleWords;
    public bool[,] Visited;
    public event Action<int> ScoreChanged;
    
    protected readonly Trie DictionaryTrie = new();
    protected readonly Trie DictionaryFoundTrie = new();
    protected readonly HashSet<string> FoundWords = new();
    protected readonly List<Vector2Int> Path = new();
    
    protected readonly int Rows;
    protected readonly int Cols;
    
    protected bool IsPathStarted;
    

    protected Boggle(int rows, int cols, TextAsset textAsset, char[] letters)
    {
        Visited = new bool[rows, cols];
        Rows = rows;
        Cols = cols;
        Board = CreateBoard(letters);
        LoadDictionary(textAsset);
        FindAllWords();
        PossibleWords = FoundWords.Count;
    }

    private char[,] CreateBoard(char[] letters)
    {
        var board = new char[Rows, Cols];
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Cols; j++)
            {
                board[i, j] = letters[i * Cols + j];
            }
        }
        return board;
    }
    
    private void LoadDictionary(TextAsset textAsset)
    {
        var words = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
            DictionaryTrie.Insert(word);
    }
    
    protected void FindAllWords() 
    {
        FoundWords.Clear();
        Visited = new bool[Rows, Cols];

        for (var i = 0; i < Rows; i++) {
            for (var j = 0; j < Cols; j++) {
                DFS(DictionaryTrie.Root, "", i, j);
            }
        }
    }
    
    private void DFS(TrieNode trieNode, string word, int x, int y) 
    {
        // print($"dfs({_board[x, y]}) word: {word}");
        if (Visited[x, y]) return;
        
        var letter = Board[x, y];
        Visited[x, y] = true;   

        if (trieNode.Children.TryGetValue(letter, out var child))
        {
            word += letter;

            if (child.IsWord)
                FoundWords.Add(word);

            foreach (var n in GetNeighbors(x, y))
            {
                DFS(child, word, n.x, n.y);
            }
        }
        Visited[x, y] = false;   

    }
    
    protected void SetScore(int score)
    {
        Score = score;
        ScoreChanged?.Invoke(Score);
    }
    
    public List<Vector2Int> GetNeighbors(int x, int y)
    {
        List<Vector2Int> neighbors = new();
    
        for (var dx = -1; dx <= 1; dx++) {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // skip self
            
                int newX = x + dx;
                int newY = y + dy;
            
                if (newX >= 0 && newX < Rows && newY >= 0 && newY < Cols) {
                    neighbors.Add(new Vector2Int(newX, newY));
                }
            }
        }
        return neighbors;
    }
    
    public bool IsNeighbor(Vector2Int source, Vector2Int neighbor)
    {
        var d = source - neighbor;
        return Mathf.Abs(d.x) <= 1 && Mathf.Abs(d.y) <= 1 && d is not { x: 0, y: 0 };
    }

    public void ResetVisited()
    {
        Visited = new bool[Rows, Cols];
    }

    public abstract bool NotifyPathStart(Vector2Int position, Action<bool, string> onStart = null);
    public abstract bool RegisterForPath(Vector2Int position, Action<bool, string> onRegister = null);
    public abstract (bool, string) NotifyPathComplete(Action<bool, string> onComplete = null);

    protected void AddToFoundWord(string word)
    {
        DictionaryFoundTrie.Insert(word);
    }

    protected bool JudgeWord(string word, Action<bool, string> onJudge = null)
    {
        var validLength = word.Length >= 3;
        if (!validLength)
        {
            onJudge?.Invoke(false, "word too short");
            return false;
        }
        var validWord = DictionaryTrie.ContainsWord(word);
        if(!validWord)
        {
            onJudge?.Invoke(false, "invalid word");
            return false;
        }
        var newlyFound = !DictionaryFoundTrie.ContainsWord(word);
        if (!newlyFound)
        {
            onJudge?.Invoke(false, "word already found");
            return false;
        }
        
        return true;
    }
}