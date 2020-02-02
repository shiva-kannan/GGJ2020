using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    private TileState mCurrentState = TileState.Decaying;

    private Vector2 mGridPos;

    [SerializeField]
    private float _cloudDensity;

    private const float CloudMaxDensity = 10;

    [SerializeField]
    private GameObject m_cloud1 = null;
    [SerializeField]
    private GameObject m_cloud2 = null;
    [SerializeField]
    private ParticleSystem m_rainParticles1 = null;
    [SerializeField]
    private ParticleSystem m_rainParticles2 = null;

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
    private PlayerNumber mCurrentPlayer = PlayerNumber.None;

    public Vector2 pGridPos { get => mGridPos; set => mGridPos = value; }

    private void Start()
    {
        SwitchState(TileState.Normal);
    }

    public void AddFartCloud(float density, PlayerNumber forPlayer)
    {
        if (mIsRaining || mPlant != null)
        {
            return;
        }

        //Debug.Log("Add fart: " + forPlayer);

        _cloudDensity += density;
        SetCloudDensity(false, forPlayer);
        
        mCloudDecayTimer = m_MaxCloudDecayTime;

        if (_cloudDensity >= 0.9f * CloudMaxDensity)
        {
            mCurrentPlayer = forPlayer;
            SwitchState(TileState.Raining);
        }
    }

    private void SetCloudDensity(bool reduce, PlayerNumber forPlayer)
    {
        _cloudDensity = Mathf.Clamp(_cloudDensity, 0, CloudMaxDensity);

        //Debug.Log("Set Cloud: " + forPlayer);

        if (!reduce)
        {
            // Also create/scale the cloud object for this cell
            GameObject m_cloud;
            if (forPlayer == PlayerNumber.Player1)
            {
                m_cloud = m_cloud1;
                //Debug.Log("For P1");
            }
            else
            {
                m_cloud = m_cloud2;
                //Debug.Log("For P2");
            }
            var ps = m_cloud.GetComponent<ParticleSystem>();
            var main = ps.main;
            var emission = ps.emission;

            Color cloudColor = main.startColor.color;
            if (!reduce)
            {
                //cloudColor = forPlayer == PlayerNumber.Player1 ? Color.blue : Color.red;
            }

            if (_cloudDensity > 0)
            {
                emission.enabled = true;
            }
            else
            {
                emission.enabled = false;
            }

            emission.rateOverTime = _cloudDensity;

            cloudColor.a = _cloudDensity / CloudMaxDensity;
            //main.startColor = cloudColor;
        }
        else
        {
            var ps1 = m_cloud1.GetComponent<ParticleSystem>();
            var main1 = ps1.main;
            var emission1 = ps1.emission;

            var ps2 = m_cloud2.GetComponent<ParticleSystem>();
            var main2 = ps2.main;
            var emission2 = ps2.emission;


            if (emission1.enabled)
            {
                //Debug.Log("Reduce P1");
                Color cloudColor = main1.startColor.color;
                if (_cloudDensity > 0)
                {
                    emission1.enabled = true;
                }
                else
                {
                    emission1.enabled = false;
                }

                emission1.rateOverTime = _cloudDensity;

                cloudColor.a = _cloudDensity / CloudMaxDensity;
                //main.startColor = cloudColor;
            }else if (emission2.enabled)
            {
                Debug.Log("Reduce P2");
                Color cloudColor = main2.startColor.color;
                if (_cloudDensity > 0)
                {
                    emission2.enabled = true;
                }
                else
                {
                    emission2.enabled = false;
                }

                emission2.rateOverTime = _cloudDensity;

                cloudColor.a = _cloudDensity / CloudMaxDensity;
                //main.startColor = cloudColor;
            }

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
                GrowVegetation();
                mIsRaining = true;
                mRainTimer = m_MaxRainTime;

                ParticleSystem m_rainParticles;
                if (mCurrentPlayer == PlayerNumber.Player1)
                {
                    m_rainParticles = m_rainParticles1;
                }
                else
                {
                    m_rainParticles = m_rainParticles2;
                }
                m_rainParticles.Play();
                break;
            case TileState.Dead:
                break;
        }

        mCurrentState = nextState;
    }

    private void GrowVegetation()
    {
        if (mPlant != null)
        {
            return;
        }

        mPlant = VegetationManager.Instance.GetRandomPlant(mCurrentPlayer);
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
                SetCloudDensity(true, PlayerNumber.None);
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
                    //Debug.Log("Plant decayed");
                    SwitchState(TileState.Dead);
                }
                else
                {
                    //Debug.Log("Decay timer : " + mPlantDecayTimer);
                    mPlantDecayTimer -= Time.deltaTime;

                    //if (mPlant != null)
                    //{
                    //    float sizeDelta = 0.035f * Time.deltaTime;
                    //    float size = Mathf.Clamp01(mPlant.transform.localScale.x - sizeDelta);
                    //    Debug.Log("Plant size : " + size);
                    //    mPlant.transform.localScale = Vector3.one * size;
                    //}
                }
                break;
            case TileState.Raining:
                if (mRainTimer <= 0)
                {
                    mCurrentPlayer = PlayerNumber.None;
                    m_rainParticles1.Stop();
                    m_rainParticles2.Stop();
                    mIsRaining = false;
                    _cloudDensity = 0;
                    SetCloudDensity(true, PlayerNumber.None);
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
                    //Debug.Log("Plant size : " + size);
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