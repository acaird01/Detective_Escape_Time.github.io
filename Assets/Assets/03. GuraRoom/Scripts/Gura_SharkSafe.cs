using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gura_SharkSafe : MonoBehaviour
{
    public int SafeAnswerInt = 0;
    public int pressedNum;

    [SerializeField]
    private GameObject[] SharkTeeth;
    [SerializeField]
    private GameObject SharkSafeHint;
    [SerializeField]
    private GameObject PoseidonCase;

    [SerializeField]
    private AudioClip CorrectSound;
    [SerializeField]
    private AudioClip WrongSound;
    [SerializeField]
    GameObject flashEffect;

    [SerializeField]
    GameObject SharkSafeHintColl;

    private void Start()
    {
        SafeAnswerInt = 0;

        Setting_SceneStart();

        StartCoroutine(OpenSafe());
    }

    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    void Setting_SceneStart()
    {
        //find component Interaction_Gimics
        Interaction_Gimics interaction_Gimics = gameObject.GetComponent<Interaction_Gimics>();

        // 내가 추가한거
        /*interaction_Gimics.hintChild = new GameObject[1];
        interaction_Gimics.hintChild[0] = GameObject.Find("GuraCase");*/

        // 수정한 아웃라인
        interaction_Gimics.hintChild_OutLine = new cakeslice.Outline[1];
        interaction_Gimics.hintChild_OutLine[0] = GameObject.Find("GuraCase").GetComponent<cakeslice.Outline>();

        // 씬 시작했을 때 settingGmimic의 true false값을 토대로 초기 위치 설정
        if (interaction_Gimics.run_Gimic)
        {
            //힌트 제공
            SharkSafeHint.SetActive(true);

            SharkSafeHintColl.SetActive(false);
        }
        else
        {

        }
    }



    private void Update()
    {
        if (pressedNum == 4 && SafeAnswerInt != 102)
        {
            pressedNum = 0;
            SafeAnswerInt = 0;

            PlayWrongSound();

            for (int i = 0; i < SharkTeeth.Length; i++)
            {
                SharkTeeth[i].SetActive(true);
            }


        }
        else if (pressedNum == 4 && SafeAnswerInt == 102)
        {
            pressedNum = 0;
            for (int i = 0; i < SharkTeeth.Length; i++)
            {
                SharkTeeth[i].SetActive(true);
            }
        }
        
    }

    private void PlayWrongSound()
    {
        GetComponent<AudioSource>().clip = WrongSound;
        GetComponent<AudioSource>().Play();
    }



    IEnumerator OpenSafe()
    {
        yield return new WaitUntil(() => SafeAnswerInt == 102);

        GetComponent<Interaction_Gimics>().run_Gimic = true;

        yield return new WaitForSeconds(1.5f);

        SharkSafeHint.SetActive(true);  

        GetComponent<AudioSource>().clip = CorrectSound;
        GetComponent<AudioSource>().Play();

        flashEffect.SetActive(true);
        flashEffect.GetComponent<ParticleSystem>().Play();

        SharkSafeHint.GetComponent<Animator>().Play("SharkSafeHintShow");

        SharkSafeHintColl.SetActive(false);

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();

    }

    

}
