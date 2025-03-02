using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiara_Non_Button : MonoBehaviour
{
    Interaction_Items interaction_item;
    private void Start()
    {
        interaction_item = gameObject.GetComponent<Interaction_Items>();
        interaction_item._text = "왠지 누르면 후회할꺼 같은 버튼이다...\n다행히 고장난 듯 하다.";
        interaction_item.audioSource_Object = this.gameObject;
    }
}
