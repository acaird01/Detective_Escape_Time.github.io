using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item11GuraTrident : MonoBehaviour, IItem
{
    public int Index { get; set; } = 11;
    public string Name { get; set; } = "Item_11_GuraTrident";
    public string Icon { get; set; } = "Items/Icons/GuraTrident.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "����â\n��Ʋ��Ƽ���� ����â�̴�.";

}
