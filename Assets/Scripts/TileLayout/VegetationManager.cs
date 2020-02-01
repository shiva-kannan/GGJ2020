using System.Collections;
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
    private List<GameObject> _plantPrefabs = new List<GameObject>();

    private void Start()
    {

    }

    public GameObject GetRandomPlant()
    {
        int choice = Random.Range(0, _plantPrefabs.Count);
        GameObject newPlant = Instantiate<GameObject>(_plantPrefabs[choice]);
        return newPlant;
    }
}
