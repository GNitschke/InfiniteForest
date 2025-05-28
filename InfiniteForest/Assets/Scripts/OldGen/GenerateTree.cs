using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTree : MonoBehaviour
{
    public GameObject Treefab;

    public int chunkWidth;
    public int numChunks;
    public float seed;

    public Chunk currChunk;

    bool switching;

    void Start()
    {
        chunkWidth = 10;
        numChunks = 4;
        numChunks -= numChunks % 2;
        seed = 0f;
        
        for(int i = -numChunks/2; i < numChunks/2; i++)
        {
            for(int j = -numChunks / 2; j < numChunks / 2; j++)
            {
                GenerateChunk(chunkWidth * i, chunkWidth * j);
            }
        }
        StartCoroutine(UpdateChunks());
        
    }

    public void GenerateChunk(int x, int y)
    {
        Chunk[] chunks = FindObjectsOfType<Chunk>();
        if (!Array.Find(chunks, ele => ele.x == x && ele.y == y))
        {
            float scaler = 0.13f;
            GameObject parent = new GameObject();
            parent.transform.position = new Vector3(x, 0, y);
            parent.name = "Chunk" + x + "_" + y;
            //BoxCollider bc = parent.AddComponent<BoxCollider>();
            //bc.size = new Vector3(chunkWidth - 0.5f, chunkWidth - 0.5f, chunkWidth - 0.5f);
            parent.AddComponent<Chunk>();
            Chunk c = parent.GetComponent<Chunk>();
            c.x = x;
            c.y = y;
            for (float i = x - chunkWidth / 2f; i < x + chunkWidth / 2f + 1f; i++)
            {
                for (float j = y - chunkWidth / 2f; j < y + chunkWidth / 2f + 1f; j++)
                {
                    float noise = Mathf.PerlinNoise(seed + i * scaler, seed + j * scaler);
                    float offsetX = Mathf.PerlinNoise(seed + i * 10.23f, seed + j * 10.23f) * 1.8f - 0.9f;
                    float offsetY = Mathf.PerlinNoise(seed + i * 10.33f, seed + j * 10.33f) * 1.8f - 0.9f;
                    if (noise > 0.3f)
                    {
                        GameObject tree = Instantiate(Treefab, new Vector3(i, 0, j), Quaternion.identity);
                        tree.transform.parent = parent.transform;
                        tree.transform.localScale = new Vector3(noise, noise, noise);
                        tree.transform.position = new Vector3(tree.transform.position.x + offsetX, 0, tree.transform.position.z + offsetY);
                    }
                }
            }
        }
    }

    public void DestroyChunk(float x, float y)
    {
        Destroy(GameObject.Find("Chunk" + x + "_" + y));
    }

    public IEnumerator UpdateChunks()
    {
        yield return new WaitForSeconds(5);
        Chunk[] chunks = FindObjectsOfType<Chunk>();
        for (int i = (int)transform.position.x - ((int)transform.position.x % chunkWidth) - (numChunks / 2) * chunkWidth; i < transform.position.x - ((int)transform.position.x % chunkWidth) + (numChunks / 2) * chunkWidth; i += chunkWidth)
        {
            for (int j = (int)transform.position.z - ((int)transform.position.z % chunkWidth) - (numChunks / 2) * chunkWidth; j < transform.position.z - ((int)transform.position.z % chunkWidth) + (numChunks / 2) * chunkWidth; j += chunkWidth)
            {
                GenerateChunk(i, j);
            }
        }
        foreach (Chunk c in chunks)
        {
            if (c.x > transform.position.x + chunkWidth * numChunks / 2 || c.x < transform.position.x - chunkWidth * numChunks / 2 ||
                c.y > transform.position.z + chunkWidth * numChunks / 2 || c.y < transform.position.z - chunkWidth * numChunks / 2)
            {
                DestroyChunk(c.x, c.y);
            }
        }
        StartCoroutine(UpdateChunks());
    }

    /*
    public IEnumerator UpdateChunks(Chunk c)
    {
        yield return new WaitUntil(() => switching == false);
        switching = true;
        currChunk = c;
        for (int i = 0; i < chunks.Length; i++)
        {
            Chunk loaded = chunks[i];
            if (loaded.x < currChunk.x - chunkWidth || loaded.x > currChunk.x + chunkWidth)
            {
                chunks[i] = GenerateChunk(currChunk.x + (currChunk.x - loaded.x) / 2, currChunk.y + (currChunk.y - loaded.y));
                DestroyChunk(loaded.x, loaded.y);
            }
        }
        for (int i = 0; i < chunks.Length; i++)
        {
            Chunk loaded = chunks[i];
            if (loaded.y < currChunk.y - chunkWidth || loaded.y > currChunk.y + chunkWidth)
            {
                chunks[i] = GenerateChunk(currChunk.x + (currChunk.x - loaded.x), currChunk.y + (currChunk.y - loaded.y) / 2);
                DestroyChunk(loaded.x, loaded.y);
            }
        }
        switching = false;
    }
    */
}
