using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpellCaster))]
public class PlayerSpells : MonoBehaviour
{
    const float SIGILSENSITIVITY = 150f;
    public float sigilSize;
    public float castingSensitivity;
    public Transform sigilTracer;

    SpellCaster playerCaster;

    [SerializeField]
    int currIncantation;
    int currNode;

    Vector2 currSigilPos;

    // Start is called before the first frame update
    void Start()
    {
        playerCaster = GetComponent<SpellCaster>();
    }

    // SIGIL
    //   1
    // 2   3
    //   4

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //---------------
            #region SIGIL INCANTATION
            currSigilPos.x += Input.GetAxis("Mouse X") * Time.deltaTime * castingSensitivity;
            currSigilPos.y += Input.GetAxis("Mouse Y") * Time.deltaTime * castingSensitivity;

            //------------------------
            #region LESSER AXIS RECALLIBRATION

            float _absX = Mathf.Abs(currSigilPos.x);
            float _absY = Mathf.Abs(currSigilPos.y);

            if (_absX > _absY) // find if x or y is the less dominant axis
            {
                if(_absY > SIGILSENSITIVITY - _absX) // if the cursor y is out of bounds of the sigil
                {
                    _absY = SIGILSENSITIVITY - _absX; // reset cursor y to the edge of the sigil
                }
            }
            else
            {
                if (_absX > SIGILSENSITIVITY - _absY) // if the cursor x is out of bounds of the sigil
                {
                    _absX = SIGILSENSITIVITY - _absY; // reset cursor x to the edge of the sigil
                }
            }

            currSigilPos.x = (currSigilPos.x > 0) ? _absX : -_absX; // reset x coord to in bounds value with original sign
            currSigilPos.y = (currSigilPos.y > 0) ? _absY : -_absY; // reset y coord to in bounds value with original sign

            #endregion
            //------------------------

            //---------------------------------------------------------
            #region INCANTATION NODE DETERMINATION & GREATER AXIS RECALLBRATION

            if (currSigilPos.x > SIGILSENSITIVITY) // is x coord out of bounds (upper)
            {
                currSigilPos.x = SIGILSENSITIVITY; // reset x coord to be in bounds
                currSigilPos.y = 0;                // reset y coord to be in bounds
                if (currNode != 3)         // if node is not already this corner
                {
                    currIncantation *= 10; 
                    currIncantation += 3;
                    currNode = 3;          // add node to incantation
                }
            }
            else if (currSigilPos.x < -SIGILSENSITIVITY)
            {
                currSigilPos.x = -SIGILSENSITIVITY;
                currSigilPos.y = 0;
                if (currNode != 2)
                {
                    currIncantation *= 10;
                    currIncantation += 2;
                    currNode = 2;
                }
            }

            if (currSigilPos.y > SIGILSENSITIVITY)
            {
                currSigilPos.y = SIGILSENSITIVITY;
                currSigilPos.x = 0;
                if (currNode != 1)
                {
                    currIncantation *= 10;
                    currIncantation += 1;
                    currNode = 1;
                }
            }
            else if (currSigilPos.y < -SIGILSENSITIVITY)
            {
                currSigilPos.y = -SIGILSENSITIVITY;
                currSigilPos.x = 0;
                if (currNode != 4)
                {
                    currIncantation *= 10;
                    currIncantation += 4;
                    currNode = 4;
                }
            }
            #endregion
            //---------------------------------------------------------

            sigilTracer.localPosition = (Vector3)currSigilPos * (sigilSize / SIGILSENSITIVITY);
            #endregion
            //---------------
        }

        if (Input.GetMouseButtonDown(0))
        {
            //-------------
            #region NEW INCANTATION

            currIncantation = 0;
            currNode = 0;
            currSigilPos = Vector2.zero;
            sigilTracer.localPosition = Vector3.zero;

            #endregion
            //-------------
        }

        if (Input.GetMouseButtonUp(0))
        {
            //---------------------
            #region END CURRENT INCANTATION
            sigilTracer.localPosition = Vector3.zero;

            #endregion
            //---------------------
        }

        if (Input.GetMouseButtonDown(1))
        {
            //--------------
            #region CAST INCANTATION

            playerCaster.CastSpell(currIncantation);
            currIncantation = 0;

            #endregion
            //--------------
        }
    }
}
