using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kiara_Fauna_branch : MonoBehaviour
{
    /*private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded ��������Ʈ ü���� �̿���
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // ���� ó�� �ε� �Ǿ��� �� ȣ���ϱ�
    {
        fauna = gameObject.GetComponent<Item15FaunaBranch>();
        ItemSetting();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }*/


    private void Start()
    {
        ItemSetting();
    }


    void ItemSetting()
    {
        if (ItemManager._instance.inventorySlots[15].GetComponent<IItem>().isGetItem)
        {
            gameObject.SetActive(false);
        }
    }
}
