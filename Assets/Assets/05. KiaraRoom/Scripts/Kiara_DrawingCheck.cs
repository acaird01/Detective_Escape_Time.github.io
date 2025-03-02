using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_DrawingCheck : MonoBehaviour
{
    BoxCollider boxCollider;
    Interaction_Gimics interaction;
    int episode_Round; // 현제 회차 저장할 변수

    // Start is called before the first frame update
    void Start()
    {
        episode_Round = GameManager.instance.Episode_Round;
        boxCollider = gameObject.GetComponentInChildren<BoxCollider>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        SceneStartSetting();
    }
    public void SaveDrawingData(bool savedata)
    {
        interaction.run_Gimic = savedata;
    }

    public void SceneStartSetting() // 받아온 데이터에 따른 초기 위치 설정
    {
        if (episode_Round == 1)
        {
            boxCollider.enabled = false;
        }
        else
        {
            if (interaction.run_Gimic)
            {
                boxCollider.enabled = false; // 이미 클리어 했으면 상호작용 x
            }
            else
            {
                boxCollider.enabled = true;
            }
        }
    }
}
