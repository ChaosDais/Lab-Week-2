using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SphereControl : MonoBehaviour
{
    public float radius;
}

#if UNITY_EDITOR
[CustomEditor(typeof(SphereControl)), CanEditMultipleObjects]
public class SphereEditor : Editor
{
    Color storedColor = Color.green;

    public override void OnInspectorGUI()
    {
        /* Serialized Field */
        serializedObject.Update();

        var radius = serializedObject.FindProperty("radius");

        EditorGUILayout.PropertyField(radius);

        serializedObject.ApplyModifiedProperties();

        if (radius.floatValue < 0f)
        {
            EditorGUILayout.HelpBox("Radius cannot be less than 0!", MessageType.Warning);
        }

        /* Buttons */
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Select all spheres"))
            {
                var allSpheresComponents = FindObjectsOfType<SphereControl>();
                var allSpheres = allSpheresComponents
                    .Select(sphere => sphere.gameObject)
                    .ToArray();
                Selection.objects = allSpheres;
            }

            if (GUILayout.Button("Clear selection"))
            {
                Selection.objects = null;
            }
        }

        GUI.backgroundColor = storedColor;
        if (GUILayout.Button("Disable/Enable all spheres", GUILayout.Height(40)))
        {
            foreach (var sphere in FindObjectsOfType<SphereControl>(true))
            {
                Undo.RecordObject(sphere.gameObject, "Disable/Enable Sphere"); // Record the action in the undo menu
                sphere.gameObject.SetActive(!sphere.gameObject.activeSelf); // Disable or enable object
                storedColor = sphere.gameObject.activeSelf ? Color.green : Color.red; // Change button's color
            }
        }
    }
}
#endif
