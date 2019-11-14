using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] List<TileVisualizer> tiles = null;
    private float originalTileSize = 4;
    private float mapSize = 5;

    public void ShowTiles(int rows, int columns)
    {
        int tileSize = 0;
        if (rows > columns)
        {
            tileSize = rows;
        }
        else
        {
            tileSize = columns;
        }
        foreach(TileVisualizer tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }
        for(int i=0; i<rows; i++)
        {
            for(int j=0; j < columns; j++)
            {
                int index = j + i * columns;
                tiles[index].gameObject.SetActive(true);
                tiles[index].transform.localScale = new Vector3(originalTileSize / tileSize, originalTileSize / tileSize, originalTileSize / tileSize);
                float posX = -mapSize + mapSize / tileSize * (j + 1) + mapSize / tileSize * j;
                if (rows > columns)
                {
                    posX += mapSize - mapSize * columns / rows;
                }
                float posY = mapSize - mapSize / tileSize * (i + 1) - mapSize / tileSize * i;
                if (columns > rows)
                {
                    posY -= mapSize - mapSize * rows / columns;
                }
                tiles[index].transform.position = new Vector3(posX, posY);
            }
        }
    }
}
