using UnityEngine;
using UnityEditor;

public class DisableStaticForAllObjects : EditorWindow
{
    [MenuItem("Tools/Disable Static for All Objects")]
    public static void ShowWindow()
    {
        GetWindow<DisableStaticForAllObjects>("Disable Static for All Objects");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Disable Static for All Objects"))
        {
            DisableStaticFlags();
        }
    }

    private static void DisableStaticFlags()
    {
        // Get all objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Disable all static flags (setting them to 0)
            GameObjectUtility.SetStaticEditorFlags(obj, (StaticEditorFlags)0);

            // Log the change for tracking purposes
            Debug.Log($"Static flags disabled for {obj.name}");
        }

        // Refresh the scene after changes
        Debug.Log("Static flags have been disabled for all objects.");
    }
}
