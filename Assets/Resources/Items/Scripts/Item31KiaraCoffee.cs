using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item31KiaraCoffee : MonoBehaviour,IItem
{
    public int Index { get; set; } = 31;
    public string Name { get; set; } = "Item_31_Kiara_Coffee";
    public string Icon { get; set; } = "Items/Icons/Coffee.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "커피\n따뜻한 커피. 마시고 점장님의 방송을 보자.";
}
