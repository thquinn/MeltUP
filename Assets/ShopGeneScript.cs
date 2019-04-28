using Assets.Structs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopGeneScript : MonoBehaviour
{
    public GameObject spriteObject;
    public TextMeshPro nameText, priceText;
    GeneID id;
    int price;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetID(GeneID id) {
        this.id = id;
        nameText.text = Gene.NAME_LOOKUP[id];
        foreach (SpriteRenderer spriteRenderer in spriteObject.GetComponentsInChildren<SpriteRenderer>()) {
            spriteRenderer.color = Gene.COLOR_LOOKUP[id];
        }
        int[] prices = Gene.PRICE_LOOKUP[id];
        price = Random.Range(prices[0], prices[1] + 1);
        priceText.text = price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.frameCount * .033f) * .0125f;
        spriteObject.transform.localPosition = new Vector3(0, y, 0);
    }

    public void Buy(PlayerScript playerScript) {
        if (playerScript.uranium < price) {
            return;
        }
        playerScript.uranium -= price;
        playerScript.genes.Add(new Gene(id));
        if (id == GeneID.DoubleJump) {
            playerScript.jumps++;
        }
        Destroy(gameObject);
    }
}
