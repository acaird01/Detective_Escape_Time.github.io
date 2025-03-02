using UnityEngine;
using UnityEditor;

public class MakeCollidersConvex : EditorWindow
{
    [MenuItem("Tools/Make All Colliders Convex")]
    public static void MakeAllCollidersConvex()
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        int meshColliderCount = 0;

        // Iterate through each GameObject
        foreach (GameObject obj in allObjects)
        {
            // Get all MeshColliders in the GameObject
            MeshCollider[] meshColliders = obj.GetComponents<MeshCollider>();

            foreach (MeshCollider meshCollider in meshColliders)
            {
                if (!meshCollider.convex)
                {
                    meshCollider.convex = true;
                    meshColliderCount++;
                    Debug.Log($"Set {obj.name}'s MeshCollider to convex.");
                }
            }
        }

        Debug.Log($"Total MeshColliders set to convex: {meshColliderCount}");
    }
}
