using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 360f;
    public int rayCount = 10;
    public float viewDistance = 10f;

    private float lastFovUpdate = 0f;
    public float updateRate = 20f;

    Vector3 origin = Vector3.zero;

    public LayerMask layerMask;

    private List<Renderer> lastShown = new List<Renderer>();

    MeshFilter mf;

    void Awake()
    {
        mf = GetComponent<MeshFilter>();
    }

    void Update()
    {
        if (lastFovUpdate < Time.time)
        {
            lastFovUpdate = Time.time + 1f / updateRate;
            transform.position = PlayerController.instance.transform.position;
            updateMesh();
        }
    }

    void updateMesh()
    {
        foreach(Renderer ren in lastShown)
        {
            ren.enabled = false;
        }
        lastShown.Clear();
        Mesh mesh;
        if (mf.mesh != null)
        {
            mesh = mf.mesh;
        }
        else
        {
            mesh = new Mesh();
        }
        mesh.name = "FieldOfView";
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = new Vector3(0, 0, 0);

        int vertexIndex = 1;
        int triangeIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, layerMask);
            if (hit.collider == null)
            {
                vertex = direction * viewDistance;
            }
            else
            {
                vertex = hit.point - (Vector2)transform.position;
                Renderer render = hit.transform.GetComponent<Renderer>();
                if (render)
                {
                    lastShown.Add(render);
                    render.enabled = true;
                }
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangeIndex + 0] = 0;
                triangles[triangeIndex + 1] = vertexIndex - 1;
                triangles[triangeIndex + 2] = vertexIndex;

                triangeIndex += 3;
            }

            vertexIndex++;

            angle -= angleIncrease;

            //yield return new WaitForSeconds(1f / updateRate / rayCount);
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mf.mesh = mesh;
    }
}
