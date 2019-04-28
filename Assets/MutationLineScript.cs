using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationLineScript : MonoBehaviour
{
    float dx, dy;
    int frames;
    LineRenderer lineRenderer;
    float targetX, targetY;
    List<Vector3> positions;
    public Material lineMaterial;
    int i;
    bool destroyed;

    // Start is called before the first frame update
    void Start()
    {
        dx = .4f;
        dy = .025f;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        targetX = -1f;
        positions = new List<Vector3> {
            new Vector3(1.75f, -.2f, 0)
        };
    }
    public void SetTarget(int i) {
        this.i = i;
        targetY = FloatingPanelScript.GENE_SPACING * i / 2;
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        if (frames > 100) {
            Destroy(gameObject);
            return;
        }
        Vector3 lastPoint = positions[positions.Count - 1];
        float t = Mathf.Min(frames * .02f, 1);
        float x = Mathf.Lerp(lastPoint.x + dx, targetX, t);
        float y = Mathf.Lerp(lastPoint.y + dy, targetY, Mathf.Min(t * 2, 1));
        positions.Add(new Vector3(x, y, 0));
        if (positions.Count > 15) {
            positions.RemoveAt(0);
        }
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        if (x <= 0 && !destroyed) {
            transform.parent.gameObject.GetComponent<FloatingPanelScript>().DestroyGene(i);
            destroyed = true;
        }
    }
}
