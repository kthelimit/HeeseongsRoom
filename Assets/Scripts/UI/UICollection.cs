using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICollection : MonoBehaviour
{
    public Transform contents;
    public GameObject slotPrefab;
    public List<CollectSlot> collectSlots;
    public UICollectionDesc collectionDesc;

    public void Init()
    {
        CollectSlot[] objs = contents.GetComponentsInChildren<CollectSlot>();
        foreach (CollectSlot slot in objs)
        {
            Destroy(slot.gameObject);
        }

        collectSlots = new List<CollectSlot>();
        foreach (ItemCollect a in InfoManager.Instance.GameInfo.collectedList)
        {
            GameObject g = Instantiate(slotPrefab, contents);
            g.GetComponent<CollectSlot>().SetItem(a);
            collectSlots.Add(g.GetComponent<CollectSlot>());
        }
        collectionDesc.Init();
    }

    public void UpdateSlot(int index, ItemCollect item)
    {
        collectSlots[index].SetItem(item);
    }

    public void ResetSelectedSlot()
    {
        foreach(CollectSlot slot in collectSlots)
        {
            slot.NotSelected();
        }
    }
}
