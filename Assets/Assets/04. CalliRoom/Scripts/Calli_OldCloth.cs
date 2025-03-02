using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calli_OldCloth : MonoBehaviour
{
    Material material;  // 변경할 메테리얼
    Color startColor; // 천 시작 색
    Color targetColor; // 천이 최종적으로 바뀔 색
    public float duration = 3f;  // 변화 시간
    string interactionText = "낡은 천에 뭔가 쓰여있었던 것 같다.\n크로니의 힘으로 되돌릴 수 있을까?"; // 칼 없이 상호작용 했을 때 나올 텍스트

    GameObject player;
    bool GimicMove = false; // true일 때 실행 된거

    BoxCollider boxCollider;
    TextController textController;
    Interaction_Gimics interaction;

    SpriteRenderer takoImage; // 나올 타코 이미지 힌트?
    //Material takoImage_Mat; // 타코이미지의 메테리얼
    Color takoImage_mat_start;
    Color takoImage_mat_target; // 타코 이미지 점점 보이게 하기위한 메테리얼값 -> 메테리얼인줄 알았는데 스프라이트 랜더러였음ㅋㅋ 이름 바꾸기 귀찮은데 냅둠

    void Start()
    {
        player = GameObject.Find("Player");
        material = GetComponent<MeshRenderer>().material;
        boxCollider = GetComponent<BoxCollider>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        takoImage = gameObject.GetComponentInChildren<SpriteRenderer>();
        //takoImage_Mat = takoImage.GetComponent<Material>();
        GimicMove =interaction.run_Gimic;
        textController = player.GetComponentInChildren<TextController>();
        
        SceneStartSetting_KiaraPosBox();
        StartCoroutine(WaitTouch());
    }

    
    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);

                if (ItemManager._instance.hotkeyItemIndex == 16)
                {
                    GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // 기믹 종료 시점에 호출

                    StartCoroutine(ChangeColorOverTime());
                    boxCollider.enabled = false;
                    break;
                }
                else
                {
                    StartCoroutine(textController.SendText(interactionText));
                    interaction.run_Gimic = false;
                }
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }
    IEnumerator ChangeColorOverTime()
    {
        float elapsedTime = 0f;

        startColor = new Color(material.color.r, material.color.g, material.color.b, material.color.a);
        targetColor = new Color(material.color.r, 255f / 255f, 255f / 255f, material.color.a);

        takoImage.gameObject.SetActive(true);
        takoImage_mat_start = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 0 / 255f); 
        takoImage_mat_target = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 255 / 255f);

        while (elapsedTime < duration) // 천은 점점 초록색이되고, 타코 투명도 점점 올려서 보이게 하기
        {
            material.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            takoImage.color = Color.Lerp(takoImage_mat_start, takoImage_mat_target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;  
        }

        material.color = targetColor;
        takoImage.color = takoImage_mat_target;

        // 진행도 업데이트
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
    }

    public void SceneStartSetting_KiaraPosBox()
    {
        if (GimicMove) // 수행됬으면 타코 이미지 띄우고 색 밝게, 상호작용 x 밑은 반대
        {
            takoImage.color = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 255 / 255f);
            material.color = new Color(material.color.r, 255f / 255f, 255f / 255f, material.color.a);
            boxCollider.enabled = false;
            takoImage.gameObject.SetActive(true);
        }
        else
        {
            takoImage.color = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 0 / 255f);
            material.color = new Color(material.color.r, 128f / 255f, 128f / 255f, material.color.a);
            boxCollider.enabled = true;
            takoImage.gameObject.SetActive(false);
        }
    }
}
