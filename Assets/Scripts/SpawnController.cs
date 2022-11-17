using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Drag a prefab in unity onto this field
    public GameObject slimePrefab;
    public int spawnSpeed = 10;
    public int spawnRange = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        // print("Fixed update on SpawnController");
         System.Random rnd = new();
        int number = rnd.Next(0,1000);
        if (number < spawnSpeed) {
            float randX = rnd.Next(-spawnRange*100,spawnRange*100)*0.01f;
            float randY = rnd.Next(-spawnRange*100,spawnRange*100)*0.01f;
            Instantiate(slimePrefab, new Vector3(randX, randY, 0), Quaternion.identity);
        }
    }
}
