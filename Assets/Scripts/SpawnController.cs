using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Drag a prefab in unity onto this field
    public GameObject slimePrefab;
    public int spawnSpeed = 10;
    public float spawnRepeatRate = 1f;
    public int spawnRange = 2;
    public int maxNumber = 20; // TODO: Do this

    private readonly System.Random rnd = new();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnSlime), 0, spawnRepeatRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnSlimeRandomChance()
    {
        // print("Fixed update on SpawnController");
        int number = rnd.Next(0, 100);
        if (number < spawnSpeed)
        {
            SpawnSlime();
        }
    }

    private void SpawnSlime()
    {
        print(GameObject.FindGameObjectsWithTag("Enemy").Length);
        float randX = rnd.Next(-spawnRange * 100, spawnRange * 100) * 0.01f;
        float randY = rnd.Next(-spawnRange * 100, spawnRange * 100) * 0.01f;
        GameObject newSlime = Instantiate(slimePrefab, new Vector3(randX, randY, 0), Quaternion.identity);
        newSlime.transform.parent = GameObject.Find("SlimeGroup").transform;
    }
}
