using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gura_CrackedPillar : MonoBehaviour
{

    Interaction_Gimics interaction;
    GameObject player;
    TextController textController;

    public bool isBaelzDIce = false;

    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    GameObject smokePrefab;

    [SerializeField]
    GameObject BrokenPillar;
    [SerializeField]
    GameObject takoBaelz;
    [SerializeField]
    GameObject takoBaelzHint;

    [SerializeField]
    AudioSource audioExplosion;

    [TextArea]
    [SerializeField]
    string PillarText;
    

    private bool settingGimic { get; set; }
    public bool SettingForObjectToInteration
    {
        get { return settingGimic; }
        set { settingGimic = value; }
    }

    bool isPillarDone = false;

    private void OnEnable()
    {
        // This will be called every time the object is activated
        player = GameObject.Find("Player");
        interaction = gameObject.GetComponent<Interaction_Gimics>();

        textController = player.GetComponentInChildren<TextController>();

        Setting_SceneStart();

        if (interaction != null && gameObject.activeSelf)
        {
            StartCoroutine(WaitTouch());
        }

        

    }


    void Setting_SceneStart()
    {
        BrokenPillar.SetActive(false);


            //Å¸ÄÚº§ Ã£¾Æ¼­ ÀÖÀ¸¸é ²¨ÁÜ. Å¸ÄÚº§ ¸ÀÀÖ°Ú´Ù ÈæÈæ
            if (ItemManager._instance.inventorySlots[5].GetComponent<IItem>().isGetItem == true)
            {
                BrokenPillar.SetActive(true);

                takoBaelzHint.SetActive(true);

                isPillarDone = true;

                takoBaelz.SetActive(false);

                gameObject.SetActive(false);

            }
    }


    IEnumerator WaitTouch()
    {
        if (ItemManager._instance.inventorySlots[5].GetComponent<IItem>().isGetItem == false)
        {
            yield return new WaitUntil(() => (interaction.run_Gimic) == true);

            if (ItemManager._instance.hotkeyItemIndex == 14)
            {
                isBaelzDIce = true;
            }

            if (isBaelzDIce)
            {
                if (ItemManager._instance.inventorySlots[5].GetComponent<IItem>().isGetItem == false)
                {
                    if (isPillarDone == false)
                    {
                        audioExplosion.Play();
                    }

                    yield return new WaitForSeconds(0.5f);

                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                    gameObject.SetActive(false);

                    BrokenPillar.SetActive(true);

                    WaitForSeconds wait = new WaitForSeconds(1.5f);

                    takoBaelz.SetActive(true);

                    Instantiate(smokePrefab, transform.position, Quaternion.identity);

                    takoBaelzHint.SetActive(true);

                    GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
                }
                else
                {
                    BrokenPillar.SetActive(true);

                    takoBaelzHint.SetActive(true);

                    takoBaelz.SetActive(false);

                    gameObject.SetActive(false);
                }

            }
            else
            {
                interaction.run_Gimic = false;

                StartCoroutine(textController.SendText(PillarText));

                StartCoroutine(WaitTouch());
            }
        }

    }
}


