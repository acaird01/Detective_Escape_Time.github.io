using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gura_Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Slot의 자식의 갯수가 0 이라는 의미는
        // 하위 오브젝트가 없는 경우를 말한다
        if (transform.childCount == 0)
        {
            Gura_Drag.draggingEN2.transform.SetParent(this.transform);
        }
    }
}
