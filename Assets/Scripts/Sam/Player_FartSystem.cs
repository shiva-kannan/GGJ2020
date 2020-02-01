using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FartSystem : MonoBehaviour
{
    public float fartValue; // How much cloud density does one fart increase.
    private float fartMeter = 0f; // How long can the angel keep farting, represented in seconds.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("How long can I fart: " + fartMeter);

        if (Input.GetKey(KeyCode.Space))
        {
            if (fartMeter > 0)
            {
                ReleaseFart();
                fartMeter -= Time.deltaTime;
            }
            else
            {
                fartMeter = 0f;
            }
        }
    }

    public void AccumulateFart(float amount)
    {
        fartMeter += amount;
    }


    public void ReleaseFart()
    {
        //Debug.Log("Farting rn!");
        TileCell cellToFartOn = TileMap.Instance.GetTileUnderPoint(transform.position);
        cellToFartOn._cloudDensity += fartValue;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Food")
        {
            // Consume food
            if (Input.GetKeyDown(KeyCode.X))
            {
                Destroy(other.gameObject);

            }
        }
    }
}
