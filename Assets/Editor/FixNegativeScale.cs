using UnityEngine;
using UnityEditor;

public class FixNegativeScale : EditorWindow
{
    [MenuItem("Tools/Fix Negative Scale")]
    public static void ShowWindow()
    {
        GetWindow<FixNegativeScale>("Fix Negative Scale");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Fix Negative Scale for All Objects"))
        {
            FixAllNegativeScales();
        }
    }

    private static void FixAllNegativeScales()
    {
        // Get all objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Transform objTransform = obj.transform;

            // Get the current scale
            Vector3 scale = objTransform.localScale;

            // Check if any scale component is negative
            bool hasNegativeScale = scale.x < 0 || scale.y < 0 || scale.z < 0;

            if (hasNegativeScale)
            {
                // Make each component of the scale positive by multiplying by -1 if it's negative
                scale.x = Mathf.Abs(scale.x);
                scale.y = Mathf.Abs(scale.y);
                scale.z = Mathf.Abs(scale.z);

                // Apply the new scale
                objTransform.localScale = scale;

                // Log the change for tracking purposes
                Debug.Log($"Fixed negative scale for {obj.name}");
            }
        }

        //// Refresh the scene to update the changes
        //EditorUtility.SetDirty(SceneView.lastActiveSceneView);
        //Debug.Log("All negative scales have been fixed.");
    }
}
