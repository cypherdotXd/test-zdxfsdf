using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnsCountText;
    [SerializeField] private TMP_Text matchCountText;
    [SerializeField] private Button homeButton;
    
    private void OnEnable()
    {
        LevelManager.TurnPlayed += UpdateTurnsCount;
        LevelManager.CardsMatched += UpdateMatchCount;
        homeButton.onClick.AddListener(ReloadGame);
    }

    private void OnDisable()
    {
        LevelManager.TurnPlayed -= UpdateTurnsCount;
        LevelManager.CardsMatched -= UpdateMatchCount;
        homeButton.onClick.RemoveListener(ReloadGame);
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(0);
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
