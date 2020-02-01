using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2 _gridPos;
    public float _cloudDensity;
    public float _cloudThreshold;

    [SerializeField]
    private GameObject m_cloud = null;

    private void Start()
    {
        
    }

    public void AddFartCloud(float density)
    {
        _cloudDensity += density;
        
        // Also create/scale the cloud object for this cell.

    }

    private void Update()
    {
        // Cloud timer
    }
}