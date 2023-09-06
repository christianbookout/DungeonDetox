using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<GameObject> enemyPrefabs; // List of enemy prefabs
    public float timeBetweenWaves = 5.0f;
    public int numberOfWaves = 5;
    public TextMeshProUGUI waveNum;
    public TextMeshProUGUI enemiesNum;
    private GameObject player;
    public float maxDistance = 100f;
    private int currentWave = 0;
    private int enemiesToSpawn;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        int count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesNum.text = count.ToString();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy == null) continue;
            if (Vector3.Distance(player.transform.position, enemy.transform.position) > maxDistance)
            {
                Destroy(enemy);
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < numberOfWaves)
        {
            waveNum.text = "Wave " + (currentWave + 1);
            currentWave++;
            enemiesToSpawn = currentWave * 5;

            GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy(selectedEnemyPrefab);
                yield return new WaitForSeconds(1.0f);
            }

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        int randomSpawnIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomSpawnIndex];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public int GetNumberOfWaves()
    {
        return currentWave;
    }

    public int GetEnemiesRemaining()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        return enemyCount;
    }

}