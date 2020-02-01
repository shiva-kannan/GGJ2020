using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    private TileState mCurrentState = TileState.Decaying;

    [Range(0, 10)][SerializeField]
    private float _cloudDensity;
    private float _cloudThreshold;

    private const float CloudMaxDensity = 10;

    [SerializeField]
    private GameObject m_cloud = null;

    private float m_CloudDecayRate = 10;   // Cloud decays in m_CloudDecayRate seconds
    private float mCloudDecayTimer = 0;    // In seconds

    [SerializeField]
    private float m_MaxRainTime = 5;
    private float mRainTimer = 0;

    private GameObject mPlant = null;
    [SerializeField]
    private float m_DecayDelay = 3;
    private float mPlantDecayTimer = 0;
    private float m_PlantDecayRate = 10;

    private bool mIsRaining = false;

    private void Start()
    {
        SwitchState(TileState.Normal);
    }

    public void AddFartCloud(float density)
    {
        if (mIsRaining)
        {
            return;
        }

        _cloudDensity += density;
        _cloudDensity = Mathf.Clamp(_cloudDensity, 0, CloudMaxDensity);

        // Also create/scale the cloud object for this cell.
        var ps = m_cloud.GetComponent<ParticleSystem>();
        var main = ps.main;
        var emission = ps.emission;

        emission.rateOverTime = _cloudDensity;
        //main.startColor = 

        if (_cloudDensity >= 0.9f * CloudMaxDensity)
        {
            SwitchState(TileState.Raining);
        }
    }

    private void SwitchState(TileState nextState)
    {
        if (mCurrentState == nextState)
        {
            return;
        }

        switch (nextState)
        {
            case TileState.Normal:
                mPlantDecayTimer = m_DecayDelay;
                break;
            case TileState.Decaying:
                mPlantDecayTimer = m_PlantDecayRate;
                break;
            case TileState.Raining:
                
                break;
            case TileState.Dead:
                break;
        }

        mCurrentState = nextState;
    }

    private void Update()
    {
        UpdateState();

        //if (mCurrentState != TileState.Raining)
        //{
        //    // Cloud timer
        //    if (mCloudDecayTimer <= 0)
        //    {
        //        Debug.Log("cloud decayed");
        //        Debug.Break();
        //    }
        //    else
        //    {
        //        mCloudDecayTimer -= Time.deltaTime;
        //        _cloudDensity = mCloudDecayTimer * 
        //    }
        //}
        
    }

    private void UpdateState()
    {
        switch (mCurrentState)
        {
            case TileState.Normal:
                if (mPlantDecayTimer <= 0)
                {
                    SwitchState(TileState.Decaying);
                }
                else
                {
                    //Debug.Log("Decay timer : " + mPlantDecayTimer);
                    mPlantDecayTimer -= Time.deltaTime;
                }
                break;
            case TileState.Decaying:
                if (mPlantDecayTimer <= 0)
                {
                    Debug.Log("Plant decayed");
                    SwitchState(TileState.Dead);
                }
                else
                {
                    //Debug.Log("Decay timer : " + mPlantDecayTimer);
                    mPlantDecayTimer -= Time.deltaTime;
                }
                break;
            case TileState.Raining:
                if (mRainTimer <= 0)
                {
                    mIsRaining = false;
                    if (mPlantDecayTimer < m_PlantDecayRate)
                    {
                        SwitchState(TileState.Decaying);
                    }
                    else
                    {
                        SwitchState(TileState.Normal);
                    }
                }
                break;
            case TileState.Dead:
                break;
        }
    }
}

public enum TileState
{
    Normal,
    Decaying,
    Raining,
    Dead,
}