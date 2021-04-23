using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoise : MonoBehaviour
{
    public enum ConditionTileMap
    {
        GRASS,
        GROUND,
        WATER,
        DEEPWATER
    }

    public enum ConditionOperation
    {
        LESS,
        GREATER
    }

    public class Condition
    {
        public ConditionTileMap TileMap;
        public ConditionOperation Operation;

    }


    [SerializeField] private Condition[] conditions = new Condition[2];
    private float[] grid;

    [SerializeField]private int height = 256;
    [SerializeField]private int width = 256;
    [SerializeField] public float scale = 20f;

    [SerializeField] private Vector2 offset = Vector2.one;
    
    [SerializeField] private Tilemap grassTileMap;
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Tilemap waterTileMap;
    [SerializeField] private Tilemap deepWaterTileMap;
    [SerializeField] private Tilemap onlyWaterTileMap;

    [SerializeField] private TileBase simpleWater;
    [SerializeField] private TileBase water;
    [SerializeField] private TileBase deepWater;
    [SerializeField] private TileBase terrain;
    [SerializeField] private TileBase grass;

    private void Start()
    {
        grid = new float[width * height];
    }

    public void Clear()
    {
        grassTileMap.ClearAllTiles();
        groundTileMap.ClearAllTiles();
        waterTileMap.ClearAllTiles();
        deepWaterTileMap.ClearAllTiles();
        onlyWaterTileMap.ClearAllTiles();
    }

    public void GenerateTexture()
    {
        Clear();

        var texture = new Texture2D(width , height  );

        for (int x = 0; x < width  ; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var value = CalculatePerlinNoise(x, y);
                
                if (value < .7f)
                    onlyWaterTileMap.SetTile(new Vector3Int(x, y, 0), simpleWater);

                //if (value <= .2f && value > .1f)
                //    deepWaterTileMap.SetTile(new Vector3Int(x, y, 0), deepWater);

                //if (value < .5f && value > .2f)
                //    waterTileMap.SetTile( new Vector3Int(x, y, 0), water);

                //if(value > .39f)
                //    groundTileMap.SetTile(new Vector3Int(x, y, 0), terrain);

                if (value < .1f)
                    deepWaterTileMap.SetTile(new Vector3Int(x, y, 0), water);

                if (value > .5f)
                    waterTileMap.SetTile(new Vector3Int(x, y, 0), water);

                if (value >= .6f)
                    grassTileMap.SetTile(new Vector3Int(x, y, 0), grass);

                if (value >= .85f)
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), terrain);

            }
        }

    }

    private float CalculatePerlinNoise(int x, int y)
    {
        float xCoord = (float)x / width * scale + offset.x;
        float yCoord = (float)y / height * scale + offset.y;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}
