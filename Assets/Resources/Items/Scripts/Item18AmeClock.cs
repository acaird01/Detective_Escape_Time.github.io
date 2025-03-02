using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item18AmeClock : MonoBehaviour, IItem
{
    public int Index { get; set; } = 18;
    public string Name { get; set; } = "Item_18_AmeClock";
    public string Icon { get; set; } = "Items/Icons/AmeClock.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "�Ƹ��� ȸ�߽ð�\n�������κ��� ���� �ð������� �� �� �ְ� ���ִ� �ð��.";
}
