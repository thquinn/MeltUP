using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorScript : MonoBehaviour
{
    static Dictionary<Color, TileType> COLOR_TO_TILE = new Dictionary<Color, TileType>() {
        { Color.black, TileType.Ground },
        { new Color32( 0x80, 0xD0, 0xFF, 0xFF ), TileType.Teflon },
        { new Color32( 0x00, 0x00, 0xFF, 0xFF ), TileType.Conveyor },
        { new Color32( 0x00, 0x00, 0x80, 0xFF ), TileType.ConveyorLeft },
        { Color.green, TileType.Grate },
        { new Color32( 0xFF, 0xD0, 0x00, 0xFF ), TileType.Uranium },
    };

    public GameObject shopSpawnerPrefab;
    public GameObject groundPrefab, teflonPrefab, conveyorPrefab, gratePrefab, uraniumPrefab;
    public Dictionary<TileType, GameObject> TILE_TO_PREFAB;

    public Texture2D chunksTexture;
    List<TileType[,]> chunks;
    List<GameObject> activeChunks;
    float nextY;
    bool flip;

    // Start is called before the first frame update
    void Start()
    {
        // Load prefabs.
        TILE_TO_PREFAB = new Dictionary<TileType, GameObject>() {
            { TileType.Ground, groundPrefab },
            { TileType.Teflon, teflonPrefab },
            { TileType.Conveyor, conveyorPrefab },
            { TileType.ConveyorLeft, conveyorPrefab },
            { TileType.Grate, gratePrefab },
            { TileType.Uranium, uraniumPrefab },
        };
        // Load chunks.
        chunks = new List<TileType[,]>();
        for (int chunkX = 0; chunkX < chunksTexture.width / 32; chunkX++) {
            for (int chunkY = 0; chunkY < chunksTexture.height / 16; chunkY++) {
                if (GetPixel(chunkX * 32, chunkY * 16) != Color.magenta) {
                    continue;
                }
                int height = 1;
                for (int dy = 1; chunkY + dy < chunksTexture.height / 16; dy++) {
                    Color dColor = GetPixel(chunkX * 32, (chunkY + dy) * 16);
                    if (dColor != Color.magenta && dColor != Color.white) {
                        height++;
                    } else {
                        break;
                    }
                }
                TileType[,] c = new TileType[32, 16 * height];
                for (int x = 0; x < 32; x++) {
                    for (int y = 0; y < 16 * height; y++) {
                        c[x, y] = GetTile(chunkX * 32 + x, chunkY * 16 + y);
                    }
                }
                chunks.Add(c);
            }
        }
        activeChunks = new List<GameObject>();
        SpawnNewChunk(0);
        SpawnNewChunk(1);
        SpawnNewChunk(2);
        SpawnNewChunk(3);
        SpawnNewChunk(4);
    }
    Color GetPixel(int x, int y) {
        return chunksTexture.GetPixel(x, chunksTexture.height - y - 1);
    }
    TileType GetTile(int x, int y) {
        Color color = GetPixel(x, y);
        if (COLOR_TO_TILE.ContainsKey(color)) {
            return COLOR_TO_TILE[color];
        }
        return TileType.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnNewChunk() {
        SpawnNewChunk(Random.Range(2, chunks.Count));
    }
    void SpawnNewChunk(int index) {
        TileType[,] chunk = chunks[index];
        GameObject chunkObject = new GameObject();
        nextY += chunk.GetLength(1) * .2f;
        chunkObject.transform.Translate(0, nextY, 0);
        if (index == 1) {
            Instantiate(shopSpawnerPrefab, chunkObject.transform);
        }
        for (int tx = 0; tx < chunk.GetLength(0); tx++) {
            for (int ty = 0; ty < chunk.GetLength(1); ty++) {
                if (chunk[tx, ty] == TileType.None) {
                    continue;
                }
                float x = (tx - chunk.GetLength(0) / 2) * .2f;
                if (flip) {
                    x *= -1;
                }
                float y = -ty * .2f;
                TileType tileType = chunk[tx, ty];
                GameObject tile = Instantiate(TILE_TO_PREFAB[tileType], chunkObject.transform);
                tile.transform.Translate(x, y, 0);
                if ((tileType == TileType.Conveyor || tileType == TileType.ConveyorLeft) && flip == (tileType == TileType.ConveyorLeft)) {
                    tile.GetComponent<ConveyorScript>().Flip();
                }
            }
        }
        flip = !flip;
    }
}

public enum TileType {
    None, Ground, Teflon, Conveyor, ConveyorLeft, Grate, Uranium
}
