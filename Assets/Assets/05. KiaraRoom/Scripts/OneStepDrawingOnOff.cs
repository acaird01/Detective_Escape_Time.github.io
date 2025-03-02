using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class OneStepDrawingOnOff : MonoBehaviour
{
    public GameObject NodePrefab; // 한붓그리기 노드들이 들어있는 프리팹
    Interaction_Gimics interaction;
    GameObject player;
    Gimic_One_StepDrawing stepDrawing;

    TextController textConteroller;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        stepDrawing = gameObject.GetComponentInChildren<Gimic_One_StepDrawing>();
        player = GameObject.Find("Player");
        textConteroller = player.GetComponentInChildren<TextController>();
        StartCoroutine(WaitTouch());
        NodePrefab.SetActive(false);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                textConteroller.SetActiveFalseText();
                NodePrefab.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
            {
                yield return new WaitUntil(() => interaction.run_Gimic == false);
            }
        }
    }
    public void CanvasClose() // x버튼에 들어갈꺼
    {
        Time.timeScale = 1;
        NodePrefab.SetActive(false);
        stepDrawing.ResetData();
        Cursor.lockState = CursorLockMode.Locked;
        interaction.run_Gimic = false;
    }
}
