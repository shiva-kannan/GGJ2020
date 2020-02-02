﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationManager : MonoBehaviour
{
    #region Singleton
    public static VegetationManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField]
    private List<GameObject> _p1PlantPrefabs = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _p2PlantPrefabs = new List<GameObject>();

    private void Start()
    {

    }

    public GameObject GetRandomPlant(PlayerNumber forPlayer)
    {
        int choice;
        GameObject newPlant;
        if (forPlayer == PlayerNumber.Player1)
        {
            choice = Random.Range(0, _p1PlantPrefabs.Count);
            newPlant = Instantiate<GameObject>(_p1PlantPrefabs[choice]);
        }
        else
        {
            choice = Random.Range(0, _p2PlantPrefabs.Count);
            newPlant = Instantiate<GameObject>(_p2PlantPrefabs[choice]);
        }
        
        return newPlant;
    }
}
