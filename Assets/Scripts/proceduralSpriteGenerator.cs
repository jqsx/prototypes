using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralSpriteGenerator : MonoBehaviour
{
    public Color[] colors = new Color[0];
    void Start()
    {
        Create();
    }

    private void Create()
    {
        int size = Random.Range(500, 1500);
        Texture2D tex = new Texture2D(size, size);
        tex.filterMode = FilterMode.Point;
        tex.alphaIsTransparency = true;
        Vector2 middle = Vector2.one * size / 2f;
        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                float height = Mathf.PerlinNoise((float)x / 100f, (float)y / 100f);
                float color = Mathf.PerlinNoise((float)-x / 10f, (float)-y / 10f);
                float distance = Vector2.Distance(middle, new Vector2(x, y));
                tex.SetPixel(x, y, new Color(0, 0, 0, 0));
                if (distance < size / 2f)
                {
                    float alpha = height * (size / 2 - distance) / size * 2 > 0.1f ? 1 : 0f;
                    Color current = colors[Random.Range(0, colors.Length)];
                    current.a = alpha;
                    tex.SetPixel(x, y, current);
                }
            }
        }
        tex.Apply();

        GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(Vector2.zero, Vector2.one * size), Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Create();
        }
    }
}
