using UnityEngine;

public class Item15FaunaBranch : MonoBehaviour, IItem
{
    public int Index { get; set; } = 15;
    public string Name { get; set; } = "Item15_Fauna_branch";
    public string Icon { get; set; } = "Items/Icons/Fauna_branch.png";
    public bool isGetItem { get; set; } = false;
    public string Text { get; set; } = "대자연의 나뭇가지\n생기가 느껴지는 나뭇가지다.\n금방이라도 나무를 자라게 할 것 같다.";
}
