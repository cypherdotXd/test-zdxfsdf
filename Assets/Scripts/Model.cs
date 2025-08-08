using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int bugCount;
    public int wordCount;
    public int timeSec;
    public int totalScore;
    public GridSize gridSize;
    public List<TileData> gridData;
}

[System.Serializable]
public class GridSize
{
    public int x;
    public int y;
}

[System.Serializable]
public class TileData
{
    public int tileType;  // 0 = normal, 1 = blocked, 2 = bonus
    public string letter;
}
