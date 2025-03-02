using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_TreeBed : MonoBehaviour
{
    Interaction_Gimics interaction; // 얘는 기믹 수행 여부 저장할 필요 x 부모에서 처리함. 여기서 interaction은 단지 상호작용을 위해 받는것 뿐
    GameObject player;
    TextController textController;

    string before_Growth_Text = "저번에 창문으로 볼 땐 나무가 있었던거 같은데.\n뭔가가 있다면 자라게 할 수 있지 않을까?"; // 성장하기 전에 상호작용시 나올 텍스트

    public bool Growth_Tree_True; // true일 때 나무 자란거

    Kiara_StructureTree structureTree;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        interaction = gameObject.GetComponentInChildren<Interaction_Gimics>();
        Growth_Tree_True = interaction.run_Gimic;
        structureTree = gameObject.GetComponentInParent<Kiara_StructureTree>();
        structureTree.SceneStartSetting_StructureTree();
        //interaction.run_Gimic = false; // 이게 진짜 테스트용!!!!!!!!!!!!!!!!!!!
        StartCoroutine(WaitTouch());
        if (!Growth_Tree_True)
        {
            StartCoroutine(Growth_TreeWait());
        }
    }


    IEnumerator Growth_TreeWait() // 조건 만족하면 나무 자라게 하기
    {
        yield return new WaitUntil(() => Growth_Tree_True == true);
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        ItemManager._instance.ReturnItem(15);
        ItemManager._instance.DeactivateItem(15);
        StartCoroutine(structureTree.GrowthTree());
    }

    IEnumerator WaitTouch() // 나무가 다 자란 상태에서 상호작용 할 예정 자라기전엔 콜라이더 끔으로써 상호작용 x
    {
        while (player)
        {
            if (interaction.run_Gimic == false && Growth_Tree_True == false) // 나무가 자라지 않았을 때
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if(ItemManager._instance.hotkeyItemIndex == 15)
                {
                    Growth_Tree_True = true;
                }
                StartCoroutine(textController.SendText(before_Growth_Text));
            }
            else if(interaction.run_Gimic == true && Growth_Tree_True == false)
            {
                interaction.run_Gimic = false;
            }
            if(Growth_Tree_True == true)
            {
                interaction.run_Gimic = true;
                break;
            }
        }
    }
}
