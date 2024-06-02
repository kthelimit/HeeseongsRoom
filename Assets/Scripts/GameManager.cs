using Myevan;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //GameInfo gameData;
    //public string playerName; //����� �̸�
    //public float like; // ���� ȣ����
    //public float hp; //���� ü��
    //public float money; //���� ��
    //public float progress; //���� �ϰ� �ִ� �Ƿ� ���൵
    //public List<ItemInfo> heeSungsItems; //���� ������ �����۸���Ʈ
    //public List<ItemCollect> collectedList;     //����� ������ �����۸���Ʈ
    //public List<MissionInfo> missions;  //���� �ϰ� �ִ� �Ƿڸ���Ʈ   

    public MainController mainController;

    private void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        DataManager.Instance.LoadStatLevelData();
        DataManager.Instance.LoadLineData();
        DataManager.Instance.LoadTalkData();
        DataManager.Instance.LoadItemData();
        Init();
        LoadData();
        mainController.UICollection.Init();
        mainController.inventoryManager.Init();
    }

    private void Init()
    {
        GameInfo gameInfo = null;
        string path = Path.Combine(UnityEngine.Application.persistentDataPath, "game_info.json");
        if (File.Exists(path))
        {
            //��������
            string json = File.ReadAllText(path);
            gameInfo = JsonConvert.DeserializeObject<GameInfo>(json);
            // ���� �� ���̺� ���� ������....
            // ���̳ʽ� Ȥ�� 0�� �������� �κ����� �����Ѵ�.
            while (true)
            {
                int count = 0;
                for (int i = 0; i < gameInfo.heeSungsItems.Count; i++)
                {
                    if (gameInfo.heeSungsItems[i].quantity <= 0)
                    {
                        gameInfo.heeSungsItems.Remove(gameInfo.heeSungsItems[i]);
                        count++;
                        break;
                    }
                }
                if(count == 0)
                    break;
            }
            //���ȷ����� 6�̸� 5�� �����Ѵ�.
            for(int i = 0; i<gameInfo.stats.Count; i++)
            {
                if (gameInfo.stats[i].level==6)
                {
                    gameInfo.stats[i].level = 5;
                }
            }

            InfoManager.Instance.GameInfo = gameInfo;
        }
        else
        {
            //�ű�����
            gameInfo = new GameInfo();
            gameInfo.Init();
            InfoManager.Instance.GameInfo = gameInfo;
            InfoManager.Instance.SaveLocal();


        }
    }


    void LoadData()
    {
        //gameData = InfoManager.Instance.GameInfo;
        //playerName = gameData.playerName;
        //like = gameData.like;
        //hp = gameData.hp;
        //money = gameData.money;
        //progress = gameData.progress;
        //heeSungsItems = gameData.heeSungsItems;
        //collectedList = gameData.collectedList;
        //missions = gameData.missions;        
        mainController.UpdateAllBar();
        mainController.workStationController.Init();
        mainController.bedController.Init();

        if (InfoManager.Instance.GameInfo.searchLeftTime != 0)
        {
            Search();
        }
        else
        {
            mainController.ChangeBText("� ��.");
        }
    }
    //public void SaveData()
    //{
    //    gameData.playerName = playerName;
    //    gameData.like = like;
    //    gameData.hp = hp;
    //    gameData.money = money;
    //    gameData.progress = progress;
    //    gameData.heeSungsItems = heeSungsItems;
    //    gameData.collectedList = collectedList;
    //    gameData.missions = missions;
    //    InfoManager.Instance.GameInfo = gameData;
    //    mainController.UpdateAllBar();
    //}

    public void StopChaCouroutine()
    {
        if (mainController.characterController.coroutine != null)
        {
            StopCoroutine(mainController.characterController.coroutine);
            mainController.characterController.state = CharacterController.State.None;
            mainController.characterController.animator.SetBool("isMove", false);
        }
    }

    public void StartWork()
    {
        mainController.workStationController.coroutine = StartCoroutine(mainController.workStationController.Working());
    }

    public void StartSleep()
    {

        mainController.bedController.coroutine = StartCoroutine(mainController.bedController.Sleep());
    }
    public void StartMove()
    {
        mainController.characterController.coroutine = StartCoroutine(mainController.characterController.Move());
    }

    public void GotoSleep()
    {
        if (mainController.characterController.state == CharacterController.State.Search)
        {
            mainController.ChangeTText("���� Ž�����̶� �Ұ����ϴ�.");
            return;
        }
        if (mainController.characterController.state != CharacterController.State.None) { return; }
        StopChaCouroutine();
        if (InfoManager.Instance.GameInfo.hp >= 80)
        {
            mainController.ChangeBText("������ ������ �ʾ�.");
            return;
        }
        mainController.ChangeBText("�׷��� �Ѽ� �߰�. �� ��.");
        mainController.ChangeState(CharacterController.State.Sleep);
        mainController.characterController.ChangeTargetPos(mainController.bedController.transform.position.x);
        StartMove();
    }

    public void GotoWork()
    {
        if (mainController.characterController.state == CharacterController.State.Search)
        {
            mainController.ChangeTText("���� Ž�����̶� �Ұ����ϴ�.");
            return;
        }
        if (mainController.characterController.state != CharacterController.State.None) { return; }
        StopChaCouroutine();
        if (InfoManager.Instance.GameInfo.hp <= 0)
        {
            mainController.ChangeBText("������ �ʹ� �ǰ���...");
            return;
        }
        mainController.ChangeState(CharacterController.State.Work);
        mainController.characterController.ChangeTargetPos(mainController.workStationController.chair.transform.position.x);
        StartMove();
    }

    public void GetItemWithMessage(ItemData itemData, int quantity = 1)
    {
        if (quantity > 0)
        {
            string str = itemData.name + "(��)�� " + quantity + "�� ȹ���ߴ�!";
            mainController.ChangeTText(Korean.ReplaceJosa(str));
        }
        GetItem(itemData, quantity);

    }

    public void GetSeveralItem(int count)
    {
        List<ItemData> list = DataManager.Instance.GetDropItemList();
        List<ItemInfo> items = new List<ItemInfo>();
        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            GetItem(list[index]);
            int index2 = items.FindIndex(x => x.id.Equals(list[index].id));
            if (index2 != -1)
            {
                items[index2].quantity += 1;
            }
            else
            {
                ItemInfo itemInfo = new ItemInfo();
                itemInfo.id = list[index].id;
                itemInfo.quantity = 1;
                items.Add(itemInfo);
            }
        }
        mainController.DeActiveMenuButton();
        mainController.ActiveNotHere();
        mainController.ActiveResult();
        mainController.UIResult.SetResult(items);
        mainController.ActiveCharacter();
        mainController.ChangeBText("�ٳ�Ծ�.");
    }

    public void GetItem(ItemData itemData, int quantity = 1)
    {

        CheckCollect(itemData);

        int index = InfoManager.Instance.GameInfo.heeSungsItems.FindIndex(x => x.id.Equals(itemData.id));
        if (index != -1)
        {
            //��� �� ����� 0���� ũ�ų� ������
            if (InfoManager.Instance.GameInfo.heeSungsItems[index].quantity + quantity >= 0)
            {
                InfoManager.Instance.GameInfo.heeSungsItems[index].quantity += quantity;

                //��� �� ����� 0�϶�
                if (InfoManager.Instance.GameInfo.heeSungsItems[index].quantity <= 0)
                {
                    //����
                    InfoManager.Instance.GameInfo.heeSungsItems.Remove(InfoManager.Instance.GameInfo.heeSungsItems[index]);
                }
            }
            else
            {
                //��� �� ����� 0���� ������ ���� �޽����� ����Ѵ�.                
                Debug.Log("������ �����մϴ�");
            }
        }
        else
        {
            //�ش� Ű�� ������� ������ ���� �߰��Ѵ�.
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.id = itemData.id;
            itemInfo.quantity = quantity;
            InfoManager.Instance.GameInfo.heeSungsItems.Add(itemInfo);

        }

    }

    public void GetMoney(int amount)
    {
        if (InfoManager.Instance.GameInfo.money + amount >= 0)
        {
            InfoManager.Instance.GameInfo.money += amount;
            mainController.UIMoneyManager.UpdateMoneyText();
        }
        else
        {
            Debug.Log("���� ����...");
        }

    }
    public void GetHp(float amount)
    {
        if (InfoManager.Instance.GameInfo.hp + amount >= 0)
        {
            InfoManager.Instance.GameInfo.hp += amount;
            mainController.UIHPProgress.UpdateHPBar();
        }
        else
        {
            Debug.Log("�ǰ���...");
        }
    }

    public void GetLike(int like)
    {
        InfoManager.Instance.GameInfo.like += like;
        mainController.uiLike.UpdateBar();
    }

    //���� �����۵��� �ݷ��ǿ� ����ٰ� üũ�ϱ�
    private void CheckCollect(ItemData itemData)
    {
        int index = InfoManager.Instance.GameInfo.collectedList.FindIndex(x => x.id.Equals(itemData.id));
        InfoManager.Instance.GameInfo.collectedList[index].isCollect = true;
        mainController.UICollection.UpdateSlot(index, InfoManager.Instance.GameInfo.collectedList[index]);
    }

    public void Search()
    {
        if (mainController.characterController.state != CharacterController.State.None) { return; }
        if (InfoManager.Instance.GameInfo.searchLeftTime != 0)
        {
            //������ Ž���� ������ �־��ٸ�
            mainController.CloseStatLv();
            mainController.characterController.state = CharacterController.State.Search;
            mainController.ActiveSearch();
            mainController.characterController.gameObject.SetActive(false);
            mainController.searchManager.StartTimer(InfoManager.Instance.GameInfo.searchLeftTime);
            mainController.ActiveNotHere();
        }
        else
        {
            if (InfoManager.Instance.GameInfo.hp - 30 >= 0)
            {
                GetHp(-30);
                mainController.UIHPProgress.ShowUpdate();
                mainController.CloseStatLv();
                mainController.characterController.state = CharacterController.State.Search;
                mainController.ActiveSearch();
                mainController.characterController.gameObject.SetActive(false);
                mainController.searchManager.StartTimer(InfoManager.Instance.GameInfo.GetSearchTime());
                mainController.ActiveNotHere();
            }
        }
    }


    public void ExitGame()
    {
        if (mainController.characterController.state != CharacterController.State.Search)
        {
            mainController.ChangeBTextWithExit("�׷� ������ ��.");
            InfoManager.Instance.SaveLocal();
        }
        else
        {
            InfoManager.Instance.SaveLocal();
            Application.Quit();

        }
    }
    void OnApplicationQuit()
    {
        InfoManager.Instance.SaveLocal();
    }

    public void Talk()
    {
        if ((InfoManager.Instance.GameInfo.talkLeft != 5) && (InfoManager.Instance.GameInfo.lastTalkTime != DateTime.Today))
        {
            //���� ��ȭȽ���� 5���� ������ ���� ��ȭ�ð��� ������ �ƴѰ�� 5�� �������ش�.
            InfoManager.Instance.GameInfo.talkLeft = 5;
        }
        if (InfoManager.Instance.GameInfo.talkLeft >= 0)
        {
            InfoManager.Instance.GameInfo.talkLeft--;
            InfoManager.Instance.GameInfo.lastTalkTime = DateTime.Today;
            mainController.characterController.CheckLike();
            TalkData talkData = DataManager.Instance.GetRandomTalkData(mainController.characterController.likeLevel);
            mainController.ChangeMText(talkData.line);
            GetLike(talkData.like);
        }
        else
        {
            TalkData talkData = DataManager.Instance.GetRandomTalkData(6);
            mainController.ChangeMText(talkData.line);
            GetLike(talkData.like);
        }
    }

}
