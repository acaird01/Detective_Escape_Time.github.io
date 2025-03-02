using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Kiara_SpawnChickenFromChairs : MonoBehaviour
{
    bool GimicMove = false; // true�� �� ���� �Ȱ�

    Kiara_Chair[] chairs;

    public Transform SpawnChickenPos; // ������ �Ҹ����� ��ġ
    public GameObject Item_Chicken; // ������ ġŲ

    int episode_Round; // ���� ȸ�� ������ ����
    bool[] chairs_LookFront; // ���ڵ��� �������� �ִ��� ���� bool��
    Interaction_Gimics interaction;
    void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        GimicMove = interaction.run_Gimic; // ��ͽ��࿩�θ� ���ӸŴ������� üũ�ؾߵǴµ�, interaction���� �޾��ذ��ϱ� ������ ��¿�� ���� �޾ƿ;��ҵ�
        //GimicMove = false; // �̰� �ӽ� �׽�Ʈ�� ���߿� �ݵ�� ���� ��!!!!! ������ �����ְ�
        episode_Round = GameManager.instance.Episode_Round;

        chairs = gameObject.GetComponentsInChildren<Kiara_Chair>();
        
        SceneStart_RndPosChairs();
        SceneStartSetting_KiaraPosBox(); // �����Ϳ��� �ҷ��� ��� ���¿� ���� �ʱ���ġ ����
        
        chairs_LookFront = new bool[chairs.Length];
        FirstChairs_BoolSet();
        StartCoroutine(WaitTouch());
    }

    private void Update()
    {
        CheckChairs_RotData();
    }
    IEnumerator WaitTouch() // ���⼱ ��� ������ true���� ���� �� ��ũ��Ʈ�� ����� �� gimicmove�� true�� �ɰ�
    {
        if (GimicMove == false )
        {
            yield return new WaitUntil(() => GimicMove == true);
            interaction.run_Gimic = GimicMove;
            //Instantiate(Item_Chicken, SpawnChickenPos);    
            Item_Chicken.gameObject.SetActive(true);

            if (episode_Round == 2)
            {
                SpawnChickenPos.GetComponent<AudioSource>().Play();
                GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
            }
        }

    }

    void FirstChairs_BoolSet()
    {
        for (int i = 0; i < chairs.Length; i++)
        {
            chairs_LookFront[i] = false;
        }
    }

    void CheckChairs_RotData() // ���ڰ� ���������� üũ
    {
        bool CheckGimicMove = false;

        for (int i = 0; i < chairs.Length; i++)
        {
            float chairRotation = chairs[i].transform.localEulerAngles.y;

            if ((i == 0 || i == 1 || i == 4 || i == 5 || i == 7) && chairRotation == 90f)
            {
                if (!chairs_LookFront[i])
                {
                    CheckGimicMove = true;
                    chairs_LookFront[i] = true;
                }
            }
            else if ((i == 2 || i == 3 || i == 6 || i == 8) && chairRotation == 270f)
            {
                if (!chairs_LookFront[i])
                {
                    CheckGimicMove = true;
                    chairs_LookFront[i] = true;
                }
            }
            else
            {
                if (chairs_LookFront[i])
                {
                    CheckGimicMove = true;
                    chairs_LookFront[i] = false;
                }
            }
        }

        if (CheckGimicMove)
        {
            GimicMove = chairs_LookFront.All(n => n == true);
        }
    }


    void SceneStart_RndPosChairs() // ó�� ������ �� ���ڵ� �����ϰ� ������
    {
        if (episode_Round == 1)
        {
            for (int i = 0; i < chairs.Length; i++)
            {
                if ((i == 0 || i == 1 || i == 4 || i == 5 || i == 7))
                {
                    chairs[i].GetComponent<Kiara_Chair>().transform.Rotate(new Vector3(0, 90, 0));
                    chairs[i].GetComponent<Kiara_Chair>().TouchNewRot_Y = 90;
                }
                else if ((i == 2 || i == 3 || i == 6 || i == 8))
                {
                    chairs[i].GetComponent<Kiara_Chair>().transform.Rotate(new Vector3(0, 270, 0));
                    chairs[i].GetComponent<Kiara_Chair>().TouchNewRot_Y = 270;
                }
            }
        }
        else
        {
            for (int i = 0; i < chairs.Length; i++)
            {
                int rndPos = Random.Range(1, 13); // 30 ~ 360������ ������ �������� ���ڸ� ����
                chairs[i].GetComponent<Kiara_Chair>().transform.Rotate(new Vector3(0, rndPos * 30, 0));
                chairs[i].GetComponent<Kiara_Chair>().TouchNewRot_Y = rndPos * 30;
            }
        }
    }
    public void SceneStartSetting_KiaraPosBox() // ���⼱ �ѹ��� ����Ǹ� �������� ��� ���̱⿡ ���� ����� ���θ� �����ð�
    {
        if (GimicMove)
        {
            transform.gameObject.GetComponent<Kiara_SpawnChickenFromChairs>().enabled = false;
        }
        else
        {
            if(episode_Round == 2)
                {
                if (Item_Chicken.gameObject.activeSelf)
                {
                    Item_Chicken.gameObject.SetActive(false);
                }
            }
        }
    }

}
