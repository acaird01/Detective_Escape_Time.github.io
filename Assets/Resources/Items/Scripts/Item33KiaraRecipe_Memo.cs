using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item33KiaraRecipe_Memo : MonoBehaviour,IItem
{
    public int Index { get; set; } = 33;
    public string Name { get; set; } = "Item_33_Kiara_Recipe_Memo";
    public string Icon { get; set; } = "Items/Icons/Memo.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "주문서\n주문이 적혀있는 메모이다\n[ Space Bar를 눌려 사용하기 ]";
}
