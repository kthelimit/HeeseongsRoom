using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CollectSlot : Slot
{
    public ItemCollect itemCollect;
    ScrollRect myScrollRect;
    public void SetItem(ItemCollect item)
    {
        if(myScrollRect==null)
        {
            MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
            myScrollRect = mainController.UICollection.GetComponent<ScrollRect>();
        }
        ItemData itemData = DataManager.Instance.GetItemData(item.id);
        itemCollect = item;
        itemImg.sprite = Resources.Load<Sprite>("itemSprite/" + itemData.icon);
        if (item.isCollect)
        {
            itemImg.color = Color.white;
        }
        else
        {
            itemImg.color = Color.black;
        }
    }

    public void Clicked()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        if (SelectImg.gameObject.activeInHierarchy)
        {
            NotSelected();
            mainController.UICollectionDesc.Init();
            mainController.UICollectionDesc.currentItem = 0;
            return;
        }
        mainController.UICollectionDesc.SetItem(itemCollect);
        if (mainController.UICollectionDesc.currentItem == itemCollect.id)
        {
            SelectImg.gameObject.SetActive(true);
        }
    }

    public void NotSelected()
    {
        SelectImg.gameObject.SetActive(false);
    }

    public void OnMouseEnter()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        mainController.ActiveDesc(transform);
        mainController.UIDesc.SetItem(DataManager.Instance.GetItemData(this.itemCollect.id));
    }
    public void OnMouseExit()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        mainController.CloseDesc();
    }
    //아래의 코드는 다음 링크에서 가져왔다. https://trialdeveloper.tistory.com/42 
    public void OnBeginDurabilitySliderDrag(BaseEventData eventData)
    {
        if (myScrollRect == null)
            return;

        myScrollRect.OnBeginDrag(eventData as PointerEventData);
    }

    public void OnDurabilitySliderDrag(BaseEventData eventData)
    {
        if (myScrollRect == null)
            return;

        myScrollRect.OnDrag(eventData as PointerEventData);
    }

    public void OnEndDurabilitySliderDrag(BaseEventData eventData)
    {
        if (myScrollRect == null)
            return;

        myScrollRect.OnEndDrag(eventData as PointerEventData);
    }    
}
