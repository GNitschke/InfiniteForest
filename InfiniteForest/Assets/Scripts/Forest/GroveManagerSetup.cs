using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroveManagerSetup : MonoBehaviour
{
    public Grove startingGrove;
    public Grove impassableGrove;
    public GameObject treePrefab;

    void Awake()
    {
        GroveManager.impassableGrove = impassableGrove;
        GroveManager.treePrefab = treePrefab;

        GroveManager.Initialize(startingGrove);
    }
}
