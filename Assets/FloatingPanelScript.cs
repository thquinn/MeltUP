using Assets.Structs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingPanelScript : MonoBehaviour
{
    static Dictionary<GeneID, Color> GENE_COLOR_LOOKUP = new Dictionary<GeneID, Color>() {
        { GeneID.Speed, new Color(1, .8f, .8f) },
        { GeneID.Jump, new Color(.85f, .95f, 1) },
        { GeneID.JumpControl, new Color(.95f, 1, .9f) },
        { GeneID.AirControl, new Color(.95f, .85f, 1) },
    };
    static Dictionary<GeneID, string> GENE_NAME_LOOKUP = new Dictionary<GeneID, string>() {
        { GeneID.Speed, "Speed" },
        { GeneID.Jump, "Jump" },
        { GeneID.JumpControl, "Jump Control" },
        { GeneID.AirControl, "Air Control" },
    };
    static Dictionary<GeneID, string> GENE_DESCRIPTION_LOOKUP = new Dictionary<GeneID, string>() {
        { GeneID.Speed, "The more you have, the faster you move." },
        { GeneID.Jump, "Determines how many jumps you have." },
        { GeneID.JumpControl, "Allows you to control the height of your jump by holding the button." },
        { GeneID.AirControl, "Allows you to control your movement in the air." },
    };
    public static readonly float GENE_SPACING = .33f;

    public PlayerScript playerScript;
    public TextMeshPro uraniumText;
    public GameObject genePrefab, mutationProjectilePrefab;
    public GameObject mutationBar;
    List<GameObject> geneObjects;

    Vector2 offset;
    float barX;

    void Start() {
        geneObjects = new List<GameObject>();
        offset = transform.position;
        barX = mutationBar.transform.localPosition.x;
        for (int i = 0; i < playerScript.genes.Count; i++) {
            Gene gene = playerScript.genes[i];
            GameObject geneObject = Instantiate(genePrefab, transform);
            geneObjects.Add(geneObject);
            geneObject.transform.Translate(0, i * GENE_SPACING, 0);
            foreach (SpriteRenderer geneRenderer in geneObject.GetComponentsInChildren<SpriteRenderer>()) {
                geneRenderer.color = GENE_COLOR_LOOKUP[gene.id];
            }
            TextMeshPro[] texts = geneObject.GetComponentsInChildren<TextMeshPro>();
            texts[0].text = GENE_NAME_LOOKUP[gene.id];
            texts[1].text = GENE_DESCRIPTION_LOOKUP[gene.id];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(Camera.main.transform.position.x + offset.x, Camera.main.transform.position.y + offset.y, transform.position.z);
        uraniumText.text = playerScript.uranium.ToString();
        float width = playerScript.mutationMeter * 117;
        float x = barX + width / 200f;
        mutationBar.transform.localPosition = new Vector3(x, mutationBar.transform.localPosition.y);
        mutationBar.transform.localScale = new Vector3(width, mutationBar.transform.localScale.y, 1);

        // Update gene icon positions.
        for (int i = 0; i < geneObjects.Count; i++) {
            geneObjects[i].transform.localPosition = Vector3.Lerp(geneObjects[i].transform.localPosition, new Vector3(0, i * GENE_SPACING / 2, 0), .166f);
        }

        if (Input.GetKeyDown(KeyCode.Space) || playerScript.mutationMeter >= 1) {
            SpawnMutationProjectile();
            playerScript.mutationMeter = 0;
        }
    }

    void SpawnMutationProjectile() {
        GameObject projectile = Instantiate(mutationProjectilePrefab, transform);
        projectile.GetComponent<MutationLineScript>().SetTarget(Random.Range(0, playerScript.genes.Count));
    }
    public void DestroyGene(int i) {
        foreach (Transform child in geneObjects[i].transform) {
            GeneDeathScript geneDeathScript = child.gameObject.GetComponent<GeneDeathScript>();
            if (geneDeathScript != null) {
                geneDeathScript.enabled = true;
            }
        }
        playerScript.genes.RemoveAt(i);
        geneObjects.RemoveAt(i);
    }
}