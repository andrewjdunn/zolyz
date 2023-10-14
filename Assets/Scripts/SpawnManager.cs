using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject areaObject;
    public GameObject pestParent;
    public GameObject[] enemyPrefabs;

    private Bounds areaBounds;

    // Start is called before the first frame update
    void Start()
    {
        var areaCollider = areaObject.GetComponent<Collider>();
        areaBounds = areaCollider.bounds;

        // Not sure yet how this will work - I think these will arrive in levels or waves
        for (int i = 0; i < 4; i++)
        {
            SpawnRandomEnemy();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomEnemy()
    {
        var pest = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        
        var minX = areaBounds.min.x + 0.5f;
        var maxX = areaBounds.max.x - 0.5f;

        var minZ = areaBounds.min.z + 0.5f;
        var maxZ = areaBounds.max.z - 0.5f;
        
        var position = new Vector3(Random.Range(minX, maxX), 0.1f, Random.Range(minZ, maxZ));
        Instantiate(pest, position, pest.transform.rotation, pestParent.transform);
    }
}
