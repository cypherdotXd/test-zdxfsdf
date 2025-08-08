using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class BoggleManager : MonoBehaviour
{
    public static BoggleManager Instance { get; private set; }
    
    public enum BoggleMode
    {
        Endless,
        Level,
    }
    [SerializeField] private TextAsset levelData;
    [SerializeField] private TextAsset dictionaryFile;
    [SerializeField] private LetterTile letterTilePrefab;
    [SerializeField] private GridLayoutGroup boardGridLayout;
    [SerializeField] private Transform boardRoot;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private BoggleMode boggleMode;

    private Boggle _boggle;
    private readonly Dictionary<Vector2Int, LetterTile> _letterTiles = new();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        var data = JsonUtility.FromJson<LevelData>(levelData.text);
        var letters = data.gridData.Select(d => d.letter.ToLower()[0]).ToArray();
        if (boggleMode == BoggleMode.Level)
        {
            _boggle = new BoggleLevelMode(gridSize.x, gridSize.y, dictionaryFile, letters, 1, 1);
        }
        else if (boggleMode == BoggleMode.Endless)
        {
            _boggle = new BoggleEndlessMode(gridSize.x, gridSize.y, dictionaryFile, letters);
        }
        CreateBoard(_boggle.Board);
    }

    private void OnEnable()
    {
        _boggle.ScoreChanged += LevelManager.NotifyScoreChanged;
        switch (boggleMode)
        {
            case BoggleMode.Level:
                ((BoggleLevelMode)_boggle).LockUnlocked += UnlockTile;
                ((BoggleLevelMode)_boggle).BugFound += OnBugFound;
                break;
            case BoggleMode.Endless:
                ((BoggleEndlessMode)_boggle).LetterChanged += UpdateTile;
                break;
        }
    }

    private void OnDisable()
    {
        _boggle.ScoreChanged -= LevelManager.NotifyScoreChanged;
        switch (boggleMode)
        {
            case BoggleMode.Level:
                ((BoggleLevelMode)_boggle).LockUnlocked -= UnlockTile;
                ((BoggleLevelMode)_boggle).BugFound -= OnBugFound;
                break;
            case BoggleMode.Endless:
                ((BoggleEndlessMode)_boggle).LetterChanged -= UpdateTile;
                break;
        }
    }

    private void Start()
    {
        print($"possible words: {_boggle.PossibleWords}");
    }

    public void SetBoggleMode(int mode)
    {
        boggleMode = (BoggleMode)mode;
    }
    
    public bool NotifyPathStart(LetterTile letter)
    {
        var valid = _boggle.NotifyPathStart(letter.Position, (res, message) =>
        {
            if(!res)
                LevelManager.NotifyWarning(message);
        });
        return valid;
    }

    public bool RegisterForPath(LetterTile letter)
    {
        var valid = _boggle.RegisterForPath(letter.Position, (res, message) =>
        {
            if (res) return;
            _letterTiles.Values.ToList().ForEach(lt => lt.SetHighlighted(false));
            LevelManager.NotifyWarning(message);
        });
        return valid;
    }

    public bool NotifyPathComplete()
    {
        var (valid, word) = _boggle.NotifyPathComplete(
            onComplete:(res, message) =>
            {
                if (res) return;
                LevelManager.NotifyWarning(message);
            }
        );
        if (valid)
            LevelManager.NotifyWarning($"{word} is valid");
        _letterTiles.Values.ToList().ForEach(lt => lt.SetHighlighted(false));
        return valid;
    }
    
    private void UnlockTile(Vector2Int position)
    {
        _letterTiles[position].SetLocked(false);
    }

    private void OnBugFound(Vector2Int position)
    {
        print("Bug found");
        _letterTiles[position].SetAsBug(false);
    }

    private void UpdateTile(Vector2Int position, char letter)
    {
        _letterTiles[position].Letter = letter;
    }

    private void CreateBoard(char[,] board)
    {
        int rows = gridSize.x, cols = gridSize.y;
        boardGridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        boardGridLayout.constraintCount = cols;
        var spacing = 30f;
        var boardWidth = ((RectTransform)boardGridLayout.transform).rect.width;
        boardGridLayout.spacing = spacing * Vector2.one;
        boardGridLayout.cellSize = new Vector2(boardWidth/cols - spacing, boardWidth/rows - spacing);
        for (var i = 0; i < rows; i++) {
            for (var j = 0; j < cols; j++) {
                var c = board[i, j];
                var letter = Instantiate(letterTilePrefab, boardRoot);
                _letterTiles.Add(new Vector2Int(i, j), letter);
                letter.Letter = c;
                letter.Position = new Vector2Int(i, j);
                if (boggleMode != BoggleMode.Level || _boggle is not BoggleLevelMode boggleLevelMode) continue;
                switch (boggleLevelMode.TilesType[i, j])
                {
                    case BoggleLevelMode.TileType.Bug:
                        letter.SetAsBug();
                        break;
                    case BoggleLevelMode.TileType.Lock:
                        letter.SetLocked();
                        break;
                }
            }
        }
    }
    
}
