using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KFP_Emplotee_Gimic : MonoBehaviour
{
    GameObject player;
    bool GimicMove = false; // true일 때 실행 된거

    public GameObject kiara_Feather_Prefab; // 깃털 프리팹

    TextController textController;
    Interaction_Gimics interaction;
    BoxCollider boxCollider;
    public GameObject FireEffect;

    string interaction_Text = "구해주셔서 감사합니다.\n이것을 드릴께요."; // 상호작용 시 출력할 텍스트

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // SceneManager.sceneLoaded 델리게이트 체인을 이용해
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 처음 로드 되었을 때 호출하기
    {
        kiara_Feather_Prefab.gameObject.SetActive(false);
        FireEffect.gameObject.SetActive(false);
        boxCollider = gameObject.GetComponent<BoxCollider>();
        //gameObject.GetComponent<Interaction_Gimics>().run_Gimic = false; // 이거 나중에 반드시 지울 것 테스트용!!!!!!!!!!!!!!!!!!!!!!!!!
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        GimicMove = interaction.run_Gimic;
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        SceneStartSetting_KiaraPosBox();
        StartCoroutine(WaitTouch());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        GimicMove = interaction.run_Gimic;

    }
    IEnumerator WaitTouch()
    {
        if (GimicMove == false)
        {
            yield return new WaitUntil(() => GimicMove == true);
            boxCollider.enabled = false;
            StartCoroutine(textController.SendText(interaction_Text));

            FireEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            kiara_Feather_Prefab.gameObject.SetActive(true);
            interaction.run_Gimic = true;
            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
            yield return new WaitForSeconds(0.5f);
            FireEffect.gameObject.SetActive(false);
            interaction.enabled = false;
        }
    }
    public void SceneStartSetting_KiaraPosBox()
    {
        if (GimicMove)
        {
            interaction.enabled = false;
        }
        else
        {
            interaction.enabled = true;
        }
    }
}
