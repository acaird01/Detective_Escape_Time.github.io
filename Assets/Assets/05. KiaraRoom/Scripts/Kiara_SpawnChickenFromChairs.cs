using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Kiara_SpawnChickenFromChairs : MonoBehaviour
{
    bool GimicMove = false; // true일 때 실행 된거

    Kiara_Chair[] chairs;

    public Transform SpawnChickenPos; // 성공시 소리나는 위치
    public GameObject Item_Chicken; // 생성될 치킨

    int episode_Round; // 현제 회차 저장할 변수
    bool[] chairs_LookFront; // 의자들이 앞을보고 있는지 담을 bool값
    Interaction_Gimics interaction;
    void Start()
    {
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        GimicMove = interaction.run_Gimic; // 기믹실행여부를 게임매니저에서 체크해야되는데, interaction에서 받아해결하기 때문에 어쩔수 없이 받아와야할듯
        //GimicMove = false; // 이거 임시 테스트용 나중에 반드시 지울 것!!!!! 위에꺼 열어주고
        episode_Round = GameManager.instance.Episode_Round;

        chairs = gameObject.GetComponentsInChildren<Kiara_Chair>();
        
        SceneStart_RndPosChairs();
        SceneStartSetting_KiaraPosBox(); // 데이터에서 불러온 기믹 상태에 따른 초기위치 설정
        
        chairs_LookFront = new bool[chairs.Length];
        FirstChairs_BoolSet();
        StartCoroutine(WaitTouch());
    }

    private void Update()
    {
        CheckChairs_RotData();
    }
    IEnumerator WaitTouch() // 여기선 모든 의자의 true값이 들어와 이 스크립트가 실행될 때 gimicmove가 true가 될것
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

    void CheckChairs_RotData() // 의자가 제방향인지 체크
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


    void SceneStart_RndPosChairs() // 처음 시작할 때 의자들 랜덤하게 돌리기
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
                int rndPos = Random.Range(1, 13); // 30 ~ 360도까지 랜덤한 방향으로 의자를 스폰
                chairs[i].GetComponent<Kiara_Chair>().transform.Rotate(new Vector3(0, rndPos * 30, 0));
                chairs[i].GetComponent<Kiara_Chair>().TouchNewRot_Y = rndPos * 30;
            }
        }
    }
    public void SceneStartSetting_KiaraPosBox() // 여기선 한번만 실행되면 아이템을 얻고 끝이기에 실행 됬는지 여부만 가져올것
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
