using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using static GroveManager;
using TMPro;

public class GroveCreator : MonoBehaviour
{
    private Grove currGrove;
    private GameObject groveObject;

    [SerializeField]
    private List<Grove> groves;

    public TMP_InputField nameInput;
    public TextMeshProUGUI saveMessage;

    public float treeMinSize;
    public float treeMaxSize;

    public float treeXRotationMax;
    public float treeZRotationMax;

    public GameObject treePrefab;

    void Start()
    {
        groves = new List<Grove>();
        CreateNewGrove();
        LoadGrovesFromFile();
    }

    public void CreateNewGrove()
    {
        currGrove = new Grove();
        groveObject = new GameObject();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, LayerMask.GetMask("Ground")))
            {
                float _size = Random.Range(treeMinSize, treeMaxSize);
                Vector3 _rotation = new Vector3(Random.Range(0, treeXRotationMax), Random.Range(0f, 360f), Random.Range(0, treeZRotationMax));
                GroveObject _tree = NewGoveObject(_hit.point, _rotation, _size);
                LogTree(currGrove, _tree);
                GrowTree(_tree);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, LayerMask.GetMask("Default")))
            {
                UnlogTree(_hit.collider.transform.parent);
                Destroy(_hit.collider.transform.parent.gameObject);
            }
        }
    }

    private void LogTree(Grove _grove, GroveObject _tree)
    {
        _grove.trees.Add(_tree);
    }

    private void GrowTree(GroveObject _tree)
    {
        Transform _treeTransform = Instantiate(treePrefab, groveObject.transform).transform;
        _treeTransform.localPosition = _tree.position;
        _treeTransform.localRotation = Quaternion.Euler(_tree.rotation);
        _treeTransform.localScale = Vector3.one * _tree.size;
    }

    private void UnlogTree(Transform _tree)
    {
        for(int i = 0; i < currGrove.trees.Count; i++)
        {
            if(currGrove.trees[i].position == _tree.position)
            {
                currGrove.trees.RemoveAt(i);
                break;
            }
        }
    }

    public void GrowAllTrees()
    {
        ClearAllTrees();
        for(int i = 0; i < currGrove.trees.Count; i++)
        {
            GrowTree(currGrove.trees[i]);
        }
    }

    private void ClearAllTrees()
    {
        for(int i = groveObject.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(groveObject.transform.GetChild(i).gameObject);
        }
    }

    public void ClearTreeLog()
    {
        currGrove.trees.Clear();
        ClearAllTrees();
    }

    public void SaveGrove()
    {
        Grove _grove = ReadGrove(WriteGrove(currGrove));
        _grove.name = nameInput.text;
        int _index = -1;
        for(int i = 0; i < groves.Count; i++)
        {
            if(groves[i].name == _grove.name)
            {
                groves[i] = _grove;
                _index = i;
                Debug.Log("found match");
            }
        }
        if (_index == -1)
        {
            groves.Add(_grove);
        }

        Debug.Log(groves.Count + " groves");

        SaveGrovesToFile();
    }
    
    public void LoadGrove(int _index)
    {
        currGrove = groves[_index];
        nameInput.text = currGrove.name;
        GrowAllTrees();
    }

    private string WriteGrove(Grove _grove)
    {
        string _groveString = _grove.name + ":Trees{";

        for(int i = 0; i < _grove.trees.Count; i++)
        {
            _groveString += "[" + _grove.trees[i].position.ToString() + ";" 
                + _grove.trees[i].rotation.ToString() + ";"
                + _grove.trees[i].size + "]";
        }

        _groveString += "}";

        return _groveString;
    }

    private Grove ReadGrove(string _groveString)
    {
        Grove _grove = new Grove();

        _grove.name = _groveString.Substring(0, _groveString.IndexOf(":") + 1);
        _groveString = _groveString.Substring(_groveString.IndexOf(":") + 1);

        //Trees
        string _treeString = _groveString.Substring(_groveString.IndexOf('[') + 1,
            _groveString.IndexOf('}') - _groveString.IndexOf('['));
        string[] _treeList = _treeString.Split('[');

        for(int i = 0; i < _treeList.Length; i++)
        {
            _treeList[i] = _treeList[i].Trim('}', '[', ']');
            string[] _treeData = _treeList[i].Split(';');
            LogTree(_grove, NewGoveObject(StringToVector3(_treeData[0]),
                StringToVector3(_treeData[1]), float.Parse(_treeData[2])));
        }

        return _grove;
    }

    private Vector3 StringToVector3(string _vectorString)
    {
        Vector3 _vector = new Vector3();

        _vectorString = _vectorString.Trim('(', ')');

        string[] _values = _vectorString.Split(',');

        _vector.x = float.Parse(_values[0]);
        _vector.y = float.Parse(_values[1]);
        _vector.z = float.Parse(_values[2]);

        return _vector;
    }

    public void SaveGrovesToFile()
    {
        string _path = "Assets/Saves/groves.txt";
        StreamWriter _writer = new StreamWriter(_path);
        for(int i = 0; i < groves.Count; i++)
        {
            _writer.WriteLine(WriteGrove(groves[i]));
        }

        _writer.Close();
        StartCoroutine(SaveSuccessful());
    }

    public void LoadGrovesFromFile()
    {
        string _path = "Assets/Saves/groves.txt";
        StreamReader _reader = new StreamReader(_path);
        while (!_reader.EndOfStream)
        {
            groves.Add(ReadGrove(_reader.ReadLine()));
        }
        _reader.Close();
    }

    private IEnumerator SaveSuccessful()
    {
        Debug.Log("save successful");
        saveMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        saveMessage.gameObject.SetActive(false);
    }
}
