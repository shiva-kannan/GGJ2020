using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FartSystem : MonoBehaviour
{
    public float fartValue; // How much cloud density does one fart increase.
    public float fartInterval; // How long is the arbitrary gay between each fart. 

    private float fartMeter = 10f; // How long can the angel keep farting, represented in seconds.
    private float fartTimer = 0f;

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
                fartTimer -= Time.deltaTime;
                if (fartTimer <= 0)
                {
                    ReleaseFart();
                    fartMeter -= Time.deltaTime;

                    fartTimer = fartInterval;
                }
            }
            else
            {
                fartMeter = 0f;
            }
        }
        else
        {
            fartTimer = 0;
        }
    }

    public void AccumulateFart(float amount)
    {
        fartMeter += amount;
    }


    public void ReleaseFart()
    {
        //Debug.Log("Farting rn!");
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        if (TileMap.Instance != null)
        {
            TileCell cellToFartOn = TileMap.Instance.GetTileUnderPoint(transform.position);
            if (cellToFartOn != null)
            {
                cellToFartOn._cloudDensity += fartValue;
            }
        }
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
