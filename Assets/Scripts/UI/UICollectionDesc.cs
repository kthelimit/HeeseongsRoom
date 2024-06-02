using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICollectionDesc : MonoBehaviour
{
    public Image itemImg;
    public TMP_Text text;
    public TMP_Text money;
    public int currentItem=0;
    public UICollection UICollection;

    public void Init()
    {
        itemImg.sprite = Resources.Load<Sprite>("itemSprite/item_000");
        text.text = "?";
        money.text = "?";
    }
    public void SetItem(ItemCollect item)
    {
        if (!item.isCollect)
        {
            return;
        }
        ItemData itemData = DataManager.Instance.GetItemData(item.id);
        itemImg.sprite = Resources.Load<Sprite>("itemSprite/" + itemData.icon);
        text.text = itemData.name;
        money.text = itemData.money.ToString();
        UICollection.ResetSelectedSlot();
        currentItem = item.id;
    }

}
