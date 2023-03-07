using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class wallGeneration : MonoBehaviour
{
    SpriteShapeController controller;
    Vector2 seed;
    public List<Vector3> targets = new List<Vector3>();
    void Start()
    {

    }

    public void GenerateShape(Vector3 position, Vector2 seed)
    {
        this.seed = seed;
        transform.position = Vector3.zero;
        StartCoroutine(setupPoints(position, position, 0));
    }

    void CreateWalls(Vector3 position)
    {
        controller = GetComponent<SpriteShapeController>();
        controller.spline.Clear();
        for (int i = 0; i < targets.Count; i++)
        {
            int j = targets.Count - i - 1;

            try
            {
                controller.spline.InsertPointAt(i, targets[j]);
                controller.spline.SetRightTangent(i, transform.rotation * Vector3.up);
                controller.spline.SetRightTangent(i, transform.rotation * Vector3.down);
            } catch {
                Destroy(gameObject);
                return; 
            }
        }
        controller.RefreshSpriteShape();
    }

    IEnumerator setupPoints(Vector3 testingPosition, Vector3 originalPosition, int length)
    {
        float _x = testingPosition.x / 5f;
        float _y = testingPosition.y / 5f;
        float height = Mathf.PerlinNoise(_x + seed.x * 2, _y + seed.y * 2);
        if (height > 0.2f && length <= 20)
        {
            float dir = Mathf.Round(Mathf.PerlinNoise(_x - seed.x, _y - seed.y) * 4) / 4;
            float adir = Mathf.Round(Mathf.PerlinNoise(_x - seed.x / 2f, _y - seed.y / 2f) * 4) / 4;
            //float dir = Mathf.PerlinNoise(_x - seed.x, _y - seed.y);
            Vector3 direction = new Vector3(Mathf.Cos(dir * Mathf.PI * 2 + adir * Mathf.PI * 2), Mathf.Sin(dir * Mathf.PI * 2 + adir * Mathf.PI * 2), 0);
            direction.Normalize();
            direction = new Vector3(Mathf.Round(direction.x), Mathf.Round(direction.y), 0);
            targets.Add(testingPosition);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(setupPoints(testingPosition + direction, originalPosition, length + 1));
        }
        else
        {
            float distance = 0;
            foreach(Vector3 a in targets)
            {
                float d = Vector3.Distance(a, originalPosition);
                if (d > distance)
                {
                    distance = d;
                }
            }
            if (distance > 10)
            {
                CreateWalls(originalPosition);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void fixColliders()
    {
        //Vector2[] tt = (Vector2[])targets.ToArray();
    }
 
    // Update is called once per frame
    void Update()
    {
        //GenerateShape();
    }
}
