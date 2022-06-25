using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject checkPointPrefab;
    [SerializeField] int checkPointSpawnDelay = 10;
    [SerializeField] float spawnRadius = 10;
    [SerializeField] GameObject[] powerUpPrefab;
    [SerializeField] int powerUpSpawnDelay = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCheckpointRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnCheckpointRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkPointSpawnDelay);
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            Instantiate(checkPointPrefab, randomPosition, Quaternion.identity);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(powerUpSpawnDelay);
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            int randomPrefab = Random.Range(0, powerUpPrefab.Length);
            Instantiate(powerUpPrefab[randomPrefab], randomPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
