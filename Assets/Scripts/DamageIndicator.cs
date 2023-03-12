using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public TMPro.TMP_Text text;
    float time = 0f;
    private void Start()
    {
        time = Time.time;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    void Update()
    {
        if (time + 5f < Time.time)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position += new Vector3(0, 0.5f) * Time.deltaTime;
        }
    }

    public void setup(float amount)
    {
        text.text = amount.ToString();
    }
}
