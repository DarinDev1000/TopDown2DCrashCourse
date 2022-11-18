using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tileGroups;

    public GameObject prefabDirt;
    public GameObject prefabGrass;
    public GameObject prefabGrassPlant;
    public GameObject prefabBush;

    public int mapWidth = 160;
    public int mapHeight = 90;
    public float magnification = 14.0f; // between 4 - 20 recommended
    public int xOffset = 0;
    public int yOffset = 0;
    public List<int> tileOrder = new() { 0, 1, 2, 3 };

    List<List<int>> noiseGrid = new();
    List<List<GameObject>> tileGrid = new();


    // Start is called before the first frame update
    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    void CreateTileset()
    {
        tileset = new()
        {
            { tileOrder[0], prefabDirt },
            { tileOrder[1], prefabGrass },
            { tileOrder[2], prefabGrassPlant },
            { tileOrder[3], prefabBush }
        };
    }

    void CreateTileGroups()
    {
        tileGroups = new();
        foreach (KeyValuePair<int, GameObject> prefabPair in tileset)
        {
            GameObject tileGroup = new(prefabPair.Value.name);
            tileGroup.transform.parent = gameObject.transform;
            tileGroup.transform.localPosition = Vector3.zero;
            // Add tileGroup to tileGroups
            tileGroups.Add(prefabPair.Key, tileGroup);
        }
    }

    void GenerateMap()
    {
        // Loop through all x values
        for (int x = 0; x < mapWidth; x++)
        {
            // Generate a new nested list for each x value
            noiseGrid.Add(new());
            tileGrid.Add(new());

            // Loop through all y values
            for (int y = 0; y < mapHeight; y++)
            {
                int tileId = GetIdUsingPerlin(x, y);
                noiseGrid[x].Add(tileId);
                CreateTile(tileId, x, y);
            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float xModified = (x - xOffset) / magnification;
        float yModified = (y - yOffset) / magnification;
        float rawPerlin = Mathf.PerlinNoise(xModified, yModified); // Works better with float inputs
        // Output can be > 1 and < 0
        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);

        // Scale noise to tile id range
        float scaledPerlin = clampPerlin * tileset.Count;
        if (scaledPerlin == 4)
        {
            scaledPerlin = 3;
        }
        return Mathf.FloorToInt(scaledPerlin);
    }

    void CreateTile(int tileId, int x, int y)
    {
        GameObject tilePrefab = tileset[tileId];
        GameObject tileGroup = tileGroups[tileId];
        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.name = string.Format("time_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tileGrid[x].Add(tile);
    }
}
