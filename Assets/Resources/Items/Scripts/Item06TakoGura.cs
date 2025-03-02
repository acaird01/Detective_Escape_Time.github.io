using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item06TakoGura : MonoBehaviour, IItem
{
    public int Index { get; set; } = 6;
    public string Name { get; set; } = "Item_06_TakoGura";
    public string Icon { get; set; } = "Items/Icons/TakoGura.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "Ÿ�� ����\n������ ����� �� Ÿ�ڴ�ġ��.";
}
