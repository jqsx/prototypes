using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicAI : Entity
{
    private Vector2 target = new Vector2(0, 0);
    private Vector2[] castPositions = new Vector2[9];
    private Rigidbody2D rb;
    public LayerMask layerMask;
    private void Awake()
    {
        for(int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                castPositions[(y + 1) * 3 + x + 1] = new Vector2(x, y);
            }
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement(CastPossibleLocations());
    }

    private Vector2 CastPossibleLocations()
    {
        Vector2 closest = transform.position;
        foreach(Vector2 pos in castPositions)
        {
            if (optimizedDistance(closest, target) > optimizedDistance(closest, pos + (Vector2)transform.position))
            {
                Collider2D col = Physics2D.OverlapBox(pos + (Vector2)transform.position, Vector2.one, 0, layerMask);
                if (!col)
                {
                    closest = (Vector2)transform.position + pos;
                }
            }
        }

        return closest - (Vector2)transform.position;
    }

    private void movement(Vector2 direction)
    {
        rb.velocity = Vector2.Lerp(rb.velocity, direction * EntityStatistics.MoveSpeed, Time.deltaTime * 5f);
    }

    private static float optimizedDistance(Vector2 one, Vector2 two)
    {
        return Mathf.Abs(one.x - two.x) + Mathf.Abs(one.y - two.y);
    }
}
