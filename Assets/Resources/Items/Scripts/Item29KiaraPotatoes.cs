using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item29KiaraPotatoes : MonoBehaviour,IItem
{
    public int Index { get; set; } = 29;
    public string Name { get; set; } = "Item_29_Kiara_Potatoes";
    public string Icon { get; set; } = "Items/Icons/Potatoes.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "����Ƣ��\n�� Ƣ�� �ٻ�ٻ��� ����Ƣ���̴�.";
}
