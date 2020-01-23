using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Vector3 SpawnArea;
    public float SpawnTime;
    private float currentTime;
    public Target TargetPrefab;
    public LayerMask ObstacleLayer;
    public List<Target> targets;

    // Start is called before the first frame update
    void Start()
    {
        targets = new List<Target>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > SpawnTime) {
            doSpawnTarget();
            currentTime = 0f;
        }
    }

    private void doSpawnTarget() {
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
            targets.Add(Instantiate(TargetPrefab, spawnPosition, Quaternion.identity));
        } else {
            Debug.Log("Failed to find suitable spawn location after 10 retries. Aborting spawn.");
        }
        
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, SpawnArea);
    }
}
