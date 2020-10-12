using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTree : MonoBehaviour
{
    public GameObject Treefab;

    private float range;

    // Start is called before the first frame update
    void Start()
    {
        range = 8;
        Generate(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generate(float x, float y, float seed)
    {
        float scaler = 0.13f;
        for(float i = x - range / 2f; i < x + range / 2f; i++)
        {
            for(float j = y - range / 2f; j < y + range / 2f; j++)
            {
                float noise = Mathf.PerlinNoise(seed + i * scaler, seed + j * scaler);
                float offsetX = Mathf.PerlinNoise(seed + i * 10.23f, seed + j * 10.23f) * 1.8f - 0.9f;
                float offsetY = Mathf.PerlinNoise(seed + i * 10.33f, seed + j * 10.33f) * 1.8f - 0.9f;
                if (noise > 0.3f)
                {
                    GameObject tree = Instantiate(Treefab, new Vector3(i, 0, j), Quaternion.identity);
                    tree.transform.localScale = new Vector3(noise, noise, noise);
                    tree.transform.position = new Vector3(tree.transform.position.x + offsetX, 0,  tree.transform.position.z + offsetY);
                }
            }
        }
    }
}
