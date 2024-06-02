using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEditor.VersionControl;

public class DataManager
{
    public static readonly DataManager Instance = new DataManager();

    private Dictionary<int, ItemData> dicItemDatas;
    private Dictionary<int, ItemInfo> dicItemInfos;
    private Dictionary<int, ItemData> dicMakeItemDatas;
    private Dictionary<int, StatLevelData> dicStatLevelDatas;
    private List<ItemData> ListDropItemDatas;
    private List<ItemData> ListRandomItemDatas;
    private List<List<LineData>> ListLineData;
    private List<List<TalkData>> ListTalkData;

    private DataManager() { }



    public void LoadTalkData()
    {
        ListTalkData = new List<List<TalkData>>();
        TextAsset asset = Resources.Load<TextAsset>("talk_data");
        string json = asset.text;
        TalkData[] datas = JsonConvert.DeserializeObject<TalkData[]>(json);

        for (int i = 0; i < 7; i++)
        {
            List<TalkData> a = new List<TalkData>();
            ListTalkData.Add(a);
        }
        //데이터를 넣는 코드
        for (int i = 0; i < datas.Length; i++)
        {
             ListTalkData[datas[i].likeLevel].Add(datas[i]);
        }

    }

    public void LoadLineData()
    {
        ListLineData = new List<List<LineData>>();
        TextAsset asset = Resources.Load<TextAsset>("line_data");
        string json = asset.text;
        LineData[] datas = JsonConvert.DeserializeObject<LineData[]>(json);
        
        for (int i = 0; i < 6; i++)
        {
            List<LineData> a = new List<LineData>();
            ListLineData.Add(a);
        }
        for(int i = 0;i< datas.Length; i++)
        {
            ListLineData[datas[i].likeLevel].Add(datas[i]);
        }

    }
    public void LoadStatLevelData()
    {
        TextAsset asset = Resources.Load<TextAsset>("stat_data");
        string json = asset.text;
        StatLevelData[] datas = JsonConvert.DeserializeObject<StatLevelData[]>(json);
        dicStatLevelDatas = datas.ToDictionary(x => x.level);
    }

    public void LoadItemData()
    {
        TextAsset asset = Resources.Load<TextAsset>("item_data");
        string json = asset.text;
        ItemData[] datas = JsonConvert.DeserializeObject<ItemData[]>(json);
        dicItemDatas = datas.ToDictionary(x => x.id);
        dicMakeItemDatas = new Dictionary<int, ItemData>();
        ListDropItemDatas = new List<ItemData>();
        ListRandomItemDatas = new List<ItemData>();
        foreach (ItemData data in datas)
        {
            if (data.type == "drop")
            {
                ListDropItemDatas.Add(data);
            }
            else if (data.type == "product")
            {
                dicMakeItemDatas.Add(data.id, data);
            }
            else if(data.type=="random")
            {
                ListRandomItemDatas.Add(data);
            }
        }
    }

    public List<TalkData> GetTalkDataList(int likelevel)
    {
        return ListTalkData[likelevel];
    }

    public List<LineData> GetLineDataList(int likelevel)
    {
        return ListLineData[likelevel];
    }
    public TalkData GetRandomTalkData(int likelevel)
    {
        return ListTalkData[likelevel][Random.Range(0, ListTalkData[likelevel].Count)];
    }

    public LineData GetRandomLineData(int likelevel)
    {
        return ListLineData[likelevel][Random.Range(0, ListLineData[likelevel].Count)];
    }
    public StatLevelData GetStatLevelData(int id)
    {
        return dicStatLevelDatas[id];
    }

    public List<StatLevelData> GetStatLevelDataList()
    {
        return dicStatLevelDatas.Values.ToList();
    }

    public ItemData GetItemData(int id)
    {
        return dicItemDatas[id];
    }
    public int GetCountMakeItem()
    {
        return dicMakeItemDatas.Count;
    }
    public List<ItemData> GetDropItemList()
    {
        return ListDropItemDatas;
    }

    public List<ItemData> GetRandomItemList()
    {
        return ListRandomItemDatas;
    }

    public ItemData GetMakeItemData(int id)
    {
        return dicItemDatas[id];
    }

    public List<ItemData> GetItemDataList()
    {
        return dicItemDatas.Values.ToList();
    }


}
