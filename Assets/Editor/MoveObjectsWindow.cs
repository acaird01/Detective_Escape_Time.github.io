using UnityEditor;
using UnityEngine;

public class MoveObjectsWindow : EditorWindow
{
    Vector3 moveAmount = new Vector3(2, 0, -1);

    [MenuItem("Tools/Move Objects Window")]
    public static void ShowWindow()
    {
        GetWindow<MoveObjectsWindow>("Move Objects");
    }

    void OnGUI()
    {
        GUILayout.Label("Move Selected Objects", EditorStyles.boldLabel);
        moveAmount = EditorGUILayout.Vector3Field("Move Amount", moveAmount);

        if (GUILayout.Button("Move"))
        {
            MoveSelectedObjects();
        }
    }

    void MoveSelectedObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Move Objects");
            obj.transform.position += moveAmount;
        }
    }
}

