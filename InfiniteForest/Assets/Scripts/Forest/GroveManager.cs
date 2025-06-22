using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GroveManager
{
    static GroveInstance prevGrove;
    static GroveInstance currGrove;
    //static GameObject currGroveInstance;

    public static Grove impassableGrove;

    public struct GroveObject
    {
        public float size;
        public Vector3 position;
        public Vector3 rotation;
    }

    private static Vector3 recycleBinPos = Vector3.down * 1000f;

    private static List<GameObject> groveRecycler = new List<GameObject>();

    public static GameObject treePrefab;
    private static List<GameObject> treeRecycler = new List<GameObject>();

    public static void Initialize(Grove _start)
    {
        currGrove = _start.CreateInstance(Vector3.zero, 0);
        currGrove.InstanceNeighbors(-1);

        prevGrove = currGrove.neighbors[0];
    }

    public static void CheckNewGrove(Vector3 _playerPosition)
    {
        if(Vector3.Distance(_playerPosition, currGrove.transform.position) > HexMetrics.outerRadius)
        {
            Vector2 _direction = Vector2.zero;
            if (_playerPosition.x > currGrove.transform.position.x)
            {
                _direction.x = 1;
            }
            else
            {
                _direction.x = -1;
            }

            float _zBound = ((0.5f * HexMetrics.outerRadius) / HexMetrics.innerRadius) * Mathf.Abs(_playerPosition.x - currGrove.transform.position.x);
            Debug.Log(_zBound);
            if (_playerPosition.z > currGrove.transform.position.z + _zBound)
            {
                _direction.y = 1;
            }
            else if (_playerPosition.z > currGrove.transform.position.z - _zBound)
            {
                _direction.y = 0;
            }
            else
            {
                _direction.y = -1;
            }

            Debug.Log(_direction);
            OnEnterNewGrove(_direction);
        }
    }

    public static void OnEnterNewGrove(Vector2 _direction)
    {
        int exceptionEnable = -1;
        int exceptionDisable = -1;
        prevGrove = currGrove;
        if(_direction.x > 0)
        {
            switch (_direction.y)
            {
                case 1:
                    exceptionEnable = 3;
                    exceptionDisable = 0;
                    break;
                case 0:
                    exceptionEnable = 4;
                    exceptionDisable = 1;
                    break;
                case -1:
                    exceptionEnable = 5;
                    exceptionDisable = 2;
                    break;
            }
        }
        else
        {
            switch (_direction.y)
            {
                case -1:
                    exceptionEnable = 0;
                    exceptionDisable = 3;
                    break;
                case 0:
                    exceptionEnable = 1;
                    exceptionDisable = 4;
                    break;
                case 1:
                    exceptionEnable = 2;
                    exceptionDisable = 5;
                    break;
            }
        }
        currGrove = prevGrove.neighbors[exceptionDisable];
        currGrove.neighbors[exceptionEnable] = prevGrove;
        prevGrove.RemoveNeighborInstances(exceptionDisable);
        currGrove.InstanceNeighbors(exceptionEnable);
    }

    public static GroveInstance PullGrove()
    {
        GroveInstance _grove = PullFromRecycler(groveRecycler).GetComponent<GroveInstance>();

        if (_grove == null)
        {
            _grove = new GameObject().AddComponent<GroveInstance>();
            _grove.name = "GroveInstance";
        }

        return _grove;
    }
    public static void RecycleGrove(GameObject _grove)
    {
        RecycleObject(_grove, groveRecycler);
    }

    public static GameObject PullTree()
    {
        GameObject _tree = PullFromRecycler(treeRecycler);

        if(_tree == null)
        {
            _tree = Object.Instantiate(treePrefab);
            _tree.name = "TreeInstance";
        }
        
        return _tree;
    }
    public static void RecycleTree(GameObject _tree)
    {
        RecycleObject(_tree, treeRecycler);
    }

    public static GroveObject NewGoveObject(Vector3 _position, Vector3 _rotation, float _size)
    {
        return new GroveObject()
        {
            position = _position,
            rotation = _rotation,
            size = _size
        };
    }

    private static void RecycleObject(GameObject _object, List<GameObject> _recycler)
    {
        _object.transform.position = recycleBinPos;
        _recycler.Add(_object);
    }

    private static GameObject PullFromRecycler(List<GameObject> _recycler)
    {
        GameObject _object;
        if(_recycler.Count > 0)
        {
            _object = _recycler[_recycler.Count - 1];
            _recycler.RemoveAt(_recycler.Count - 1);
        }
        else
        {
            _object = null;
        }

        return _object;
    }
}
