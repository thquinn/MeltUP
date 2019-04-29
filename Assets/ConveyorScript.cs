using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    GameObject gearbox;
    float[] rotations = new float[] { -10, 4, -8 };

    // Start is called before the first frame update
    void Start()
    {
        Combine(-1);
        Combine(1);
        gearbox = transform.GetChild(0).gameObject;
    }

    void Combine(int direction) {
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        if (myCollider == null) {
            return;
        }

        int LAYER_MASK_ENVIRONMENT = 1 << LayerMask.NameToLayer("Environment");
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right * direction, .2f);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider == myCollider || hit.collider.gameObject.tag != "Conveyor") {
                continue;
            }
            BoxCollider2D otherCollider = (BoxCollider2D)hit.collider;
            Debug.Assert(myCollider != otherCollider);
            myCollider.offset += new Vector2(otherCollider.size.x / 2 * direction, 0);
            myCollider.size += new Vector2(otherCollider.size.x, 0);
            DestroyImmediate(otherCollider);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gearbox.transform.Rotate(0, 0, rotations[0]);
        gearbox.transform.GetChild(0).Rotate(0, 0, rotations[1]);
        gearbox.transform.GetChild(1).Rotate(0, 0, rotations[2]);
    }

    public void Flip() {
        tag = "ConveyorLeft";
        for (int i = 0; i < rotations.Length; i++) {
            rotations[i] = -rotations[i];
        }
    }
}
