using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kiara_Fauna_branch : MonoBehaviour
{
    /*private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
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
