using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item34ChestKey : MonoBehaviour, IItem
{
    public int Index { get; set; } = 34;
    public string Name { get; set; } = "Item_34_ChestKey";
    public string Icon { get; set; } = "Items/Icons/ChestKey.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "�ڹ����� ����\n(�ƽ��Ե�) �ܽ����� �� �ɷ��ִ� ����� �ƴϴ�.";
}