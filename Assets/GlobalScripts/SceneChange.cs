using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    private void OnMouseDown() // �� ��ũ��Ʈ ���� �ھƳ���, �� �̸� �� �̸����� ��Ÿ���� �������ٰ�
    {
        string checkDoorName = gameObject.name;
        LoadingSceneManager.LoadScene(checkDoorName);
    }
}
