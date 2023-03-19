using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public TMPro.TMP_Text text;
    float time = 0f;
    private bool smoothDisplay = false;
    private bool isString = false;
    public float amount = 0f;
    private float displayAmount = 0f;

    private string prefix = "";
    private string suffix = "";

    private void Start()
    {
        time = Time.time;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    void Update()
    {
        if (time + 1f < Time.time)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position += new Vector3(0, 0.5f) * Time.deltaTime;
            displayAmount = Mathf.Lerp(displayAmount, amount, Time.deltaTime * 20f);
            if (smoothDisplay && !isString) text.text = suffix + (Mathf.Round(displayAmount * 100f) / 100f).ToString() + prefix;
        }
    }

    public DamageIndicator setup(float amount)
    {
        isString = false;
        this.amount = amount;
        text.text = suffix + amount.ToString() + prefix;
        return this;
    }


    /// <summary>
    /// If you want to set suffix or prefix do that before doing this yeah. cos this puts all of it together and the text isnt updated in the update function<br></br>
    /// This also works with rich text
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public DamageIndicator setText(string txt)
    {
        text.text = suffix + txt + prefix;
        isString = true;

        return this;
    }


    public DamageIndicator setColor(Color color)
    {
        text.color = color;
        return this;
    }

    public DamageIndicator setSmooth(bool isSmooth)
    {
        smoothDisplay = isSmooth;
        text.text = suffix + amount.ToString() + prefix;
        return this;
    }

    public DamageIndicator setPrefix(string prefix)
    {
        this.prefix = prefix;
        return this;
    }

    public DamageIndicator setSuffix(string suffix)
    {
        this.suffix = suffix;
        return this;
    }

    public DamageIndicator parseDamageTextAsColors()
    {
        string constructed = "";
        string t = text.text;
        for(int i = 0; i < t.Length; i++)
        {
            constructed += "<color=" + Color.Lerp(Color.white, Color.red, i / t.Length).GetHashCode().ToString() + ">" + t[i];
        }

        text.text = constructed;

        return this;
    }
}
