using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Vector3 SpawnArea;
    public float SpawnTime;
    private float currentTime;
    public Enemy EnemyPrefab;
    public LayerMask ObstacleLayer;
    public List<Enemy> enemies {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {
        enemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > SpawnTime) {
            SpawnEnemy();
            currentTime = 0f;
        }
    }

    private void SpawnEnemy() {
        Vector3 spawnPosition = transform.position + new Vector3(SpawnArea.x * Random.Range(-0.5f, 0.5f),
        1f,
        SpawnArea.z * Random.Range(-0.5f, 0.5f));

        int spawnAttempts = 0;
        do {
            spawnPosition = transform.position + new Vector3(SpawnArea.x * Random.Range(-0.5f, 0.5f),
                                        1f,
                                        SpawnArea.z * Random.Range(-0.5f, 0.5f));
            spawnAttempts++;
        } while(Physics.OverlapSphere(spawnPosition, 2f, ObstacleLayer).Length > 0
            && spawnAttempts < 10);
        if(spawnAttempts < 10) {
            enemies.Add(Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity));
        } else {
            Debug.Log("Failed to find suitable spawn location after 10 retries. Aborting spawn.");
        }
        
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, SpawnArea);
    }
}
