using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Slot
{
    public Image numBack;
    public TMP_Text num;
    public ItemInfo ItemInfo;
    ScrollRect myScrollRect;
    public override void SetItem(ItemInfo item)
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        myScrollRect = mainController.inventoryManager.GetComponent<ScrollRect>();
        base.SetItem(item);
        ItemInfo = item;
        if (item.quantity > 1)
        {
            num.gameObject.SetActive(true);
            numBack.gameObject.SetActive(true);
            num.text = item.quantity.ToString();
        }
    }
    public void Clicked()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        if (SelectImg.gameObject.activeInHierarchy)
        {
            NotSelected();
            mainController.inventoryManager.ResetSelectedSlot();
            mainController.inventoryManager.currentItem = 0;
            return;
        }
        mainController.inventoryManager.SetItem(ItemInfo);
        if (mainController.inventoryManager.currentItem == ItemInfo.id)
        {
            SelectImg.gameObject.SetActive(true);
        }
    }
    public void OnMouseEnter()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        mainController.ActiveDesc(transform);
        mainController.UIDesc.SetItem(DataManager.Instance.GetItemData(this.ItemInfo.id));
    }
    public void OnMouseExit()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        mainController.CloseDesc();
    }
    public void NotSelected()
    {
        SelectImg.gameObject.SetActive(false);
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
