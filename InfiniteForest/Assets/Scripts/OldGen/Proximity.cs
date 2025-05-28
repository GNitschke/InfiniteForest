using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    private Material _material;

    public float currentAlpha;
    public float currentDist;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        currentAlpha = 0;
        var distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        //_material.SetFloat("_Distance", distance);
        currentAlpha = (15f - distance) / (8f);
        currentDist = distance;
    }

    private void Update()
    {
        var distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        //_material.SetFloat("_Distance", distance);
        currentAlpha = (10f - distance) / (5f);
        currentDist = distance;
    }
}
