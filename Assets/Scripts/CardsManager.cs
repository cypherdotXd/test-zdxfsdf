using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _cardsGridLayout;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotsParent;
    [SerializeField] private int _slotsCount;

    private List<Card> _allCards = new();
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
        CreateSlots(_slotsCount);
        _allCards = _slotsParent.GetComponentsInChildren<Card>().ToList();
    }

    private void Start()
    {
        StartCoroutine(RevealAllCardsRoutine());
    }

    public void RegisterCardForMatch(ImageCard imageCard)
    {
        if (_matchQueue.TryPeek(out var firstCard) && firstCard == imageCard) return;
        
        _matchQueue.Enqueue(imageCard);
        var matched = TryMatch(out var cardOne, out var cardTwo);
        if (matched == null) return;
        StartCoroutine(UpdateMatchedCardsRoutine(matched.Value, cardOne, cardTwo));
    }

    private void CreateSlots(int n)
    {
        var _cardsGridEnabled = _cardsGridLayout.enabled; 
        _cardsGridLayout.enabled = false;
        var presentSlotsCount = _slotsParent.childCount;
        var countDifference = presentSlotsCount - n;
        var start = countDifference > 0 ? n : presentSlotsCount;
        for (var i = start; i < start + Mathf.Abs(countDifference); i++)
        {
            if (countDifference > 0)
            {
                _slotsParent.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                Instantiate(_slotPrefab, _slotsParent);
            }
        }
        
        for (int i = 0; i < n; i++)
        {
            _slotsParent.GetChild(i).gameObject.SetActive(true);
        }
        _cardsGridLayout.enabled = true;
    }

    private bool? TryMatch(out Card imageCard1, out Card imageCard2)
    {
        if (_matchQueue.Count < 2)
        {
            imageCard1 = null; imageCard2 = null;
            return null;
        }

        LevelManager.NotifyTurnPlayed();
        
        imageCard1 = _matchQueue.Dequeue();
        imageCard2 = _matchQueue.Dequeue();
        return imageCard1.Match(imageCard2);
    }

    private IEnumerator RevealAllCardsRoutine(float duration = 1)
    {
        yield return null;
        _allCards.ForEach(card => card.Flip());
        yield return new WaitForSeconds(duration);
        _allCards.ForEach(card => card.Fold());
    }
    
    private IEnumerator UpdateMatchedCardsRoutine(bool matched, Card card1, Card card2, float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        if (matched)
        {
            card1.transform.DOScale(Vector3.zero, 0.2f);
            card2.transform.DOScale(Vector3.zero, 0.2f);
            LevelManager.NotifyCardsMatched(card1, card2);
        }
        else
        {
            card1.Fold();
            card2.Fold();
        }
    }
}
