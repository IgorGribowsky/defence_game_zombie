using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EnemiesMonochromeSpawn_LoadTest : MonoBehaviour
{
    public float Rate = 1f;
    public float CountOfMovePointsToAdd = 20f;
    public List<Enemies> EnemiesList;

    private List<GameObject> movePoints;
    public float deltaTime;

    public List<GameObject> MovePoints
    {
        get { return movePoints; }
    }

    private List<GameObject> spawnPoints;
    public List<GameObject> SpawnPoints
    {
        get { return spawnPoints; }
    }

    private bool cd = false;
    private float currentTimer;

    public int Waves = 10;
    public int WaveNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = Rate;
        spawnPoints = GameObject.FindGameObjectsWithTag(UnityTags.EnemySpawnPoint.ToString()).ToList();
        movePoints = GameObject.FindGameObjectsWithTag(UnityTags.EnemyMovePoint.ToString()).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        Debug.Log(Mathf.Ceil(fps).ToString());

        if (!cd && WaveNum < Waves)
        {
            cd = true;
            foreach (var spawn in spawnPoints)
            {
                foreach (var enemies in EnemiesList)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        var enemy = Instantiate(enemies.Enemy, spawn.transform.position, spawn.transform.rotation);
                        var enemyMovement = enemy.GetComponent<EnemyMovement>();
                        if (enemyMovement != null)
                        {
                            SetupMovement(enemyMovement);
                        }
                    }
                }
            }
            WaveNum++;
        }

        currentTimer -=Time.deltaTime;
        if (currentTimer <= 0)
        {
            cd = false;
            currentTimer = Rate;
        }
    }

    private void SetupMovement(EnemyMovement enemyMovement)
    {
        for (int i = 0; i < CountOfMovePointsToAdd; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, movePoints.Count);
            enemyMovement.Points.Add(MovePoints[randomIndex].transform.position);
            enemyMovement.Loop = true;
        }
    }

    [Serializable]
    public class Enemies
    {
        public GameObject Enemy;
        public int Count;
    }
}