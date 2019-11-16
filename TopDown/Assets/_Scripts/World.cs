using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public Vector3 SpawnArea;
    public float SpawnTime;
    private float currentTime;
    public Target TargetPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Vector3 spawnOffset = new Vector3(SpawnArea.x * Random.Range(-0.5f, 0.5f),
        1f,
        SpawnArea.z * Random.Range(-0.5f, 0.5f));
        
        Instantiate(TargetPrefab, transform.position + spawnOffset, Quaternion.identity);
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, SpawnArea);
    }
}
