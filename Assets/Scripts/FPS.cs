using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private TMPro.TMP_Text textDisplay;
    private List<float> fpsAvg = new List<float>();
    void Awake()
    {
        textDisplay = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fpsAvg.Count >= 10)
        {
            float avg = 0f;
            foreach(float a in fpsAvg)
            {
                avg += a;
            }
            avg /= fpsAvg.Count;
            textDisplay.text = "FPS: " + Mathf.Floor(avg);
            fpsAvg.Clear();
        }
        else
        {
            fpsAvg.Add(1f / Time.deltaTime);
        }
    }
}
