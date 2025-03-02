using cakeslice;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Pos_Interaction_MumeiFeather : MonoBehaviour
{
    string OpenText = "깃털에 깃든 힘이 포스기를 고친거 같아!"; // 열었을때 나올 텍스트
    string CloseText1 = "고쳐야 열 수 있을 것 같아.\n기계를 고쳐주는 깃털 같은게 있다면 좋겠는데."; // 잠겼을때 나올 텍스트
    string CloseText2 = "저번엔 깃털로 고쳤던 것 같은데\n이번에도 어디 있지 않을까?"; // 잠겼을때 나올 텍스트

    GameObject player;
    BoxCollider boxCollider;
    KiaraPosBox kiaraPosBox;

    TextController textController;
    Interaction_Gimics interaction;
    AudioSource audiosSource;

    bool isDonegimic = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        kiaraPosBox = gameObject.GetComponentInChildren<KiaraPosBox>();
        audiosSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        SceneStartSetting_KiaraPosBox();
        
    }


    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (ItemManager._instance.hotkeyItemIndex == 17)
                {

                    StartCoroutine(textController.SendText(OpenText));
                    boxCollider.enabled = false;
                    kiaraPosBox.boxCollider.enabled = true;
                    // 깃털을 돌려보냄
                    ItemManager._instance.ReturnItem(17);
                    ItemManager._instance.DeactivateItem(17);
                    interaction.run_Gimic = true;
                    isDonegimic = true;
                    GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
                    break;
                }
                else
                {
                    if (GameManager.instance.Episode_Round == 1)
                    {
                        if (!isDonegimic)
                        {
                            StartCoroutine(textController.SendText(CloseText1));
                            audiosSource.Play();
                        }
                        else
                        {
                            interaction.run_Gimic = true;
                            break;
                        }
                    }
                    else
                    {
                        if (!isDonegimic)
                        {
                            StartCoroutine(textController.SendText(CloseText2));
                            audiosSource.Play();
                        }
                        else
                        {
                            interaction.run_Gimic = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                //yield return new WaitUntil(() => interaction.run_Gimic == false);
                
                if(isDonegimic)
                {
                    interaction.run_Gimic = true;
                    break;
                }
                else
                {
                    interaction.run_Gimic = false;
                }
            }
        }
    }
    public void SceneStartSetting_KiaraPosBox()
    {
        if (interaction.run_Gimic)
        {
            boxCollider.enabled = false;
            kiaraPosBox.boxCollider.enabled = true;
            
            isDonegimic = true;
            gameObject.GetComponent<Pos_Interaction_MumeiFeather>().enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
            kiaraPosBox.boxCollider.enabled = false;
            StartCoroutine(WaitTouch());
        }
    }
}
