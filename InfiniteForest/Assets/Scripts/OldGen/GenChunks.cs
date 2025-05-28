using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenChunks : MonoBehaviour
{
    public GameObject Treefab;

    public Transform player;

    private int chunkWidth;
    private int numChunks;
    public float seed;
    public float saplingsPerChunk;
    [Range(0f, 1f)]
    public float density;
    public float sizeDifference;
    public float averageSize;

    public Chunk currChunk;
    public Chunk prevChunk;
    public List<Chunk> chunks;
    float scaler = 0.13f;

    void Start()
    {
        numChunks = 3;
        chunkWidth = 10;
        for(int i = 0; i < numChunks * chunkWidth; i+= chunkWidth)
        {
            for (int j = 0; j < numChunks * chunkWidth; j+= chunkWidth)
            {
                GenerateChunk(i, j);
            }
        }
        currChunk = chunks[0];
        player.position = new Vector3(chunks[4].x, player.position.y, chunks[4].y);
    }

    public void GenerateChunk(int x, int y)
    {
        GameObject parent = new GameObject();
        parent.transform.position = new Vector3(x, 0, y);
        parent.name = "Chunk_" + x + "_" + y;
        parent.AddComponent<Chunk>();
        Chunk c = parent.GetComponent<Chunk>();
        chunks.Add(c);
        c.x = x;
        c.y = y;

        for(int i = 0; i < Mathf.Pow(chunkWidth / saplingsPerChunk, 2); i++)
        {
            Instantiate(Treefab, parent.transform);
        }
        SetupChunk(c, x, y);
    }

    public void UpdateChunks()
    {
        if(prevChunk.x != currChunk.x)
        {
            var remove = chunks.FindAll(delegate (Chunk c)
            {
                return c.x == 2 * prevChunk.x - currChunk.x;
            });
            foreach(Chunk r in remove)
            {
                SetupChunk(r, 2 * currChunk.x - prevChunk.x, r.y);
            }
        }
        if(prevChunk.y != currChunk.y)
        {
            var remove = chunks.FindAll(delegate (Chunk c)
            {
                return c.y == 2 * prevChunk.y - currChunk.y;
            });
            foreach (Chunk r in remove)
            {
                SetupChunk(r, r.x, 2 * currChunk.y - prevChunk.y);
            }
        }
    }

    public void SetupChunk(Chunk c, float x, float y)
    {
        c.x = x;
        c.y = y;
        c.transform.position = new Vector3(x, 0, y);
        c.name = "Chunk_" + x + "_" + y;

        float i = x - chunkWidth / 2f;
        float j = y - chunkWidth / 2f;

        for(int t = 0; t < c.transform.childCount; t++)
        {
            float noise = Mathf.PerlinNoise(seed + i * scaler, seed + j * scaler);
            float offsetX = Mathf.PerlinNoise(seed + i * 10.23f, seed + j * 10.23f) * 1.8f - 0.9f;
            float offsetY = Mathf.PerlinNoise(seed + i * 10.33f, seed + j * 10.33f) * 1.8f - 0.9f;
            Transform currTree = c.transform.GetChild(t);
            currTree.position = new Vector3(i + offsetX, 0, j + offsetY);
            if (noise > 1f - density)
            {
                currTree.localScale = Vector3.one * averageSize;
                currTree.localEulerAngles = Vector3.up * (Mathf.Lerp(0.3f, 1f, noise) * 360f);
            }
            else
            {
                currTree.localScale = new Vector3(0, 0, 0);
            }

            if(i > x + chunkWidth / 2f)
            {
                i = x - chunkWidth / 2f;
                j += saplingsPerChunk;
            }
            else
            {
                i += saplingsPerChunk;
            }
        }
    }

    private void Update()
    {
        if(Mathf.Abs(player.position.x - (currChunk.x)) > chunkWidth / 2.0f || Mathf.Abs(player.position.z - (currChunk.y)) > chunkWidth / 2.0f)
        {
            prevChunk = currChunk;
            currChunk = chunks.Find(delegate (Chunk c)
            {
                return Mathf.Abs(player.position.x - (c.x)) < chunkWidth / 2.0f && Mathf.Abs(player.position.z - (c.y)) < chunkWidth / 2.0f;
            });
            UpdateChunks();
        }
    }
}
