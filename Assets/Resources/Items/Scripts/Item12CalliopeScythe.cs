using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item12CalliopeScythe : MonoBehaviour, IItem
{    public int Index { get; set; } = 12;
    public string Name { get; set; } = "Item_12_CalliopeScythe";
    public string Icon { get; set; } = "Items/Icons/CalliopeScythe.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "����� ��\n��ȥ�� ��Ȯ�ϴ� ����� ��.";
}