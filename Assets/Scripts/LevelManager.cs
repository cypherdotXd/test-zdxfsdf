using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action<int> TurnPlayed;
    public static event Action<int> HighScoreChanged;
    public static event Action LevelCompleted;
    public static event Action<int, Card, Card> CardsMatched;

    public static int HighScore
    {
        get => PlayerPrefs.GetInt("HighScore", 0);
        set => PlayerPrefs.SetInt("HighScore", value);
    }
        
    private static int _turnsPlayed;
    private static int _matchCount;

    public static void NotifyTurnPlayed()
    {
        _turnsPlayed++;
        TurnPlayed?.Invoke(_turnsPlayed);
    }

    public static void NotifyCardsMatched(Card card1, Card card2)
    {
        _matchCount++;
        if (_matchCount > HighScore)
        {
            HighScore = _matchCount;
            HighScoreChanged?.Invoke(_matchCount);
        }
        CardsMatched?.Invoke(_matchCount, card1, card2);
    }

    public static void NotifyLevelCompleted()
    {
        _turnsPlayed = 0;
        _matchCount = 0;
        LevelCompleted?.Invoke();
    }
}
