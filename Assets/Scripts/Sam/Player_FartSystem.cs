using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FartSystem : MonoBehaviour
{
    private float fartMeter = 100; // How long can the angel keep farting, represented in seconds.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("How long can I fart: " + fartMeter);

        if (Input.GetKey(KeyCode.Space))
        {
            if (fartMeter > 0)
            {
                ReleaseFart();
                fartMeter -= Time.deltaTime;
            }
            else
            {
                fartMeter = 0;
            }
        }
    }

    public void AccumulateFart(float amount)
    {
        fartMeter += amount;
    }


    public void ReleaseFart()
    {
        Debug.Log("Farting rn!");
    }
}
