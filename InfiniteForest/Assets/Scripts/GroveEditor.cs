using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grove))]
public class GroveEditor : Editor
{
    SerializedProperty neighborsProp;
    SerializedProperty groveNW;
    SerializedProperty groveNE;
    SerializedProperty groveW;
    SerializedProperty groveE;
    SerializedProperty groveSW;
    SerializedProperty groveSE;

    void OnEnable()
    {
        neighborsProp = serializedObject.FindProperty("neighbors");
        groveNW = neighborsProp.GetArrayElementAtIndex(5);
        groveNE = neighborsProp.GetArrayElementAtIndex(0);
        groveW = neighborsProp.GetArrayElementAtIndex(4);
        groveE = neighborsProp.GetArrayElementAtIndex(1);
        groveSW = neighborsProp.GetArrayElementAtIndex(3);
        groveSE = neighborsProp.GetArrayElementAtIndex(2);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("Neighbors");

        Rect _rect = GUILayoutUtility.GetRect(25, EditorGUIUtility.singleLineHeight * 6f);

        DrawGrovesHex(_rect);

        Grove _grove = (Grove)target;

        if (GUILayout.Button("Sync Neighbors"))
        {
            ConnectNeighbors(_grove);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ConnectNeighbors(Grove _grove)
    {
        for (int i = 0; i < _grove.neighbors.Length; i++)
        {
            int nIndex = i + 3 <= 5 ? i + 3 : i - 3;
            if(_grove.neighbors[i] != null)
                _grove.neighbors[i].neighbors[nIndex] = _grove;
        }
    }

    private void DrawGrovesHex(Rect _position)
    {
        Rect _drawNW = new Rect(_position.min.x + (_position.width * 0.15f), _position.min.y, 
            _position.size.x * 0.3f, EditorGUIUtility.singleLineHeight);
        Rect _drawNE = new Rect(_position.min.x + (_position.width * 0.5f), _position.min.y, 
            _position.size.x * 0.3f, EditorGUIUtility.singleLineHeight);
        Rect _drawW = new Rect(_position.min.x, _position.min.y + EditorGUIUtility.singleLineHeight * 2f, 
            _position.size.x * 0.3f, EditorGUIUtility.singleLineHeight);
        Rect _drawE = new Rect(_position.min.x + (_position.width * 0.65f), _position.min.y + EditorGUIUtility.singleLineHeight * 2f, 
            _position.size.x * 0.3f, EditorGUIUtility.singleLineHeight);
        Rect _drawSW = new Rect(_position.min.x + (_position.width * 0.15f), _position.min.y + EditorGUIUtility.singleLineHeight * 4f,
            _position.size.x * 0.3f, EditorGUIUtility.singleLineHeight);
        Rect _drawSE = new Rect(_position.min.x + (_position.width * 0.5f), _position.min.y + EditorGUIUtility.singleLineHeight * 4f,
            _position.size.x * 0.3f, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(_drawNW, groveNW, new GUIContent());
        EditorGUI.PropertyField(_drawNE, groveNE, new GUIContent());
        EditorGUI.PropertyField(_drawW, groveW, new GUIContent());
        EditorGUI.PropertyField(_drawE, groveE, new GUIContent());
        EditorGUI.PropertyField(_drawSW, groveSW, new GUIContent());
        EditorGUI.PropertyField(_drawSE, groveSE, new GUIContent());
    }
}
