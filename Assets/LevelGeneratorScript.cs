using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorScript : MonoBehaviour
{
    static Dictionary<Color, TileType> COLOR_TO_TILE = new Dictionary<Color, TileType>() {
        { Color.black, TileType.Ground },
        { new Color32( 0x80, 0xD0, 0xFF, 0xFF ), TileType.Teflon },
        { new Color32( 0x80, 0x60, 0x00, 0xFF ), TileType.Dirt },
        { new Color32( 0x00, 0x00, 0xFF, 0xFF ), TileType.Conveyor },
        { new Color32( 0x00, 0x00, 0x80, 0xFF ), TileType.ConveyorLeft },
        { new Color32( 0xFF, 0x00, 0x00, 0xFF ), TileType.Spring },
        { Color.green, TileType.Grate },
        { new Color32( 0xFF, 0xD0, 0x00, 0xFF ), TileType.Uranium },
        { new Color32( 0x80, 0x80, 0x80, 0xFF ), TileType.Hidden },
    };

    public GameObject shopSpawnerPrefab;
    public GameObject groundPrefab, teflonPrefab, dirtPrefab, conveyorPrefab, springPrefab, gratePrefab, uraniumPrefab, hiddenPrefab;
    public GameObject[] backplatePrefabs, backbackplatePrefabs;
    public Dictionary<TileType, GameObject> TILE_TO_PREFAB;
    Camera cam;

    public Texture2D chunksTexture;
    List<TileType[,]> chunks;
    List<GameObject> activeChunks;
    List<int> chunkBag;
    List<GameObject>[] backplateses;
    float nextY;
    bool flip;
    int sinceLastCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        // Load prefabs.
        TILE_TO_PREFAB = new Dictionary<TileType, GameObject>() {
            { TileType.Ground, groundPrefab },
            { TileType.Teflon, teflonPrefab },
            { TileType.Dirt, dirtPrefab },
            { TileType.Conveyor, conveyorPrefab },
            { TileType.ConveyorLeft, conveyorPrefab },
            { TileType.Spring, springPrefab },
            { TileType.Grate, gratePrefab },
            { TileType.Uranium, uraniumPrefab },
            { TileType.Hidden, hiddenPrefab },
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
        //SpawnNewChunk(1);
        chunkBag = new List<int>();

        backplateses = new List<GameObject>[2];
        backplateses[0] = new List<GameObject>();
        backplateses[1] = new List<GameObject>();
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
        // Refill chunk bag.
        if (chunkBag.Count <= 2) {
            List<int> tempChunks = new List<int>();
            for (int i = 2; i < chunks.Count; i++) {
                tempChunks.Add(i);
            }
            while (true) {
                tempChunks.Shuffle();
                if (chunkBag.Count > 0) {
                    int lastID = chunkBag[chunkBag.Count - 1];
                    if (tempChunks[0] == lastID || tempChunks[1] == lastID) {
                        continue;
                    }
                }
                if (chunkBag.Count > 1) {
                    int secondLastID = chunkBag[chunkBag.Count - 2];
                    if (tempChunks[0] == secondLastID || tempChunks[1] == secondLastID) {
                        continue;
                    }
                }
                break;
            }
            chunkBag.AddRange(tempChunks);
        }
        // Spawn new chunks if necessary.
        float topChunkY = activeChunks[activeChunks.Count - 1].transform.localPosition.y;
        if (topChunkY - cam.transform.localPosition.y < 4) {
            if (sinceLastCheckpoint == 4) {
                SpawnNewChunk(1);
                sinceLastCheckpoint = 0;
            } else {
                SpawnNewChunk();
                sinceLastCheckpoint++;
            }
        }

        // Backplates.
        for (int i = 0; i < backplateses.Length; i++) {
            List<GameObject> backplates = backplateses[i];
            if (backplates.Count > 0) {
                Vector3 screenPos = backplates[0].transform.localPosition - cam.transform.localPosition;
                if (screenPos.y < -20) {
                    Destroy(backplates[0]);
                    backplates.RemoveAt(0);
                }
            }
            bool newBackplate = backplates.Count == 0;
            if (!newBackplate) {

                Vector3 screenPos = backplates[backplates.Count - 1].transform.localPosition - cam.transform.localPosition;
                newBackplate = screenPos.y < -2;
            }
            if (newBackplate) {
                GameObject[] prefabs = i == 0 ? backplatePrefabs : backbackplatePrefabs;
                int prefabIndex;
                if (i == 1) {
                    prefabIndex = Mathf.Min(backplates.Count, 1);
                } else if (backplates.Count == 0) {
                    do {
                        prefabIndex = Random.Range(0, prefabs.Length);
                    } while (prefabIndex == 2);
                } else {
                    prefabIndex = Random.Range(0, prefabs.Length);
                }
                GameObject backplateObject = Instantiate(prefabs[prefabIndex]);
                backplateObject.transform.parent = transform.parent;
                float y = backplates.Count == 0 ? (i == 0 ? 0 : -2.66f) : backplates[backplates.Count - 1].GetComponent<BackplateScript>().originalPos.y - 18.25f;
                backplateObject.transform.localPosition = new Vector3(0, i == 0 ? y : y * .55f, 10 * (i + 1));
                SpriteRenderer backplateRenderer = backplateObject.GetComponent<SpriteRenderer>();
                if (i == 0) {
                    backplateRenderer.flipX = Random.value < .5f;
                    backplateRenderer.flipY = Random.value < .5f;
                }
                backplateObject.transform.localScale = new Vector3(5, 5);
                backplateObject.AddComponent<BackplateScript>().multiplier = i == 0 ? .5f : .975f;
                backplates.Add(backplateObject);
            }
        }
    }
    void SpawnNewChunk() {
        int chunkID = chunkBag[0];
        chunkBag.RemoveAt(0);
        SpawnNewChunk(chunkID);
    }
    void SpawnNewChunk(int index) {
        TileType[,] chunk = chunks[index];
        GameObject chunkObject = new GameObject("Chunk");
        activeChunks.Add(chunkObject);
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

    public void CleanupChunks(GameObject upTo) {
        int i = activeChunks.IndexOf(upTo);
        for (int j = 0; j < i; j++) {
            Destroy(activeChunks[j]);
        }
        activeChunks.RemoveRange(0, i);
    }
}

public enum TileType {
    None, Ground, Teflon, Dirt, Conveyor, ConveyorLeft, Spring, Grate, Uranium, Hidden
}
