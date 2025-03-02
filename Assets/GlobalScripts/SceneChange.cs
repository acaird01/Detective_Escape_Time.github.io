using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    private void OnMouseDown() // 이 스크립트 문에 박아놓고, 문 이름 씬 이름으로 오타없이 설정해줄것
    {
        string checkDoorName = gameObject.name;
        LoadingSceneManager.LoadScene(checkDoorName);
    }
}
