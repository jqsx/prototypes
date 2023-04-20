using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class OverworldGeneration : MonoBehaviour
{
    public static OverworldGeneration instance;
    public Chunk prefab;
    public int chunkSize = 100;
    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        BeginGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeginGeneration()
    {
        for (int y = -5; y <= 5; y++)
        {
            for (int x = -5; x <= 5; x++)
            {
                Instantiate(prefab, new Vector3(x * chunkSize, y * chunkSize, 0), Quaternion.identity);
            }
        }
    }
}
