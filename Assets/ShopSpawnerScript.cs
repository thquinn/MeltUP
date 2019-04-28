using Assets.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawnerScript : MonoBehaviour
{
    int noLeaveTimer;

    PlayerScript playerScript;
    public GameObject shopGenePrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        // Pick 3 weighted genes.
        GeneID[] ids = new List<GeneID>(Gene.SHOP_WEIGHT_LOOKUP.Keys).ToArray();
        float[] weights = new float[ids.Length];
        for (int i = 0; i < ids.Length; i++) {
            GeneID id = ids[i];
            float baseWeight = Gene.SHOP_WEIGHT_LOOKUP[id];
            // No repeat powers.
            if (id == GeneID.Blink || id == GeneID.Glide) {
                weights[i] = playerScript.HasGene(id) ? 0 : baseWeight;
                continue;
            }
            if (id == GeneID.DoubleJump) {
                weights[i] = baseWeight / (1 + playerScript.NumGene(GeneID.DoubleJump));
                continue;
            }
            weights[i] = baseWeight;
        }
        float totalWeight = 0;
        foreach (float f in weights) {
            totalWeight += f;
        }
        List<GeneID> chosen = new List<GeneID>();
        while (chosen.Count < 3) {
            float selector = Random.value * totalWeight;
            int targetIndex = 0;
            for (; targetIndex < weights.Length; targetIndex++) {
                selector -= weights[targetIndex];
                if (selector < 0) {
                    break;
                }
            }
            GeneID id = ids[targetIndex];
            if (chosen.Contains(id)) {
                continue;
            }
            chosen.Add(id);
        }

        float spacing = 2.25f / chosen.Count;
        for (int i = 0; i < chosen.Count; i++) {
            float xMult = i - (chosen.Count - 1) / 2;
            GameObject shopGene = Instantiate(shopGenePrefab, transform.parent, false);
            shopGene.transform.localPosition = new Vector3(xMult * spacing, -2);
            ShopGeneScript shopGeneScript = shopGene.GetComponent<ShopGeneScript>();
            shopGeneScript.SetID(chosen[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        noLeaveTimer = Mathf.Max(0, noLeaveTimer - 1);
    }

    public void Enter(PlayerScript playerScript) {
        foreach (Transform child in transform.parent) {
            if (!child.gameObject.activeSelf && child.localPosition.y < -1.5f) {
                child.gameObject.SetActive(true);
            }
        }
        noLeaveTimer = 60;
        playerScript.deathY = transform.parent.localPosition.y - 6f;
    }
    public void Leave(PlayerScript playerScript) {
        if (noLeaveTimer > 0) {
            return;
        }
        playerScript.lastSafePosition = transform.parent.localPosition;
        foreach (Transform child in transform.parent) {
            if (!child.gameObject.activeSelf) {
                child.gameObject.SetActive(true);
            }
        }
        GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGeneratorScript>().CleanupChunks(transform.parent.gameObject);
        Destroy(gameObject);
    }
}
