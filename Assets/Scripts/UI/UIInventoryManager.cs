using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryManager : MonoBehaviour
{
    public Transform contents;
    public GameObject slotPrefab;
    public TMP_Text text;
    public int currentItem=0;
    public Button useBtn;
    public Button sellBtn;
    List<InventorySlot> slots;
    public GameManager gameManager;

    public void Init()
    {
        InventorySlot[] inventorySlots = contents.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in inventorySlots)
        {
            GameObject.Destroy(slot.gameObject);
        }
        useBtn.interactable = false;
        useBtn.onClick.RemoveAllListeners();
        sellBtn.interactable = false;
        sellBtn.onClick.RemoveAllListeners();
    }
    public void ShowInventory()
    {
        Init();
        slots=new List<InventorySlot>();
        List<ItemInfo> items = InfoManager.Instance.GameInfo.heeSungsItems;
        if (items.Count != 0)
        {
            text.text = "";
        }
        else
        {
            text.text = "현재 비어있습니다.";
        }
        foreach (ItemInfo item in items)
        {
            GameObject a = Instantiate(slotPrefab, contents);
            a.GetComponent<InventorySlot>().SetItem(item);
            slots.Add(a.GetComponent<InventorySlot>());
        }
    }

    public void SetItem(ItemInfo itemInfo)
    {
        ItemData itemData = DataManager.Instance.GetItemData(itemInfo.id);
        ResetSelectedSlot();
        currentItem = itemInfo.id;
        if (itemData.id==100)
        {
            List<ItemData> randomList = DataManager.Instance.GetRandomItemList();
            useBtn.interactable = true;
            useBtn.onClick.AddListener(() => {
                gameManager.GetItem(itemData, -1);
                ItemData a = randomList[Random.Range(0, randomList.Count)];
                gameManager.GetItemWithMessage(a);
                ShowInventory();
            });
        }
        else
        {
            useBtn.interactable = false;
        }
        sellBtn.interactable=true;
        sellBtn.onClick.RemoveAllListeners();
        sellBtn.onClick.AddListener(() => {
            Debug.Log("판매~");
            //아이템 판매한 돈 넣기
            gameManager.GetMoney((int)(Mathf.Ceil(itemData.money)*InfoManager.Instance.GameInfo.GetMoneyAdd()));
            //아이템 줄이기
            gameManager.GetItem(itemData, -1);
            ShowInventory();
        });
    }

    public void UpdateSlot(int index, ItemInfo item)
    {
        slots[index].SetItem(item);
    }

    public void ResetSelectedSlot()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.NotSelected();
        }
    }
}
