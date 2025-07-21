using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    private readonly Queue<ImageCard> _matchQueue = new();
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
        if (_matchQueue.TryPeek(out var firstCard) && firstCard == imageCard) return;
        
        _matchQueue.Enqueue(imageCard);
        var matched = TryMatch(out var cardOne, out var cardTwo);
        if (matched == null) return;
        if (matched.Value)
        {
            cardOne.gameObject.SetActive(false);    
            cardTwo.gameObject.SetActive(false);
        }
        else
        {
            cardOne.Fold();
            cardTwo.Fold();
        }
    }

    private bool? TryMatch(out ImageCard imageCard1, out ImageCard imageCard2)
    {
        if (_matchQueue.Count < 2)
        {
            imageCard1 = null;
            imageCard2 = null;
            return null;
        }
        
        imageCard1 = _matchQueue.Dequeue();
        imageCard2 = _matchQueue.Dequeue();
        return imageCard1.Match(imageCard2);
    }
}
