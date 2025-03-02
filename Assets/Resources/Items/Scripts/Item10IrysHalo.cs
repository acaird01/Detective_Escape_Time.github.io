using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item10IrysHalo : MonoBehaviour, IItem
{
    public int Index { get; set; } = 10;
    public string Name { get; set; } = "Item_10_IrysHalo";
    public string Icon { get; set; } = "Items/Icons/IrysHalo.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "반짝이는 헤일로\n손전등처럼 사용할 수 있을 듯한 헤일로다.\n[ Space Bar를 눌려 사용하기 ]";
}
