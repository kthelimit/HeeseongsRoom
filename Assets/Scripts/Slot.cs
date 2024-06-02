using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image itemImg;
    public Image SelectImg;

    public virtual void SetItem(ItemInfo item)
    {
        ItemData itemData = DataManager.Instance.GetItemData(item.id);
        itemImg.sprite = Resources.Load<Sprite>("itemSprite/" + itemData.icon);
        itemImg.color = Color.white;
    }

}
