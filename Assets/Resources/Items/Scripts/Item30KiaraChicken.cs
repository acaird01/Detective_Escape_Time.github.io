using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item30KiaraChicken : MonoBehaviour, IItem
{
    public int Index { get; set; } = 30;
    public string Name { get; set; } = "Item_30_Kiara_Chicken";
    public string Icon { get; set; } = "Items/Icons/Chicken.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "KFP ġŲ\n���־� ���̴� ġŲ�̴�.\n(����� ������ Ƣ�� ���� �����ϴ�.)";
}
