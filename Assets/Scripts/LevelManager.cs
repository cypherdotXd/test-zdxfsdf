using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action<int> TurnPlayed;
    public static event Action<int, Card, Card> CardsMatched;
    
    public static int turnsPlayed { get; private set; }
    public static int matchCount { get; private set; }

    public static void NotifyTurnPlayed()
    {
        turnsPlayed++;
        TurnPlayed?.Invoke(turnsPlayed);
    }

    public static void NotifyCardsMatched(Card card1, Card card2)
    {
        matchCount++;
        CardsMatched?.Invoke(matchCount, card1, card2);
    }
}
