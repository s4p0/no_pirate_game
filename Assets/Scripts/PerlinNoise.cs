using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PerlinNoise : MonoBehaviour
{
    [Serializable]
    public struct PerlinCondition
    {
        [SerializeField]
        public float min;
        [SerializeField]
        public float max;
        public UnityEvent<int, int> dispatch;
    }

    [SerializeField]
    public List<PerlinCondition> perlinConditions = new List<PerlinCondition>();

    private float[] grid;

    [SerializeField] private int randomSeed = 0;
    [SerializeField] private float randomOffsetX = 0f;
    [SerializeField] private float randomOffsetY = 0f;


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
        UnityEngine.Random.InitState(randomSeed);
        randomOffsetX = UnityEngine.Random.value * 100;
        randomOffsetY = UnityEngine.Random.value * 100;


        Clear();

        var texture = new Texture2D(width , height  );

        for (int x = 0; x < width  ; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var value = CalculatePerlinNoise(x, y);

                foreach (var condition in perlinConditions)
                {

                    if ( value >= condition.min &&  value <= condition.max)
                        if (condition.dispatch != null)
                            condition.dispatch.Invoke(x, y);
                    
                }
            }
        }
    }

    public void AddTileSimpleWater(int x, int y)
    {
        onlyWaterTileMap.SetTile(new Vector3Int(x, y, 0), simpleWater);
    }

    public void AddTileWater(int x, int y)
    {
        deepWaterTileMap.SetTile(new Vector3Int(x, y, 0), water);
    }

    public void AddTileGrass(int x, int y)
    {
        grassTileMap.SetTile(new Vector3Int(x, y, 0), grass);
    }

    public void AddTileTerrain(int x, int y)
    {
        groundTileMap.SetTile(new Vector3Int(x, y, 0), terrain);
    }

    private float CalculatePerlinNoise(int x, int y)
    {
        float xCoord = (float)x / width * scale + offset.x + randomOffsetX;
        float yCoord = (float)y / height * scale + offset.y + randomOffsetY;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}
