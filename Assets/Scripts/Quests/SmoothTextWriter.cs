using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTextWriter : MonoBehaviour
{
    float startTime = 0f;
    public float speed = 3f;
    [TextArea]
    public string text = "";
    public TMPro.TMP_Text output;
    void Awake()
    {
        startTime = Time.time;
    }

    void Update()
    {
        int index = (int)Mathf.Clamp(Mathf.Round((Time.time - startTime) / speed * text.Length), 0, text.Length - 1);
        output.text = text.Substring(0, index);
    }
}
