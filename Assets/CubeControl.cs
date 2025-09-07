using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

public class CubeControl : MonoBehaviour
{
    public float size;
}

#if UNITY_EDITOR
[CustomEditor(typeof(CubeControl)), CanEditMultipleObjects]
public class CubeEditor : Editor
{
    Color storedColor = Color.green;

    public override void OnInspectorGUI()
    {
        /* Serialized Field */
        serializedObject.Update();

        var size = serializedObject.FindProperty("size");

        EditorGUILayout.PropertyField(size);

        serializedObject.ApplyModifiedProperties();

        if(size.floatValue < 0f)
        {
            EditorGUILayout.HelpBox("Size cannot be less than 0!", MessageType.Warning);
        }

        /* Buttons */
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Select all cubes"))
            {
                var allCubesComponents = FindObjectsOfType<CubeControl>();
                var allCubes = allCubesComponents
                    .Select(cube => cube.gameObject)
                    .ToArray();
                Selection.objects = allCubes;
            }

            if (GUILayout.Button("Clear selection"))
            {
                Selection.objects = null;
            }
        }

        GUI.backgroundColor = storedColor;
        if (GUILayout.Button("Disable/Enable all cubes", GUILayout.Height(40)))
        {
            foreach (var cube in FindObjectsOfType<CubeControl>(true))
            {
                Undo.RecordObject(cube.gameObject, "Disable/Enable Cube"); // Record the action in the undo menu
                cube.gameObject.SetActive(!cube.gameObject.activeSelf); // Disable or enable object
                storedColor = cube.gameObject.activeSelf ? Color.green : Color.red; // Change button's color
            }
        }
    }
}
#endif