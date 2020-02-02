using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    private TileState mCurrentState = TileState.Decaying;

    private Vector2 mGridPos;


    [Range(0, 10)][SerializeField]
    private float _cloudDensity;
    private float _cloudThreshold;

    private const float CloudMaxDensity = 10;

    [SerializeField]
    private GameObject m_cloud = null;
    [SerializeField]
    private ParticleSystem m_rainParticles = null;

    [SerializeField]
    private float m_MaxCloudDecayTime = 10;   // Cloud decays in m_CloudDecayRate seconds
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

    public Vector2 pGridPos { get => mGridPos; set => mGridPos = value; }

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
        SetCloudDensity();
        //main.startColor = 
        mCloudDecayTimer = m_MaxCloudDecayTime;

        if (_cloudDensity >= 0.9f * CloudMaxDensity)
        {
            SwitchState(TileState.Raining);
        }
    }

    private void SetCloudDensity()
    {
        _cloudDensity = Mathf.Clamp(_cloudDensity, 0, CloudMaxDensity);

        // Also create/scale the cloud object for this cell.
        var ps = m_cloud.GetComponent<ParticleSystem>();
        var main = ps.main;
        var emission = ps.emission;

        if (_cloudDensity > 0)
        {
            emission.enabled = true;
        }
        else
        {
            emission.enabled = false;
        }

        emission.rateOverTime = _cloudDensity;
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
                GrowVegetation();
                mIsRaining = true;
                mRainTimer = m_MaxRainTime;
                m_rainParticles.Play();
                break;
            case TileState.Dead:
                break;
        }

        mCurrentState = nextState;
    }

    private void GrowVegetation()
    {
        mPlant = VegetationManager.Instance.GetRandomPlant();
        mPlant.transform.SetParent(transform);
        mPlant.transform.localPosition = new Vector3(0,0.5f,0);
        mPlant.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        UpdateState();

        if (!mIsRaining)
        {
            // Cloud timer
            if (mCloudDecayTimer > 0)
            {
                mCloudDecayTimer -= Time.deltaTime;
                _cloudDensity = mCloudDecayTimer * CloudMaxDensity / m_MaxCloudDecayTime;
                SetCloudDensity();
            }
        }
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
                    m_rainParticles.Stop();
                    mIsRaining = false;
                    _cloudDensity = 0;
                    SetCloudDensity();
                    if (mPlantDecayTimer < m_PlantDecayRate)
                    {
                        SwitchState(TileState.Decaying);
                    }
                    else
                    {
                        SwitchState(TileState.Normal);
                    }
                }
                else
                {
                    mRainTimer -= Time.deltaTime;
                    float sizeDelta = 0.1f * Time.deltaTime;
                    float size = Mathf.Clamp01(mPlant.transform.localScale.x + sizeDelta);
                    Debug.Log("Plant size : " + size);
                    mPlant.transform.localScale = Vector3.one * size;
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