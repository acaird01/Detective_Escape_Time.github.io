using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item20Skull : MonoBehaviour, IItem
{
    public int Index { get; set; } = 20;
    public string Name { get; set; } = "Item_20_Skull";
    public string Icon { get; set; } = "Items/Icons/Skull.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "��������� �ΰ���\n��������� �ΰ����̴�. ��� ���� ����?";
}
