using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Image _fartBar1Fill = null;
    public Image _fartBar2Fill = null;
    public TMPro.TextMeshProUGUI _gameTimer = null;

    public TMPro.TextMeshProUGUI _p1ScoreText = null;
    public TMPro.TextMeshProUGUI _p2ScoreText = null;

    public GameObject startScreen = null;
    public GameObject gameOverScreen = null;
    public TMPro.TextMeshProUGUI _winText = null;

    public bool iSGameStarted = false;

    public void StartGame()
    {
        iSGameStarted = true;
        startScreen.SetActive(false);
        GameObject.FindObjectOfType<SoundTrack>().stopRandomGeneration = false;
    }

    public void SetFartMeter(float percent, PlayerNumber forPlayer)
    {
        if (forPlayer == PlayerNumber.Player1)
        {
            _fartBar1Fill.fillAmount = percent;
        }
        else
        {
            _fartBar2Fill.fillAmount = percent;
        }
    }

    public void SetGameTimer(float timeLeft)
    {
        string timeLeftStr = "Time left : " + (int)timeLeft;
        _gameTimer.text = timeLeftStr;
    }

    public void SetScore(PlayerNumber playerNumber, int howMuch)
    {
        string scoreText = "";
        if (playerNumber == PlayerNumber.Player1)
        {
            scoreText = "Player 1 : " + howMuch;
            _p1ScoreText.text = scoreText;
        }
        else
        {
            scoreText = "Player 2 : " + howMuch;
            _p2ScoreText.text = scoreText;
        }
    }

    public void ShowGameOver()
    {
        GameObject.FindObjectOfType<SoundTrack>().stopRandomGeneration = true;
        PlayerNumber whoWon;
        string playerName;
        if (VegetationManager.Instance.player1Score == VegetationManager.Instance.player2Score)
        {
            whoWon = PlayerNumber.None;
            playerName = "DRAW!";
        }
        else if (VegetationManager.Instance.player1Score > VegetationManager.Instance.player2Score)
        {
            whoWon = PlayerNumber.Player1;
            playerName = "Player 1 Wins!";
        }
        else
        {
            whoWon = PlayerNumber.Player2;
            playerName = "Player 2 Wins!";
        }

        _winText.text = playerName;
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
