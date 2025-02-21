using UnityEngine;

public class DropletsSpawner : MonoBehaviour
{

    private float spawnTime = 3f;
    private float spawnTimer;

    [SerializeField] private DropletsPoison dropletsPrefab;
    [SerializeField] private Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnTime) 
        {
            spawnTimer = 0f;
            Instantiate(dropletsPrefab, spawnPoint.position, spawnPoint.rotation);
        }
       
    }
}
