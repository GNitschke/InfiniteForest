using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroveInstance : MonoBehaviour
{
    public Grove grove;
    public GroveInstance[] neighbors = new GroveInstance[6];

    public void InstanceNeighbors(int _exception)
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (i != _exception)
            {
                if (grove.neighbors[i] != null)
                {
                    neighbors[i] = grove.neighbors[i].CreateInstance
                        (transform.position + HexMetrics.neighborPos[i], 0);
                }
                else
                {
                    neighbors[i] = GroveManager.impassableGrove.CreateInstance
                        (transform.position + HexMetrics.neighborPos[i], 0);
                }
            }
        }
    }

    public void RemoveNeighborInstances(int _exception)
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (i != _exception)
            {
                if (neighbors[i] != null)
                {
                    grove.RemoveInstance(neighbors[i]);
                    neighbors[i] = null;
                }
            }
        }
    }
}
