using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item10IrysHalo : MonoBehaviour, IItem
{
    public int Index { get; set; } = 10;
    public string Name { get; set; } = "Item_10_IrysHalo";
    public string Icon { get; set; } = "Items/Icons/IrysHalo.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "��¦�̴� ���Ϸ�\n������ó�� ����� �� ���� ���� ���Ϸδ�.\n[ Space Bar�� ���� ����ϱ� ]";
}
