using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupTextScript : MonoBehaviour
{
    static readonly int FADE_TIME = 30, STAY_TIME = 240;

    TextMeshPro text;
    int timer;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer == 0) {
            text.color = new Color(1, 1, 1, 0);
            return;
        }
        Color color = text.color;
        float a;
        if (timer > STAY_TIME + FADE_TIME) {
            a = 1 - (timer - STAY_TIME - FADE_TIME) / (float)FADE_TIME;
        } else if (timer > FADE_TIME) {
            a = 1;
        } else {
            a = timer / (float)FADE_TIME;
        }
        color.a = a;
        text.color = color;
        timer = Mathf.Max(0, timer - 1);
    }

    public void Display(string s) {
        text.text = s;
        timer = STAY_TIME + 2 * FADE_TIME;
        text.color = new Color(1, 1, 1, 0);
    }
}
