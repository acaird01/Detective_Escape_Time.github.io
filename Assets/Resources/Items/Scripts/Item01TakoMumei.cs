using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item01TakoMumei : MonoBehaviour, IItem
{
    public int Index { get; set; } = 1;
    public string Name { get; set; } = "Item_01_TakoMumei";
    public string Icon { get; set; } = "Items/Icons/TakoMumei.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "Ÿ�� ������\n�������� ����� �� Ÿ�ڴ�ġ��.";
}
