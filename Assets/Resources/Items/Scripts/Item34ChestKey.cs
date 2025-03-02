using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item34ChestKey : MonoBehaviour, IItem
{
    public int Index { get; set; } = 34;
    public string Name { get; set; } = "Item_34_ChestKey";
    public string Icon { get; set; } = "Items/Icons/ChestKey.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "자물쇠의 열쇠\n(아쉽게도) 햄스터의 목에 걸려있던 열쇠는 아니다.";
}