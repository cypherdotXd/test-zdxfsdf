using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnsCountText;
    [SerializeField] private TMP_Text matchCountText;
    
    private void OnEnable()
    {
        LevelManager.TurnPlayed += UpdateTurnsCount;
        LevelManager.CardsMatched += UpdateMatchCount;
    }

    private void OnDisable()
    {
        LevelManager.TurnPlayed -= UpdateTurnsCount;
        LevelManager.CardsMatched -= UpdateMatchCount;
    }

    private void UpdateTurnsCount(int count)
    {
        turnsCountText.text = count.ToString();
    }

    private void UpdateMatchCount(int count, Card c1, Card c2)
    {
        matchCountText.text = count.ToString();
    }
}
