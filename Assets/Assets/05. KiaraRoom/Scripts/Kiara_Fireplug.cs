using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Fireplug : MonoBehaviour
{
    Interaction_Items items;
    string interaction_Text = "����ִ� ��ȭ���̾�.\n�ƹ��͵� ���� ����..";
    // Start is called before the first frame update
    void Start()
    {
        items = gameObject.GetComponent<Interaction_Items>();
        items.audioSource_Object = this.gameObject;
        items._text = interaction_Text;
    }
}
