using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item28KiaraBurger : MonoBehaviour,IItem
{
    public int Index { get; set; } = 28;
    public string Name { get; set; } = "Item_28_Kiara_Burger";
    public string Icon { get; set; } = "Items/Icons/Burger.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "KFP ����\n���־� ���̴� �ܹ��Ŵ�.";
}
