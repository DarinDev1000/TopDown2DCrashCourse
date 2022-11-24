using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile[] tiles;
    public RuleTile[] ruleTiles;
    public List<GameObject> UITiles;

    public List<TileBase> _allTiles = new();

    public int selectedTile = 0;

    public Transform tileGridUI;

    private void Start()
    {

        int i = 0;
        foreach (Tile tile in tiles)
        {
            _allTiles.Add(tile);
            print(tile.GetType());

            GameObject UITile = new GameObject("UI Tile");
            UITile.transform.parent = tileGridUI;
            UITile.transform.localScale = new Vector3(1f, 1f, 1f);

            Image UIImage = UITile.AddComponent<Image>();
            UIImage.sprite = tile.sprite;

            Color tileColor = UIImage.color;
            tileColor.a = 0.5f;

            if (i == selectedTile)
            {
                tileColor.a = 1f;
            }
            UIImage.color = tileColor;

            UITiles.Add(UITile);

            i++;
        }

        int j = i;
        foreach (RuleTile tile in ruleTiles)
        {
            _allTiles.Add(tile);
            print(tile.GetType());

            GameObject UITile = new GameObject("UI Tile");
            UITile.transform.parent = tileGridUI;
            UITile.transform.localScale = new Vector3(1f, 1f, 1f);

            Image UIImage = UITile.AddComponent<Image>();
            UIImage.sprite = tile.m_DefaultSprite;

            Color tileColor = UIImage.color;
            tileColor.a = 0.5f;

            if (j == selectedTile)
            {
                tileColor.a = 1f;
            }
            UIImage.color = tileColor;

            UITiles.Add(UITile);

            j++;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTile = 0;
            RenderUITiles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTile = 1;
            RenderUITiles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTile = 2;
            RenderUITiles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedTile = 3;
            RenderUITiles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedTile = 4;
            RenderUITiles();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedTile = 5;
            RenderUITiles();
        }

        // Place tile on left click
        if (Input.GetMouseButtonDown(0))
        {
            // Change mouse screen location to world location
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Create the tile on click
            // tilemap.SetTile(tilemap.WorldToCell(position), tiles[selectedTile]);
            tilemap.SetTile(tilemap.WorldToCell(position), (TileBase)_allTiles[selectedTile]);
        }
        // Remove tile on right click
        if (Input.GetMouseButtonDown(1))
        {
            // Change mouse screen location to world location
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Create the tile on click
            tilemap.SetTile(tilemap.WorldToCell(position), null);
        }
    }

    void RenderUITiles()
    {
        int i = 0;
        foreach (GameObject tile in UITiles)
        {
            Image UIImage = tile.GetComponent<Image>();
            Color tileColor = UIImage.color;
            tileColor.a = 0.5f;

            if (i == selectedTile)
            {
                tileColor.a = 1f;
            }
            UIImage.color = tileColor;

            i++;
        }
    }
}
