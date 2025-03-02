using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calli_OldCloth : MonoBehaviour
{
    Material material;  // ������ ���׸���
    Color startColor; // õ ���� ��
    Color targetColor; // õ�� ���������� �ٲ� ��
    public float duration = 3f;  // ��ȭ �ð�
    string interactionText = "���� õ�� ���� �����־��� �� ����.\nũ�δ��� ������ �ǵ��� �� ������?"; // Į ���� ��ȣ�ۿ� ���� �� ���� �ؽ�Ʈ

    GameObject player;
    bool GimicMove = false; // true�� �� ���� �Ȱ�

    BoxCollider boxCollider;
    TextController textController;
    Interaction_Gimics interaction;

    SpriteRenderer takoImage; // ���� Ÿ�� �̹��� ��Ʈ?
    //Material takoImage_Mat; // Ÿ���̹����� ���׸���
    Color takoImage_mat_start;
    Color takoImage_mat_target; // Ÿ�� �̹��� ���� ���̰� �ϱ����� ���׸��� -> ���׸������� �˾Ҵµ� ��������Ʈ �������������� �̸� �ٲٱ� �������� ����

    void Start()
    {
        player = GameObject.Find("Player");
        material = GetComponent<MeshRenderer>().material;
        boxCollider = GetComponent<BoxCollider>();
        interaction = gameObject.GetComponent<Interaction_Gimics>();
        takoImage = gameObject.GetComponentInChildren<SpriteRenderer>();
        //takoImage_Mat = takoImage.GetComponent<Material>();
        GimicMove =interaction.run_Gimic;
        textController = player.GetComponentInChildren<TextController>();
        
        SceneStartSetting_KiaraPosBox();
        StartCoroutine(WaitTouch());
    }

    
    IEnumerator WaitTouch()
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);

                if (ItemManager._instance.hotkeyItemIndex == 16)
                {
                    GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate(); // ��� ���� ������ ȣ��

                    StartCoroutine(ChangeColorOverTime());
                    boxCollider.enabled = false;
                    break;
                }
                else
                {
                    StartCoroutine(textController.SendText(interactionText));
                    interaction.run_Gimic = false;
                }
            }
            else
            {
                interaction.run_Gimic = false;
            }
        }
    }
    IEnumerator ChangeColorOverTime()
    {
        float elapsedTime = 0f;

        startColor = new Color(material.color.r, material.color.g, material.color.b, material.color.a);
        targetColor = new Color(material.color.r, 255f / 255f, 255f / 255f, material.color.a);

        takoImage.gameObject.SetActive(true);
        takoImage_mat_start = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 0 / 255f); 
        takoImage_mat_target = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 255 / 255f);

        while (elapsedTime < duration) // õ�� ���� �ʷϻ��̵ǰ�, Ÿ�� ���� ���� �÷��� ���̰� �ϱ�
        {
            material.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            takoImage.color = Color.Lerp(takoImage_mat_start, takoImage_mat_target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;  
        }

        material.color = targetColor;
        takoImage.color = takoImage_mat_target;

        // ���൵ ������Ʈ
        GameObject.FindAnyObjectByType<Number_if_Gimic>().TextUpdate();
    }

    public void SceneStartSetting_KiaraPosBox()
    {
        if (GimicMove) // ���������� Ÿ�� �̹��� ���� �� ���, ��ȣ�ۿ� x ���� �ݴ�
        {
            takoImage.color = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 255 / 255f);
            material.color = new Color(material.color.r, 255f / 255f, 255f / 255f, material.color.a);
            boxCollider.enabled = false;
            takoImage.gameObject.SetActive(true);
        }
        else
        {
            takoImage.color = new Color(takoImage.color.r, takoImage.color.g, takoImage.color.b, 0 / 255f);
            material.color = new Color(material.color.r, 128f / 255f, 128f / 255f, material.color.a);
            boxCollider.enabled = true;
            takoImage.gameObject.SetActive(false);
        }
    }
}
