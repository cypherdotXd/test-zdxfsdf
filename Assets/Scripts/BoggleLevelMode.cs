using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoggleLevelMode : Boggle
{
    public enum TileType
    {
        Default,
        Bug,
        Lock,
    }
    public readonly TileType[,] TilesType;
    public event Action<Vector2Int> LockUnlocked;
    public event Action<Vector2Int> BugFound;
    
    public BoggleLevelMode(int rows, int cols, TextAsset textAsset, char[] letters, int bugsCount, int locksCount) : base(rows, cols, textAsset, letters)
    {
        TilesType = new TileType[rows, cols];
        AddTypes(bugsCount, locksCount);
    }

    private void AddTypes(int numBugs, int numLocks)
    {
        var rIndices = Enumerable.Range(0, Rows).ToList();
        var cIndices = Enumerable.Range(0, Cols).ToList();
        for (int i = 0; i < numBugs; i++)
        {
            var randRowI = Random.Range(0, rIndices.Count);
            var randColI = Random.Range(0, cIndices.Count);
            
            var rI = rIndices[randRowI];
            var rC = cIndices[randColI];
            TilesType[rI, rC] = TileType.Bug;
            
            rIndices.RemoveAt(randRowI);
            cIndices.RemoveAt(randColI);
        }
        for (int i = 0; i < numLocks; i++)
        {
            var randRowI = Random.Range(0, rIndices.Count);
            var randColI = Random.Range(0, cIndices.Count);
            
            var rI = rIndices[randRowI];
            var rC = cIndices[randColI];
            TilesType[rI, rC] = TileType.Lock;
            
            rIndices.RemoveAt(randRowI);
            cIndices.RemoveAt(randColI);
        }
    }
    
    public override bool NotifyPathStart(Vector2Int position, Action<bool, string> onStart = null)
    {
        if (TilesType[position.x, position.y] == TileType.Lock)
        {
            onStart?.Invoke(false, "Letter is Locked");
            return false;
        }
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
        if (!IsNeighbor(last, current) || Visited[current.x, current.y] || TilesType[current.x, current.y] == TileType.Lock)
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
            var bugsFound = 0;
            foreach (var p in Path.Where(p => TilesType[p.x, p.y] == TileType.Bug))
            {
                bugsFound++;
                BugFound?.Invoke(p);
                TilesType[p.x, p.y] = TileType.Default;
            }
            AddToFoundWord(word);
            SetScore(Score + word.Length + bugsFound);
            Path.ForEach(TryUnlockNeighbors);
        }
        Path.Clear();
        ResetVisited();
        return (valid, word);
    }

    private void TryUnlockNeighbors(Vector2Int source)
    {
        var neighbors = GetNeighbors(source.x, source.y);
        foreach (var n in neighbors.Where(n => TilesType[n.x, n.y] == TileType.Lock))
        {
            TilesType[n.x, n.y] = TileType.Default;
            LockUnlocked?.Invoke(n);
        }
    }
}
