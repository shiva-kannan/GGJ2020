﻿using System.Collections;
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

    [SerializeField]
    private ParticleSystem m_fartParticles = null;

    private AudioSource fartAudio;
    // Start is called before the first frame update
    void Start()
    {
        fartAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (Input.GetButton("Fire1"))
            {
                ProcessFarting();
                HUDManager.Instance.SetFartMeter(fartMeter / fartMAX, 1);
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
                HUDManager.Instance.SetFartMeter(fartMeter / fartMAX, 2);
            }
            else
            {
                fartTimer = 0;
                m_fartParticles.Stop();
                AudioController.FadeOut(fartAudioSource, 0.2f);
                fartAudioSource.GetComponent<FartAudio>().fartTriggered = false;
            }
        }

        if (gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonUp("Fire1"))
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        else
        {
            if (Input.GetButtonUp("Fire2"))
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
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

            GetComponent<Rigidbody>().velocity = GetComponent<Player_Control>().GetFaceDirect() * fartPushForce * Time.deltaTime;
        }
        else
        {
            fartMeter = 0f;
            m_fartParticles.Stop();
        }
    }

    public void AccumulateFart(float amount)
    {
        fartMeter += amount;
        if (fartMeter >= fartMAX)
        {
            fartMeter = fartMAX;
        }

        int player = gameObject.CompareTag("Player") ? 1 : 2;

        HUDManager.Instance.SetFartMeter(fartMeter / fartMAX, player);
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
