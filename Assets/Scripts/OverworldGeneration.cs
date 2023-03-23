using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class OverworldGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        createRock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createRock()
    {
        GameObject gameObject = new GameObject("rock");
        gameObject.transform.position = new Vector3(10, 0, 0);
        SpriteShapeController controller = gameObject.AddComponent<SpriteShapeController>();
        controller.spriteShape = new SpriteShape();
        gameObject.AddComponent<SpriteShapeRenderer>();

        for(int i = 0; i < Random.Range(0, 15); i++)
        {
            float deg = i / 360f;

            controller.spline.InsertPointAt(i, new Vector3(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad)) * Random.Range(1f, 5f));
        }
        controller.UpdateSpriteShapeParameters();
    }
}
