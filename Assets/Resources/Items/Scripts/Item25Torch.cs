using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item25Torch : MonoBehaviour, IItem
{
    public int Index { get; set; } = 25;
    public string Name { get; set; } = "Item_25_Torch";
    public string Icon { get; set; } = "Items/Icons/Torch.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "ȶ��\n���� �� ���� �� ���� �����";
}