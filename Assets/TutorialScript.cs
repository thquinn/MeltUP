using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour {
    static readonly string[] TUTORIAL_TEXT = new string[] {
        "Welcome to MeltUP! Press X to keep reading, or just go off and figure it out on your own.",
        "The nuclear plant is melting down! Climb this cooling tower and make your escape. Here's how.",
        "The basics: arrows keys to move, Z to jump.",
        "Gave that a try? Okay. On to more complicated stuff.",
        "All those things on the left are <color=#FAC>genes.</color> They affect how your character behaves. Hold Shift to see their names and descriptions.",
        "The meter below is the <color=#0F0>radiation meter.</color> Every time it fills up, one of your genes will be destroyed.",
        "Climb as fast as you can, collecting <color=#00DD73>uranium</color> to buy extra genes to make up for the ones you've lost.",
        "But remember: uranium is <color=#0F0>radioactive.</color> The more you carry, the faster your radiation meter will fill.",
        "Okay, time to get going. <color=#F00>Clock's ticking!</color>"
    };

    TextMeshPro text;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = TUTORIAL_TEXT[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Camera.main.transform.localPosition.y > 1.5f) {
            Destroy(gameObject);
            return;
        }

        Vector3 pos = transform.localPosition;
        pos.x = Camera.main.transform.localPosition.x;
        transform.localPosition = pos;

        if (Input.GetButtonDown("Special")) {
            i = Mathf.Min(i + 1, TUTORIAL_TEXT.Length - 1);
            text.text = TUTORIAL_TEXT[i];
        }
        Color color = text.color;
        color.a = Mathf.Clamp01(text.color.a + (Input.GetKey(KeyCode.LeftShift) ? -.1f : .1f));
        text.color = color;
    }

    public bool Done() {
        return i == TUTORIAL_TEXT.Length - 1;
    }
}
