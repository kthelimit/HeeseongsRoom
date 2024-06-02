using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo 
{
    //���� �����͸� �����ϴµ� ����� Ŭ����
    public string playerName; //����� �̸�
    public float like; // ���� ȣ����
    public float hp; //���� ü��
    public float money; //���� ��
    public float progress; //���� �ϰ� �ִ� �Ƿ� ���൵
    public float searchLeftTime; //���� �ϰ� �ִ� Ž�� ���� �ð�
    public int talkLeft; //�񼺰� ��ȭ�ð�
    public DateTime userIndate; //���� ���ӽð�
    public DateTime lastTalkTime; //���� ��ȭ�ð�
    public List<ItemInfo> heeSungsItems; //���� ������ �����۸���Ʈ
    public List<ItemCollect> collectedList;     //�ݷ���
    public List<StatInfo> stats;  //���� ����  

    //�ű� �����϶� ȣ��
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
        string[] statnames = { "ü��", "���", "Ž��","����"};
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
        //ü�� ����
        return DataManager.Instance.GetStatLevelData(stats[0].level).maxHP;
    }

    public float GetSearchTime()
    {
        //Ž�� ����
        return DataManager.Instance.GetStatLevelData(stats[2].level).searchTime;
    }

    public float GetMakingMax()
    {
        //��� ����
        return DataManager.Instance.GetStatLevelData(stats[1].level).makingMax;
    }

    public float GetMoneyAdd()
    {
        //���� ����
        return DataManager.Instance.GetStatLevelData(stats[3].level).moneyAdd;
    }
}
