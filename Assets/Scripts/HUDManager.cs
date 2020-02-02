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

    public Image _fartBarFill = null;
    public Image _healthBarFill = null;

    public void SetFartMeter(float percent)
    {
        _fartBarFill.fillAmount = percent;
    }
}
