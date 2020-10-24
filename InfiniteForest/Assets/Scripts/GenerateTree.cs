using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTree : MonoBehaviour
{
    public GameObject Treefab;
    public GameObject Chunk;

    public float chunkWidth;

    // Start is called before the first frame update
    void Start()
    {
        chunkWidth = 10f;
        GenerateChunk(0, 0, 0);
        GenerateChunk(10, 0, 0);
        DestroyChunk(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateChunk(float x, float y, float seed)
    {
        float scaler = 0.13f;
        GameObject parent = new GameObject();
        parent.transform.position = new Vector3(x, 0, y);
        parent.name = "Chunk" + x + "_" + y;
        BoxCollider bc = parent.AddComponent<BoxCollider>();
        bc.size.Set(chunkWidth, chunkWidth, chunkWidth);
        parent.AddComponent<Chunk>();
        for (float i = x - chunkWidth / 2f; i < x + chunkWidth / 2f + 1f; i++)
        {
            for(float j = y - chunkWidth / 2f; j < y + chunkWidth / 2f + 1f; j++)
            {
                float noise = Mathf.PerlinNoise(seed + i * scaler, seed + j * scaler);
                float offsetX = Mathf.PerlinNoise(seed + i * 10.23f, seed + j * 10.23f) * 1.8f - 0.9f;
                float offsetY = Mathf.PerlinNoise(seed + i * 10.33f, seed + j * 10.33f) * 1.8f - 0.9f;
                if (noise > 0.3f)
                {
                    GameObject tree = Instantiate(Treefab, new Vector3(i, 0, j), Quaternion.identity);
                    tree.transform.parent = parent.transform;
                    tree.transform.localScale = new Vector3(noise, noise, noise);
                    tree.transform.position = new Vector3(tree.transform.position.x + offsetX, 0,  tree.transform.position.z + offsetY);
                }
            }
        }
    }

    public void DestroyChunk(float x, float y)
    {
        Destroy(GameObject.Find("Chunk" + x + "_" + y));
    }


}
