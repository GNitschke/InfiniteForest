using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float x;
    public float y;

    public Chunk(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
