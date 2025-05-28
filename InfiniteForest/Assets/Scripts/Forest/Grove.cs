using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grove : MonoBehaviour
{
    public Grove[] neighbors = new Grove[6];

    public GroveManager.GroveObject[] trees = new GroveManager.GroveObject[1];

    public GroveInstance CreateInstance(Vector3 _position, int _layer)
    {
        GroveInstance _groveInstance = new GameObject(name + "Instance").AddComponent<GroveInstance>();
        _groveInstance.grove = this;
        _groveInstance.transform.position = _position;

        foreach (var _treeData in trees)
        {
            GameObject _tree = GroveManager.pullTree();
            SetGameLayerRecursive(_tree, _layer);
            _tree.transform.parent = _groveInstance.transform;
            _tree.transform.localScale = Vector3.one;// * _treeData.size;
            _tree.transform.localPosition = _treeData.position;
            _tree.transform.localRotation = Quaternion.Euler(_treeData.rotation);
        }

        return _groveInstance;
    }

    public void RemoveInstance(GroveInstance _groveInstance)
    {
        for (int i = 0; i < _groveInstance.transform.childCount; i++)
        {
            GameObject _child = _groveInstance.transform.GetChild(i).gameObject;
            if (_child.name == "TreeInstance")
            {
                GroveManager.recycleTree(_child);
            }
        }

        Destroy(_groveInstance.gameObject);
    }

    private void SetGameLayerRecursive(GameObject _gameObject, int _layer)
    {
        _gameObject.layer = _layer;
        foreach (Transform _child in _gameObject.transform)
        {
            SetGameLayerRecursive(_child.gameObject, _layer);
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i = 0; i < HexMetrics.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(transform.position + HexMetrics.corners[i], transform.position + HexMetrics.corners[i + 1]);
        }
        Gizmos.DrawLine(transform.position + HexMetrics.corners[5], transform.position + HexMetrics.corners[0]);
    }
}
