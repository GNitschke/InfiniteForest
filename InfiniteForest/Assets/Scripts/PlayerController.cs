using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GenerateTree generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<GenerateTree>();
    }

    private void OnCollisionExit(Collision collision)
    {
        Destroy(collision.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
