using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item18IrysGlasses : MonoBehaviour, IItem
{
    public int Index { get; set; } = 18;
    public string Name { get; set; } = "Item_18_IRySGlasses";
    public string Icon { get; set; } = "Items/Icons/IRySGlasses.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "���̸����� �Ȱ�\n���̸����� �ֿ��ϴ� �Ȱ��̴�.";
}