using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_StatueAnswerScript : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    GameObject[] StatueAnswer;  // Slots where statues are placed (UI)

    [SerializeField]
    GameObject[] StatueCorrectAnswer;  // Correct answer statues for comparison

    [SerializeField]
    GameObject result;

    public bool isStatueCorrect = false;

    [SerializeField]
    GameObject[] statueSpawnPoints;

    [SerializeField]
    GameObject[] statuesPrefab;

    [SerializeField]
    GameObject StatueAnswerInput;


    private void OnEnable()
    {
        player = GameObject.Find("Player");

        StartCoroutine(ShowResult());

    }

    private void Update()
    {
        CheckStatueAnswer();
        SpawnMatchingStatue();
    }

    private void CheckStatueAnswer()
    {
        GameObject[] StatueAnswerChild = new GameObject[StatueAnswer.Length];

        for (int i = 0; i < StatueAnswer.Length; i++)
        {
            if (StatueAnswer[i].transform.childCount > 0)
            {
                StatueAnswerChild[i] = StatueAnswer[i].transform.GetChild(0).gameObject;
            }
        }

        if (StatueAnswerChild[0] != null && StatueAnswerChild[1] != null && StatueAnswerChild[2] != null && StatueAnswerChild[3] != null && StatueAnswerChild[4] != null)
        {
            // Compare StatueAnswerChild with StatueCorrectAnswer
            if (StatueAnswerChild[0] == StatueCorrectAnswer[0].gameObject &&
                StatueAnswerChild[1] == StatueCorrectAnswer[1].gameObject &&
                StatueAnswerChild[2] == StatueCorrectAnswer[2].gameObject &&
                StatueAnswerChild[3] == StatueCorrectAnswer[3].gameObject &&
                StatueAnswerChild[4] == StatueCorrectAnswer[4].gameObject)
            {
                isStatueCorrect = true;
            }

        }
    }

    private void SpawnMatchingStatue()
    {
        for (int i = 0; i < StatueAnswer.Length; i++)
        {
            if (StatueAnswer[i].transform.childCount > 0)
            {
                GameObject statueChild = StatueAnswer[i].transform.GetChild(0).gameObject;
                string statueChildName = statueChild.name;

                for (int j = 0; j < statuesPrefab.Length; j++)
                {
                    if (statuesPrefab[j].name == statueChildName)
                    {
                        if (statueSpawnPoints[i].transform.childCount == 0)
                        {
                            GameObject statue = Instantiate(statuesPrefab[j], statueSpawnPoints[i].transform);
                            statue.transform.SetParent(statueSpawnPoints[i].transform);
                        }
                        break;
                    }
                }
            }
        }

        //해당하는 조각상이 없으면 스폰포인트의 자식을 삭제
        for (int i = 0; i < statueSpawnPoints.Length; i++)
        {
            if (StatueAnswer[i].transform.childCount == 0)
            {
                if (statueSpawnPoints[i].transform.childCount > 0)
                {
                    Destroy(statueSpawnPoints[i].transform.GetChild(0).gameObject);
                }
            }
        }
    }

    IEnumerator ShowResult()
    {
        yield return new WaitUntil(() => isStatueCorrect == true);

        result.GetComponent<AudioSource>().Play();
        result.GetComponent<Animator>().Play("StatueResult");
        for (int i = 0; i < result.GetComponentsInChildren<Outline>().Length; i++)
        {
            result.GetComponentsInChildren<Outline>()[i].enabled = true;
        }
        Interaction_Gimics interaction = StatueAnswerInput.GetComponent<Interaction_Gimics>();
        interaction.run_Gimic = true;

        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();

        closeSafeCanvas();
    }


    public void StatueResult()
    {
        for (int i = 0; i < statueSpawnPoints.Length; i++)
        {
            GameObject statue = Instantiate(StatueCorrectAnswer[i], statueSpawnPoints[i].transform);
            statue.transform.SetParent(statueSpawnPoints[i].transform);
        }

        result.GetComponent<Animator>().Play("StatueResult");
    }

    public void closeSafeCanvas()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponentInChildren<PlayerCtrl>().keystrokes = false;
    }
}
