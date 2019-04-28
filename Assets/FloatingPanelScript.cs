using Assets.Structs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingPanelScript : MonoBehaviour
{
    public static readonly float GENE_SPACING = .33f;

    public PlayerScript playerScript;
    new public AudioScript audio;
    public PopupTextScript popupTextScript;
    public TextMeshPro uraniumText, currentHeightText, maxHeightText;
    public GameObject genePrefab, mutationProjectilePrefab;
    public GameObject mutationBar;
    List<GameObject> geneObjects;

    Vector2 offset;
    float barX;

    void Start() {
        geneObjects = new List<GameObject>();
        offset = transform.position;
        barX = mutationBar.transform.localPosition.x;
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

        // Add missing gene icons.
        for (int i = geneObjects.Count; i < playerScript.genes.Count; i++) {
            AddGene(i);
        }

        // Update gene icon positions.
        for (int i = 0; i < geneObjects.Count; i++) {
            geneObjects[i].transform.localPosition = Vector3.Lerp(geneObjects[i].transform.localPosition, new Vector3(0, i * GENE_SPACING / 2, 0), .166f);
        }

        // Update height texts.
        currentHeightText.text = YToMeters(playerScript.gameObject.transform.localPosition.y) + "m";
        maxHeightText.text = YToMeters(playerScript.maxY) + "m";

        if (Input.GetKeyDown(KeyCode.Space) || playerScript.mutationMeter >= 1) {
            SpawnMutationProjectile();
            playerScript.mutationMeter = 0;
        }
    }
    void AddGene(int i) {
        Gene gene = playerScript.genes[i];
        GameObject geneObject = Instantiate(genePrefab, transform);
        geneObjects.Insert(i, geneObject);
        geneObject.transform.Translate(0, i * GENE_SPACING, 0);
        foreach (SpriteRenderer geneRenderer in geneObject.GetComponentsInChildren<SpriteRenderer>()) {
            geneRenderer.color = Gene.COLOR_LOOKUP[gene.id];
        }
        TextMeshPro[] texts = geneObject.GetComponentsInChildren<TextMeshPro>();
        texts[0].text = Gene.NAME_LOOKUP[gene.id];
        texts[1].text = Gene.DESCRIPTION_LOOKUP[gene.id];
    }
    int YToMeters(float y) {
        // multiplying by 5 corresponds to the player being 2 meters tall
        return Mathf.RoundToInt(y * 4.375f) - 2;
    }

    void SpawnMutationProjectile() {
        // Select the gene to lose.
        float[] chances = new float[playerScript.genes.Count];
        for (int i = 0; i < chances.Length; i++) {
            chances[i] = playerScript.genes[i].id == GeneID.Armor ? playerScript.genes.Count : 1;
        }
        float totalChance = 0;
        foreach (float f in chances) {
            totalChance += f;
        }
        float selector = Random.value * totalChance;
        int targetIndex = 0;
        for (; targetIndex < chances.Length; targetIndex++) {
            selector -= chances[targetIndex];
            if (selector < 0) {
                break;
            }
        }

        GameObject projectile = Instantiate(mutationProjectilePrefab, transform);
        projectile.GetComponent<MutationLineScript>().SetTarget(targetIndex);
        audio.Mutate();
    }
    public void DestroyGene(int i) {
        GeneID id = playerScript.genes[i].id;
        foreach (Transform child in geneObjects[i].transform) {
            GeneDeathScript geneDeathScript = child.gameObject.GetComponent<GeneDeathScript>();
            if (geneDeathScript != null) {
                geneDeathScript.enabled = true;
            }
        }
        foreach (TextMeshPro textMeshPro in geneObjects[i].GetComponentsInChildren<TextMeshPro>()) {
            Destroy(textMeshPro);
        }
        playerScript.genes.RemoveAt(i);
        geneObjects.RemoveAt(i);
        if (id == GeneID.Junk) {
            playerScript.genes.Insert(i, new Gene(GeneID.DamagedJunk));
            AddGene(i);
        }
        popupTextScript.Display(Gene.NAME_LOOKUP[id] + " gene destroyed.");
    }
}