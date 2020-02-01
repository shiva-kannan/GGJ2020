using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FartSystem : MonoBehaviour
{
    public float fartValue; // How much cloud density does one fart increase.
    public float fartInterval; // How long is the arbitrary gay between each fart.
    public float fartPushForce; // A small force that the fart applies to player.

    private float fartMeter = 4f; // How long can the angel keep farting, represented in seconds.
    private float fartTimer = 0f;

    [SerializeField]
    private ParticleSystem m_fartParticles = null;

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
                    fartTimer = fartInterval;
                }

                
                if (!m_fartParticles.isPlaying)
                {
                    m_fartParticles.Play();
                }
                fartMeter -= Time.deltaTime;

                GetComponent<Rigidbody>().velocity = GetComponent<Player_Control>().GetFaceDirect() * fartPushForce * Time.deltaTime;
            }
            else
            {
                fartMeter = 0f;
                m_fartParticles.Stop();
            }
        }
        else
        {
            fartTimer = 0;
            m_fartParticles.Stop();
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void AccumulateFart(float amount)
    {
        fartMeter += amount;
    }


    public void ReleaseFart()
    {
        //Debug.Log("Farting rn!");

        if (TileMap.Instance != null)
        {
            TileCell cellToFartOn = TileMap.Instance.GetTileUnderPoint(transform.position);
            if (cellToFartOn != null)
            {
                cellToFartOn.AddFartCloud(fartValue);
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
                fartMeter += other.GetComponent<Food_Property>().foodValue;
            }
        }
    }
}
