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
}
