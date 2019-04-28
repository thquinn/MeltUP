using Assets.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawnerScript : MonoBehaviour
{
    int noLeaveTimer;

    public GameObject shopGenePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject shopGene = Instantiate(shopGenePrefab, transform.parent, false);
        shopGene.transform.localPosition = new Vector3(0, -2);
        ShopGeneScript shopGeneScript = shopGene.GetComponent<ShopGeneScript>();
        shopGeneScript.SetID(GeneID.DoubleJump);
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
