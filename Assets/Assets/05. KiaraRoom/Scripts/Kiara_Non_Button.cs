using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Non_Button : MonoBehaviour
{
    Interaction_Items interaction_item;
    private void Start()
    {
        interaction_item = gameObject.GetComponent<Interaction_Items>();
        interaction_item._text = "���� ������ ��ȸ�Ҳ� ���� ��ư�̴�...\n������ ���峭 �� �ϴ�.";
        interaction_item.audioSource_Object = this.gameObject;
    }
}
