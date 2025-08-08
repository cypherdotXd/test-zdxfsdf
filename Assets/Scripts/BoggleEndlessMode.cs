using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoggleEndlessMode : Boggle
{
    public event Action<Vector2Int, char> LetterChanged;
    
    private List<string> _words;
    
    public BoggleEndlessMode(int rows, int cols, TextAsset textAsset, char[] letters) : base(rows, cols, textAsset, letters)
    {
        _words = textAsset.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
    
    public override bool NotifyPathStart(Vector2Int position, Action<bool, string> onStart = null)
    {
        IsPathStarted = true;
        var current = position;
        Visited[current.x, current.y] = true;
        Path.Add(current);
        return true;
    }

    public override bool RegisterForPath(Vector2Int position, Action<bool, string> onRegister = null)
    {
        if (!IsPathStarted) return false;
        
        var current = position;
        
        var last = Path[^1];
        if (!IsNeighbor(last, current) || Visited[current.x, current.y])
        {
            onRegister?.Invoke(false, "Letter not usable");
            IsPathStarted = false;
            Path.Clear();
            ResetVisited();
            return false;
        }
        onRegister?.Invoke(true, "valid");
        Visited[current.x, current.y] = true;
        Path.Add(current);
        return true;
    }
    
    public override (bool, string) NotifyPathComplete(Action<bool, string> onComplete = null)
    {
        if(!IsPathStarted) return (false, null);
        IsPathStarted = false;
        var word = Path.Select(p => Board[p.x, p.y]).Aggregate("", (acc, next) => acc + next);
        var valid = JudgeWord(word, onComplete);
        if (valid)
        {
            AddToFoundWord(word);
            ReplaceWithRandomLetters(Path);
            SetScore(Score + word.Length);
        }
        Path.Clear();
        ResetVisited();
        return (valid, word);
    }

    private void ReplaceWithRandomLetters(List<Vector2Int> path)
    {
        var randomString = _words.Skip(Random.Range(0, _words.Count - 5)).Take(path.Count).Aggregate((acc, next) => acc + next);
        foreach (var p in path)
        {
            var randLetter = randomString[Random.Range(0, randomString.Length)];
            Board[p.x, p.y] = randLetter;
            LetterChanged?.Invoke(p, randLetter);
        }

        FindAllWords();
        Debug.Log($"Found {path.Count} words");
    }

}
