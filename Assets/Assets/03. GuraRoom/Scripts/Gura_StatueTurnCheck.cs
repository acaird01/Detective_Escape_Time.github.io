    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gura_StatueTurnCheck : MonoBehaviour
{
    [SerializeField]
    GameObject[] statuesModel;

    [SerializeField]
    GameObject[] statues;

    public bool isAllStatueCorrect = false;

    [SerializeField]
    GameObject ChestTop;
    [SerializeField]
    GameObject takoMumei;

    private void Start()
    {
        StartCoroutine(StatueCorrectCheck());
    }

    private void Update()
    {
        //find statues from child of statuesModel
        statues = new GameObject[statuesModel.Length];
        for (int i = 0; i < statuesModel.Length; i++)
        {
            if (statuesModel[i].transform.childCount > 0)
            {
                statues[i] = statuesModel[i].transform.GetChild(0).gameObject;
            }
        }

        //check statues' component statueTurnScript's isStatueCorrect is true
        if (statues.Length >= 5 &&
            statues[0] != null && statues[0].GetComponent<Gura_StatueTurnScirpt>() != null && statues[0].GetComponent<Gura_StatueTurnScirpt>().isStatueCorrect &&
            statues[1] != null && statues[1].GetComponent<Gura_StatueTurnScirpt>() != null && statues[1].GetComponent<Gura_StatueTurnScirpt>().isStatueCorrect &&
            statues[2] != null && statues[2].GetComponent<Gura_StatueTurnScirpt>() != null && statues[2].GetComponent<Gura_StatueTurnScirpt>().isStatueCorrect &&
            statues[3] != null && statues[3].GetComponent<Gura_StatueTurnScirpt>() != null && statues[3].GetComponent<Gura_StatueTurnScirpt>().isStatueCorrect &&
            statues[4] != null && statues[4].GetComponent<Gura_StatueTurnScirpt>() != null && statues[4].GetComponent<Gura_StatueTurnScirpt>().isStatueCorrect)
        {
            isAllStatueCorrect = true;
        }
        else
        {
            isAllStatueCorrect = false;
        }
    }


    IEnumerator StatueCorrectCheck()
    {
        yield return new WaitUntil(() => isAllStatueCorrect == true);

        Interaction_Gimics interaction = gameObject.GetComponent<Interaction_Gimics>();
        interaction.run_Gimic= true;

        if (ItemManager._instance.inventorySlots[1].GetComponent<IItem>().isGetItem == false)
        {
            ChestTop.GetComponent<Animator>().Play("ChestResult");
            ChestTop.GetComponent<AudioSource>().Play();
            takoMumei.SetActive(true);
            yield return new WaitForSeconds(3f);
            ChestTop.GetComponent<AudioSource>().Stop();
            ChestTop.GetComponent<BoxCollider>().enabled = false;

            GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
        }
    }

    public void ChestResult()
    {
        ChestTop.GetComponent<Animator>().Play("ChestResult");
    }


}
