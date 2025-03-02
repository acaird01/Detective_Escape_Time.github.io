using UnityEngine;

public class Calli_ItemTorch : MonoBehaviour
{
    Item25Torch torch;

    private void Start()
    {
        torch = gameObject.GetComponent<Item25Torch>();
        ItemSetting();
    }

    void ItemSetting()
    {
        if (torch.isGetItem)
        {
            gameObject.SetActive(false);
        }
        else
        {
        }
    }
}
