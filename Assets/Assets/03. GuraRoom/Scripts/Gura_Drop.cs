using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gura_Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Slot�� �ڽ��� ������ 0 �̶�� �ǹ̴�
        // ���� ������Ʈ�� ���� ��츦 ���Ѵ�
        if (transform.childCount == 0)
        {
            Gura_Drag.draggingEN2.transform.SetParent(this.transform);
        }
    }
}
