using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action TurnPlayed;
    public static event Action<Card, Card> CardsMatched;

    public static void NotifyTurnPlayed()
    {
        TurnPlayed?.Invoke();
    }

    public static void NotifyCardsMatched(Card card1, Card card2)
    {
        CardsMatched?.Invoke(card1, card2);
    }
}
