using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text turnsCountText;
    [SerializeField] private TMP_Text matchCountText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button reloadButton;
    
    private void OnEnable()
    {
        LevelManager.TurnPlayed += UpdateTurnsCount;
        LevelManager.CardsMatched += UpdateMatchCount;
        LevelManager.HighScoreChanged += UpdateHighScore;
        LevelManager.LevelCompleted += ShowEndScreen;
        homeButton.onClick.AddListener(ReloadGame);
        reloadButton.onClick.AddListener(ReloadGame);
    }

    private void OnDisable()
    {
        LevelManager.TurnPlayed -= UpdateTurnsCount;
        LevelManager.CardsMatched -= UpdateMatchCount;
        LevelManager.HighScoreChanged -= UpdateHighScore;
        LevelManager.LevelCompleted -= ShowEndScreen;
        homeButton.onClick.RemoveListener(ReloadGame);
        reloadButton.onClick.RemoveListener(ReloadGame);
    }

    private void Start()
    {
        UpdateHighScore(LevelManager.HighScore);
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowEndScreen()
    {
        DOVirtual.DelayedCall(1, () => endScreen.SetActive(true));
    }

    private void UpdateHighScore(int score)
    {
        highScoreText.text = score.ToString();
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
