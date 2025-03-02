using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item16KroniiSword : MonoBehaviour, IItem
{
        public int Index { get; set; } = 16;
        public string Name { get; set; } = "Item_16_KroniiSword";
        public string Icon { get; set; } = "Items/Icons/KroniiSword.png";
        public bool isGetItem { get; set; } = false;
        public string Text { get; set; } = "시간의 검\n시계바늘 형태의 검. 시간을 되돌릴 수 있을 것 같다.";
     
}
