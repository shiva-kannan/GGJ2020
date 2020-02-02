using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
