using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SCR_PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> dirtEdgeTileset;
    Dictionary<int, GameObject> tileGroups;

    public GameObject prefabDirt;
    public GameObject prefabGrass;
    // public GameObject prefabGrassPlant;
    public GameObject prefabBush;
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
    public GameObject dirtLeftTopBottom;
    public GameObject dirtTopBottom;
    public GameObject dirtRightTopBottom;
    public GameObject dirtTopLeftRight;
    public GameObject dirtLeftRight;
    public GameObject dirtBottomLeftRight;
    public GameObject dirtInnerTopLeftAndBottomRight;
    public GameObject dirtInnerTopRightAndBottomLeft;
    public GameObject dirtInnerTop;
    public GameObject dirtInnerBottom;
    public GameObject dirtInnerLeft;
    public GameObject dirtInnerRight;

    public int mapWidth = 160;
    public int mapHeight = 90;
    public float magnification = 14.0f; // between 4 - 20 recommended
    public int xOffset = 0;
    public int yOffset = 0;
    // public List<int> tileOrder = new() { 0, 1 };
    public float unitPixels = 1f; // Grid PPU

    List<List<int>> noiseGrid = new();
    List<List<GameObject>> tileGrid = new();

    public Grid tilemapGrid;
    public Tilemap tilemap;


    // Start is called before the first frame update
    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
        GenerateMapEdges();
    }

    void CreateTileset()
    {
        /**
            Collect and assign ID codes to the tile prefab, for ease of access.
            Best ordered to match land elevation.
        **/

        tileset = new()
        {
            { 0, prefabDirt },
            { 1, prefabGrass },
            // { 2, prefabBush }
            // { tileOrder[2], prefabGrassPlant },
        };

        dirtEdgeTileset = new()
        {
            { 0, prefabDirt },
            { 1, dirtTop },
            { 2, dirtBottom },
            { 3, dirtLeft },
            { 4, dirtRight },
            { 5, dirtOuterTopLeft },
            { 6, dirtOuterTopRight },
            { 7, dirtOuterBottomLeft },
            { 8, dirtOuterBottomRight },
            { 9, dirtInnerTopLeft },
            { 10, dirtInnerTopRight },
            { 11, dirtInnerBottomLeft },
            { 12, dirtInnerBottomRight },
            { 13, dirtSingle },
            { 14, dirtLeftTopBottom },
            { 15, dirtTopBottom },
            { 16, dirtRightTopBottom },
            { 17, dirtTopLeftRight },
            { 18, dirtLeftRight },
            { 19, dirtBottomLeftRight },
            { 20, dirtInnerTopLeftAndBottomRight },
            { 21, dirtInnerTopRightAndBottomLeft },
            { 22, dirtInnerTop },
            { 23, dirtInnerBottom },
            { 24, dirtInnerLeft },
            { 25, dirtInnerRight },
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
        if (scaledPerlin == tileset.Count)
        {
            scaledPerlin = tileset.Count - 1;
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
        // GameObject tile = tilemap.SetTile(new Vector3(x, y, 0));

        // Scale to 16 pixel tiles and center map
        float xUnit = x * unitPixels - mapWidth * unitPixels / 2;
        float yUnit = y * unitPixels - mapHeight * unitPixels / 2;

        tile.name = string.Format("time_x{0}_y{1}", xUnit, yUnit);
        tile.transform.localPosition = new Vector3(xUnit, yUnit, 0);

        tileGrid[x].Add(tile);
    }

    void GenerateMapEdges()
    {
        // Loop through all x values
        for (int x = 0; x < mapWidth; x++)
        {
            // Loop through all y values
            for (int y = 0; y < mapHeight; y++)
            {
                if (noiseGrid[x][y] == 0)
                {
                    // print("dirt edge");
                    // int dirtEdgeTileId = 0;

                    bool firstRow = false;
                    bool lastRow = false;
                    bool firstColumn = false;
                    bool lastColumn = false;
                    if (x == 0) firstRow = true;
                    if (x == mapWidth - 1) lastRow = true;
                    if (y == 0) firstColumn = true;
                    if (y == mapHeight - 1) lastColumn = true;

                    int leftTile = 1;
                    int rightTile = 1;
                    int topTile = 1;
                    int bottomTile = 1;

                    if (!firstRow) leftTile = noiseGrid[x - 1][y];
                    if (!lastRow) rightTile = noiseGrid[x + 1][y];
                    if (!firstColumn) bottomTile = noiseGrid[x][y - 1];
                    if (!lastColumn) topTile = noiseGrid[x][y + 1];

                    // { 0, prefabDirt },
                    // if (leftTile == 0 && rightTile == 0 && topTile == 0 && bottomTile == 0)
                    // {
                    //     // CreateTileEdge(0, 0, x, y);
                    // }
                    // { 1, dirtTop },
                    if (leftTile == 0 && rightTile == 0 && topTile == 1 && bottomTile == 0)
                    {
                        CreateTileEdge(0, 1, x, y);
                    }
                    // { 2, dirtBottom },
                    else if (leftTile == 0 && rightTile == 0 && topTile == 0 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 2, x, y);
                    }
                    // { 3, dirtLeft },
                    else if (leftTile == 1 && rightTile == 0 && topTile == 0 && bottomTile == 0)
                    {
                        CreateTileEdge(0, 3, x, y);
                    }
                    // { 4, dirtRight },
                    else if (leftTile == 0 && rightTile == 1 && topTile == 0 && bottomTile == 0)
                    {
                        CreateTileEdge(0, 4, x, y);
                    }
                    // { 5, dirtOuterTopLeft },
                    else if (leftTile == 1 && rightTile == 0 && topTile == 1 && bottomTile == 0)
                    {
                        CreateTileEdge(0, 5, x, y);
                    }
                    // { 6, dirtOuterTopRight },
                    else if (leftTile == 0 && rightTile == 1 && topTile == 1 && bottomTile == 0)
                    {
                        CreateTileEdge(0, 6, x, y);
                    }
                    // { 7, dirtOuterBottomLeft },
                    else if (leftTile == 1 && rightTile == 0 && topTile == 0 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 7, x, y);
                    }
                    // { 8, dirtOuterBottomRight },
                    else if (leftTile == 0 && rightTile == 1 && topTile == 0 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 8, x, y);
                    }
                    // { 13, dirtSingle }
                    else if (leftTile == 1 && rightTile == 1 && topTile == 1 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 13, x, y);
                    }
                    // { 14, dirtLeftTopBottom },
                    else if (leftTile == 1 && rightTile == 0 && topTile == 1 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 14, x, y);
                    }
                    // { 15, dirtTopBottom },
                    else if (leftTile == 0 && rightTile == 0 && topTile == 1 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 15, x, y);
                    }
                    // { 16, dirtRightTopBottom },
                    else if (leftTile == 0 && rightTile == 1 && topTile == 1 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 16, x, y);
                    }
                    // { 17, dirtTopLeftRight },
                    else if (leftTile == 1 && rightTile == 1 && topTile == 1 && bottomTile == 0)
                    {
                        CreateTileEdge(0, 17, x, y);
                    }
                    // { 18, dirtLeftRight },
                    else if (leftTile == 1 && rightTile == 1 && topTile == 0 && bottomTile == 0)
                    {
                        CreateTileEdge(0, 18, x, y);
                    }
                    // { 19, dirtBottomLeftRight },
                    else if (leftTile == 1 && rightTile == 1 && topTile == 0 && bottomTile == 1)
                    {
                        CreateTileEdge(0, 19, x, y);
                    }
                    // If the 4 sides are dirt, check corners
                    else if (leftTile == 0 && rightTile == 0 && topTile == 0 && bottomTile == 0)
                    {
                        // Check if any corners are bordered by grass
                        int topLeftTile = 1;
                        int topRightTile = 1;
                        int bottomLeftTile = 1;
                        int bottomRightTile = 1;
                        if (!firstRow && !lastColumn) topLeftTile = noiseGrid[x - 1][y + 1];
                        if (!lastRow && !lastColumn) topRightTile = noiseGrid[x + 1][y + 1];
                        if (!firstRow && !firstColumn) bottomLeftTile = noiseGrid[x - 1][y - 1];
                        if (!lastRow && !firstColumn) bottomRightTile = noiseGrid[x + 1][y - 1];
                        // print($"topLeftTile {topLeftTile} topRightTile {topRightTile} bottomLeftTile {bottomLeftTile} bottomRightTile {bottomRightTile}");

                        // { 9, dirtInnerTopLeft },
                        if (topLeftTile == 1 && topRightTile == 0 && bottomLeftTile == 0 && bottomRightTile == 0)
                        {
                            CreateTileEdge(0, 9, x, y);
                        }
                        // { 10, dirtInnerTopRight },
                        else if (topLeftTile == 0 && topRightTile == 1 && bottomLeftTile == 0 && bottomRightTile == 0)
                        {
                            CreateTileEdge(0, 10, x, y);
                        }
                        // { 11, dirtInnerBottomLeft },
                        else if (topLeftTile == 0 && topRightTile == 0 && bottomLeftTile == 1 && bottomRightTile == 0)
                        {
                            CreateTileEdge(0, 11, x, y);
                        }
                        // { 12, dirtInnerBottomRight },
                        else if (topLeftTile == 0 && topRightTile == 0 && bottomLeftTile == 0 && bottomRightTile == 1)
                        {
                            CreateTileEdge(0, 12, x, y);
                        }
                        // { 20, dirtInnerTopLeftAndBottomRight },
                        else if (topLeftTile == 1 && topRightTile == 0 && bottomLeftTile == 0 && bottomRightTile == 1)
                        {
                            CreateTileEdge(0, 20, x, y);
                        }
                        // { 21, dirtInnerTopRightAndBottomLeft },
                        else if (topLeftTile == 0 && topRightTile == 1 && bottomLeftTile == 1 && bottomRightTile == 0)
                        {
                            CreateTileEdge(0, 21, x, y);
                        }
                        // { 22, dirtInnerTop },
                        else if (topLeftTile == 1 && topRightTile == 1 && bottomLeftTile == 0 && bottomRightTile == 0)
                        {
                            print($"inner {topLeftTile} {topRightTile} {bottomLeftTile} {bottomRightTile}");
                            CreateTileEdge(0, 22, x, y);
                        }
                        // { 23, dirtInnerBottom },
                        else if (topLeftTile == 0 && topRightTile == 0 && bottomLeftTile == 1 && bottomRightTile == 1)
                        {
                            print($"inner {topLeftTile} {topRightTile} {bottomLeftTile} {bottomRightTile}");
                            CreateTileEdge(0, 23, x, y);
                        }
                        // { 24, dirtInnerLeft },
                        else if (topLeftTile == 1 && topRightTile == 0 && bottomLeftTile == 1 && bottomRightTile == 0)
                        {
                            print($"inner {topLeftTile} {topRightTile} {bottomLeftTile} {bottomRightTile}");
                            CreateTileEdge(0, 24, x, y);
                        }
                        // { 25, dirtInnerRight },
                        else if (topLeftTile == 0 && topRightTile == 1 && bottomLeftTile == 0 && bottomRightTile == 1)
                        {
                            print($"inner {topLeftTile} {topRightTile} {bottomLeftTile} {bottomRightTile}");
                            CreateTileEdge(0, 25, x, y);
                        }
                    }
                }
            }
        }
    }

    void CreateTileEdge(int tileId, int dirtEdgeTileId, int x, int y)
    {
        /**
           Creates a new tile using the type id code, group it with common tiles,
           set it's position and store the gameObject.
       **/

        GameObject dirtEdgeTilePrefab = dirtEdgeTileset[dirtEdgeTileId];
        GameObject tileGroup = tileGroups[tileId];
        GameObject tile = Instantiate(dirtEdgeTilePrefab, tileGroup.transform);

        // Scale to 16 pixel tiles and center map
        float xUnit = x * unitPixels - mapWidth * unitPixels / 2;
        float yUnit = y * unitPixels - mapHeight * unitPixels / 2;

        tile.name = string.Format("time_x{0}_y{1}_edge", xUnit, yUnit);
        tile.transform.localPosition = new Vector3(xUnit, yUnit, 0);

        tileGrid[x].Add(tile);
    }
}
