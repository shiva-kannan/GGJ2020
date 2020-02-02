using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FartSystem : MonoBehaviour
{
    public float fartValue; // How much cloud density does one fart increase.
    public float fartInterval; // How long is the arbitrary gay between each fart.
    public float fartPushForce; // A small force that the fart applies to player.
    public float fartMAX; // Max amount of fart you can store.

    public AudioSource fartAudioSource;
    private float fartMeter = 4f; // How long can the angel keep farting, represented in seconds.
    private float fartTimer = 0f;
    private bool isSuperFart = false;

    [SerializeField]
    private ParticleSystem m_fartParticles = null;

    private AudioSource fartAudio;

    private PlayerNumber mPlayerNumer;

    // Start is called before the first frame update
    void Start()
    {
        fartAudio = GetComponent<AudioSource>();
        mPlayerNumer = gameObject.CompareTag("Player") ? PlayerNumber.Player1 : PlayerNumber.Player2;
    }

    // Update is called once per frame
    void Update()
    {
        if (mPlayerNumer == PlayerNumber.Player1)
        {
            if (Input.GetButton("Fire1"))
            {
                ProcessFarting();
                HUDManager.Instance.SetFartMeter(fartMeter / fartMAX, mPlayerNumer);
            }
            else
            {
                fartTimer = 0;
                m_fartParticles.Stop();
                AudioController.FadeOut(fartAudioSource, 0.2f);
                fartAudioSource.GetComponent<FartAudio>().fartTriggered = false;
            }
        }
        else
        {
            if (Input.GetButton("Fire2"))
            {
                ProcessFarting();
                HUDManager.Instance.SetFartMeter(fartMeter / fartMAX, mPlayerNumer);
            }
            else
            {
                fartTimer = 0;
                m_fartParticles.Stop();
                AudioController.FadeOut(fartAudioSource, 0.2f);
                fartAudioSource.GetComponent<FartAudio>().fartTriggered = false;
            }
        }

        if (mPlayerNumer == PlayerNumber.Player1)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                isSuperFart = false;
            }
        }
        else
        {
            if (Input.GetButtonUp("Fire2"))
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                isSuperFart = false;
            }
        }

        // ----------- CHEAT -------------
        if (Input.GetKeyDown(KeyCode.P))
        {
            fartMeter = fartMAX;
        }
        // ----------- End CHEAT -------------
    }

    private void ProcessFarting()
    {
        if (fartMeter > 0)
        {
            fartTimer -= Time.deltaTime;
            if (fartTimer <= 0)
            {
                if (fartMeter >= fartMAX)
                {
                    isSuperFart = true;
                }
                ReleaseFart();
                fartTimer = fartInterval;
            }


            if (!m_fartParticles.isPlaying)
            {
                m_fartParticles.Play();
            }

            // Play the fart audio
            if (!fartAudioSource.GetComponent<FartAudio>().fartTriggered)
            {
                StartCoroutine(fartAudioSource.GetComponent<FartAudio>().randomPlayFart());
            }
            fartMeter -= Time.deltaTime;
            if (isSuperFart)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Player_Control>().GetFaceDirect() * fartPushForce * 5f * Time.deltaTime;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Player_Control>().GetFaceDirect() * fartPushForce * Time.deltaTime;
            }
        }
        else
        {
            fartMeter = 0f;
            isSuperFart = false;
            m_fartParticles.Stop();
            Debug.Log("Stp the fart sound already!");
            AudioController.FadeOut(fartAudioSource, 0.2f);
            fartAudioSource.GetComponent<FartAudio>().fartTriggered = false;
        }
    }

    public void AccumulateFart(float amount)
    {
        fartMeter += amount;
        if (fartMeter >= fartMAX)
        {
            fartMeter = fartMAX;
        }

        HUDManager.Instance.SetFartMeter(fartMeter / fartMAX, mPlayerNumer);
    }


    public void ReleaseFart()
    {
        Debug.Log("Farting rn: " + mPlayerNumer);

        if (TileMap.Instance != null)
        {
            TileCell cellToFartOn = TileMap.Instance.GetTileUnderPoint(transform.position);
            if (cellToFartOn != null)
            {
                if (isSuperFart)
                {
                    cellToFartOn.AddFartCloud(fartValue * 2.5f, mPlayerNumer);
                }
                else
                {
                    cellToFartOn.AddFartCloud(fartValue, mPlayerNumer);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            // Consume food
            
            AccumulateFart(other.GetComponent<Food_Property>().foodValue);
            other.gameObject.Recycle();
        }
    }
}
