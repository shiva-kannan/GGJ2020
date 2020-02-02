using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Manager : MonoBehaviour
{
    public GameObject food1Prefab;
    public GameObject food2Prefab;
    public float spawnIntervalMAX;
    public float spawnIntervalMIN;
    public float bigFoodRatio;

    public int maxFoodNum;
    public int foodSpawnThreshold;

    private bool isSpawningFood = false;
    private float spawnTimer = 0;

    [SerializeField][Tooltip("Game time in seconds")]
    private float m_GameTime = 60;
    private float mGameTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        food1Prefab.CreatePool(30);
        food2Prefab.CreatePool(30);
        mGameTimer = m_GameTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (mGameTimer <= 0)
        {
            // Game is over
            StopSpawning();
            return;
        }
        else
        {
            mGameTimer -= Time.deltaTime;
            HUDManager.Instance.SetGameTimer(mGameTimer);
        }

        float numOfFood = GameObject.FindGameObjectsWithTag("Food").Length;
        if (numOfFood >= maxFoodNum)
        {
            StopSpawning();
        }else if (numOfFood <= foodSpawnThreshold)
        {
            StartSpawning();
        }

        if (isSpawningFood)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                // get a random pos to spawn
                Vector3 spawnPos = Vector3.zero;
                if (TileMap.Instance != null)
                {
                    float xPos = Random.Range(0, TileMap.Instance._mapSize.x);
                    float zPos = Random.Range(0, TileMap.Instance._mapSize.y);
                    float yPos = GameObject.FindWithTag("Player").transform.position.y;
                    spawnPos = TileMap.Instance._origin + new Vector3(xPos, yPos, zPos);
                }

                // determine the type of food to spawn
                float foodType = Random.Range(0f, 1f);
                if (foodType <= bigFoodRatio)
                {
                    food2Prefab.Spawn(spawnPos);
                }
                else
                {
                    food1Prefab.Spawn(spawnPos);
                }

                spawnTimer = Random.Range(spawnIntervalMIN, spawnIntervalMAX);
                
            }
        }
    }


    void StartSpawning()
    {
        isSpawningFood = true;
    }

    void StopSpawning()
    {
        isSpawningFood = false;
    }
}
