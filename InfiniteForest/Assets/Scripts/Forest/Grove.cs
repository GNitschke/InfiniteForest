using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grove
{
    public string name;
    public Grove[] neighbors = new Grove[6];

    public List<GroveManager.GroveObject> trees = new List<GroveManager.GroveObject>();

    public GroveInstance CreateInstance(Vector3 _position, int _layer)
    {
        GroveInstance _groveInstance = GroveManager.PullGrove();
        _groveInstance.name = name + "Instance";
        _groveInstance.grove = this;
        _groveInstance.transform.position = _position;

        foreach (var _treeData in trees)
        {
            GameObject _tree = GroveManager.PullTree();
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
                GroveManager.RecycleTree(_child);
            }
        }

        GroveManager.RecycleGrove(_groveInstance.gameObject);
    }

    private void SetGameLayerRecursive(GameObject _gameObject, int _layer)
    {
        _gameObject.layer = _layer;
        foreach (Transform _child in _gameObject.transform)
        {
            SetGameLayerRecursive(_child.gameObject, _layer);
        }
    }
}
