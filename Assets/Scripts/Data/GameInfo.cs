using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo 
{
    //게임 데이터를 저장하는데 사용할 클래스
    public string playerName; //사용자 이름
    public float like; // 희성의 호감도
    public float hp; //희성의 체력
    public float money; //희성의 돈
    public float progress; //희성이 하고 있는 의뢰 진행도
    public float searchLeftTime; //희성이 하고 있는 탐색 남은 시간
    public int talkLeft; //희성과 대화시간
    public DateTime userIndate; //이전 접속시간
    public DateTime lastTalkTime; //지난 대화시간
    public List<ItemInfo> heeSungsItems; //희성이 소지한 아이템리스트
    public List<ItemCollect> collectedList;     //콜렉션
    public List<StatInfo> stats;  //희성의 스탯  

    //신규 유저일때 호출
    public void Init()
    {
        playerName = "";
        like = 0;
        hp = 100;
        money = 0;
        progress = 0;
        searchLeftTime = 0;
        userIndate = DateTime.Now;
        lastTalkTime = DateTime.Today;
        talkLeft = 5;
        this.heeSungsItems = new List<ItemInfo>();
        this.collectedList = new List<ItemCollect>();
        MakeCollectList();
        this.stats = new List<StatInfo>();
        string[] statnames = { "체력", "기술", "탐색","협상"};
        foreach (string statname in statnames)
        {
            StatInfo info = new StatInfo();
            info.name = statname;           
            stats.Add(info);
        }
    }
    void MakeCollectList()
    {

        List<ItemData> dataList = DataManager.Instance.GetItemDataList();
        foreach (ItemData data in dataList)
        {
            ItemCollect itemCollect = new ItemCollect();
            itemCollect.id = data.id;
            collectedList.Add(itemCollect);
        }

    }

    public int GetMaxHP()
    {        
        //체력 레벨
        return DataManager.Instance.GetStatLevelData(stats[0].level).maxHP;
    }

    public float GetSearchTime()
    {
        //탐색 레벨
        return DataManager.Instance.GetStatLevelData(stats[2].level).searchTime;
    }

    public float GetMakingMax()
    {
        //기술 레벨
        return DataManager.Instance.GetStatLevelData(stats[1].level).makingMax;
    }

    public float GetMoneyAdd()
    {
        //협상 레벨
        return DataManager.Instance.GetStatLevelData(stats[3].level).moneyAdd;
    }
}
