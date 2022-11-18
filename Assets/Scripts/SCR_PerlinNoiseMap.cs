using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tileGroups;

    public GameObject prefabDirt;
    public GameObject prefabGrass;
    // public GameObject prefabGrassPlant;
    // public GameObject prefabBush;
    public GameObject dirtTop;
    public GameObject dirtBottom;
    public GameObject dirtLeft;
    public GameObject dirtRight;
    public GameObject dirtOuterTopLeft;
    public GameObject dirtOuterTopRight;
    public GameObject dirtOuterBottomLeft;
    public GameObject dirtOuterBottomRight;
    public GameObject dirtInnerTopLeft;
    public GameObject dirtInnerTopRight;
    public GameObject dirtInnerBottomLeft;
    public GameObject dirtInnerBottomRight;
    public GameObject dirtSingle;

    public int mapWidth = 160;
    public int mapHeight = 90;
    public float magnification = 14.0f; // between 4 - 20 recommended
    public int xOffset = 0;
    public int yOffset = 0;
    public List<int> tileOrder = new() { 0, 1, 2, 3 };
    public float unitPixels = 0.16f;

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
        /**
            Collect and assign ID codes to the tile prefab, for ease of access.
            Best ordered to match land elevation.
        **/

        tileset = new()
        {
            { tileOrder[0], prefabDirt },
            { tileOrder[1], prefabGrass },
            // { tileOrder[2], prefabGrassPlant },
            // { tileOrder[3], prefabBush }
        };
    }

    void CreateTileGroups()
    {
        /**
            Create empty gameObjects for grouping tiles of the same type,
            ie dirt tiles
        **/

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
        /**
            Generate a 2D grid using the Perlin noise function, storing it as
            both raw ID values and tile gameObjects
        **/

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
        /**
           Using a grid to coordinate input, generate a Perlin noise values
           to be converted into a tile ID code. Rescale the normalized Perlin value
           to the number of tiles available.
       **/

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
        /**
           Creates a new tile using the type id code, group it with common tiles,
           set it's position and store the gameObject.
       **/

        GameObject tilePrefab = tileset[tileId];
        GameObject tileGroup = tileGroups[tileId];
        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        // Scale to 16 pixel tiles and center map
        float xUnit = x * unitPixels - mapWidth * unitPixels / 2;
        float yUnit = y * unitPixels - mapHeight * unitPixels / 2;

        tile.name = string.Format("time_x{0}_y{1}", xUnit, yUnit);
        tile.transform.localPosition = new Vector3(xUnit, yUnit, 0);

        tileGrid[x].Add(tile);
    }

    void CreateTileEdge(int tileId, int x, int y)
    {
        /**
           Creates a new tile using the type id code, group it with common tiles,
           set it's position and store the gameObject.
       **/

        GameObject tilePrefab = tileset[tileId];
        GameObject tileGroup = tileGroups[tileId];
        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        // Scale to 16 pixel tiles and center map
        float xUnit = x * unitPixels - mapWidth * unitPixels / 2;
        float yUnit = y * unitPixels - mapHeight * unitPixels / 2;

        tile.name = string.Format("time_x{0}_y{1}", xUnit, yUnit);
        tile.transform.localPosition = new Vector3(xUnit, yUnit, 0);

        tileGrid[x].Add(tile);
    }
}
