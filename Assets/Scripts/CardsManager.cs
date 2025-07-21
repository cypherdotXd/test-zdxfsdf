using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    private Queue<ImageCard> _matchQueue = new();
    public static CardsManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RegisterCardForMatch(ImageCard imageCard)
    {
        _matchQueue.Enqueue(imageCard);
    }

    public void TryMatch()
    {
        if (_matchQueue.Count < 2)
            return;
        var card1 = _matchQueue.Dequeue();
        var card2 = _matchQueue.Dequeue();
        if card1.
    }
}
