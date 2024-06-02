using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : MonoBehaviour
{
    public Transform content;
    public GameObject slotPrefab;

    public void Init()
    {
        InventorySlot[] inventorySlots= content.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in inventorySlots)
        {
            GameObject.Destroy(slot.gameObject);
        }
    }    

    public void SetResult(List<ItemInfo> list)
    {
        foreach (ItemInfo item in list)
        {
            GameObject a= Instantiate(slotPrefab, content);
            a.GetComponent<InventorySlot>().SetItem(item);
            a.GetComponent<Button>().interactable = false;
        }
        StartCoroutine(CloseThis());
    }

    IEnumerator CloseThis()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        yield return new WaitForSeconds(5f);
        mainController.CloseResult();
    }
}
