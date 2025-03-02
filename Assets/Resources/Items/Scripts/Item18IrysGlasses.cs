using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item18IrysGlasses : MonoBehaviour, IItem
{
    public int Index { get; set; } = 18;
    public string Name { get; set; } = "Item_18_IRySGlasses";
    public string Icon { get; set; } = "Items/Icons/IRySGlasses.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "아이리스의 안경\n아이리스가 애용하는 안경이다.";
}