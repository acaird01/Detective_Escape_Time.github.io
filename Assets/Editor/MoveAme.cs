using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveAme : EditorWindow

{
    //write me an editor that finds player by name "Player" and sets its position y to 10
    [MenuItem("Tools/MoveAme")]
    public static void ShowWindow()
    {
        GetWindow<MoveAme>("MoveAme");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Move Player"))
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(player.transform.position.x, 10, player.transform.position.z);
            }
            else
            {
                Debug.Log("Player not found");
            }
        }
    }


}
