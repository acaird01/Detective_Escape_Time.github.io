using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_OuijaBoard : MonoBehaviour
{
    Interaction_Gimics interaction;
    GameObject player;

    [SerializeField]
    GameObject OuijaBoardCanvas;



    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    private void Start()
    {
        //settingGimic = false;
        player = GameObject.Find("Player");
        interaction = gameObject.GetComponent<Interaction_Gimics>();

        interaction.hintChild_OutLine = new cakeslice.Outline[1];
        interaction.hintChild_OutLine[0] = GameObject.Find("Cylinder.002").GetComponent<cakeslice.Outline>();

        if (interaction != null)
        {
            StartCoroutine(WaitTouch()); // 이건 문만 다른거할때 지우자!
        }

        Setting_SceneStart();

        OuijaBoardCanvas.SetActive(false);  //시작할때 끄기
    }

    void Setting_SceneStart()
    {
        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (interaction.run_Gimic)
        {
            OuijaBoardCanvas.GetComponent<Gura_OuijaBoardCanvas>().HintOff();

            if (ItemManager._instance.inventorySlots[16].GetComponent<IItem>().isGetItem == true)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                GetComponent<Gura_KroniiSword>().KroniiUp();    
            }
        }
        else
        {

        }
    }

    public IEnumerator WaitTouch()     //상호작용시 위자보드 캔버스 실행
    {
        if (interaction.run_Gimic == false)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true && OuijaBoardCanvas.GetComponent<Gura_OuijaBoardCanvas>().isAnswer == false);

            GetComponent<AudioSource>().Play();

            OuijaBoardCanvas.SetActive(true);

            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Gura_OuijaBoardCanvas OuijaCanvas = OuijaBoardCanvas.GetComponent<Gura_OuijaBoardCanvas>();
            player.GetComponentInChildren<PlayerCtrl>().keystrokes = true;

            interaction.run_Gimic = false;

            if (!OuijaCanvas.isOuijaCleared)
            {   
                StartCoroutine(WaitTouch());
            }
            else
            { 
                interaction.run_Gimic = true;

                GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
            }
        }
    }


}
