using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_DrawingCheck : MonoBehaviour
{
    BoxCollider boxCollider;
    Interaction_Gimics interaction;
    int episode_Round; // ���� ȸ�� ������ ����

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

    public void SceneStartSetting() // �޾ƿ� �����Ϳ� ���� �ʱ� ��ġ ����
    {
        if (episode_Round == 1)
        {
            boxCollider.enabled = false;
        }
        else
        {
            if (interaction.run_Gimic)
            {
                boxCollider.enabled = false; // �̹� Ŭ���� ������ ��ȣ�ۿ� x
            }
            else
            {
                boxCollider.enabled = true;
            }
        }
    }
}
