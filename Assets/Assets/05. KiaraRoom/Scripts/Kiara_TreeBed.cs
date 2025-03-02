using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_TreeBed : MonoBehaviour
{
    Interaction_Gimics interaction; // ��� ��� ���� ���� ������ �ʿ� x �θ𿡼� ó����. ���⼭ interaction�� ���� ��ȣ�ۿ��� ���� �޴°� ��
    GameObject player;
    TextController textController;

    string before_Growth_Text = "������ â������ �� �� ������ �־����� ������.\n������ �ִٸ� �ڶ�� �� �� ���� ������?"; // �����ϱ� ���� ��ȣ�ۿ�� ���� �ؽ�Ʈ

    public bool Growth_Tree_True; // true�� �� ���� �ڶ���

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
        //interaction.run_Gimic = false; // �̰� ��¥ �׽�Ʈ��!!!!!!!!!!!!!!!!!!!
        StartCoroutine(WaitTouch());
        if (!Growth_Tree_True)
        {
            StartCoroutine(Growth_TreeWait());
        }
    }


    IEnumerator Growth_TreeWait() // ���� �����ϸ� ���� �ڶ�� �ϱ�
    {
        yield return new WaitUntil(() => Growth_Tree_True == true);
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        ItemManager._instance.ReturnItem(15);
        ItemManager._instance.DeactivateItem(15);
        StartCoroutine(structureTree.GrowthTree());
    }

    IEnumerator WaitTouch() // ������ �� �ڶ� ���¿��� ��ȣ�ۿ� �� ���� �ڶ������ �ݶ��̴� �����ν� ��ȣ�ۿ� x
    {
        while (player)
        {
            if (interaction.run_Gimic == false && Growth_Tree_True == false) // ������ �ڶ��� �ʾ��� ��
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
