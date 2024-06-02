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
    //public string playerName; //사용자 이름
    //public float like; // 희성의 호감도
    //public float hp; //희성의 체력
    //public float money; //희성의 돈
    //public float progress; //희성이 하고 있는 의뢰 진행도
    //public List<ItemInfo> heeSungsItems; //희성이 소지한 아이템리스트
    //public List<ItemCollect> collectedList;     //당신이 소지한 아이템리스트
    //public List<MissionInfo> missions;  //희성이 하고 있는 의뢰리스트   

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
            //기존유저
            string json = File.ReadAllText(path);
            gameInfo = JsonConvert.DeserializeObject<GameInfo>(json);
            // 오류 난 세이브 파일 수정용....
            // 마이너스 혹은 0인 아이템을 인벤에서 제거한다.
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
            //스탯레벨이 6이면 5로 수정한다.
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
            //신규유저
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
            mainController.ChangeBText("어서 와.");
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
            mainController.ChangeTText("현재 탐색중이라 불가능하다.");
            return;
        }
        if (mainController.characterController.state != CharacterController.State.None) { return; }
        StopChaCouroutine();
        if (InfoManager.Instance.GameInfo.hp >= 80)
        {
            mainController.ChangeBText("지금은 졸리지 않아.");
            return;
        }
        mainController.ChangeBText("그러면 한숨 잘게. 잘 자.");
        mainController.ChangeState(CharacterController.State.Sleep);
        mainController.characterController.ChangeTargetPos(mainController.bedController.transform.position.x);
        StartMove();
    }

    public void GotoWork()
    {
        if (mainController.characterController.state == CharacterController.State.Search)
        {
            mainController.ChangeTText("현재 탐색중이라 불가능하다.");
            return;
        }
        if (mainController.characterController.state != CharacterController.State.None) { return; }
        StopChaCouroutine();
        if (InfoManager.Instance.GameInfo.hp <= 0)
        {
            mainController.ChangeBText("지금은 너무 피곤해...");
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
            string str = itemData.name + "(을)를 " + quantity + "개 획득했다!";
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
        mainController.ChangeBText("다녀왔어.");
    }

    public void GetItem(ItemData itemData, int quantity = 1)
    {

        CheckCollect(itemData);

        int index = InfoManager.Instance.GameInfo.heeSungsItems.FindIndex(x => x.id.Equals(itemData.id));
        if (index != -1)
        {
            //계산 후 결과가 0보다 크거나 같을때
            if (InfoManager.Instance.GameInfo.heeSungsItems[index].quantity + quantity >= 0)
            {
                InfoManager.Instance.GameInfo.heeSungsItems[index].quantity += quantity;

                //계산 후 결과가 0일때
                if (InfoManager.Instance.GameInfo.heeSungsItems[index].quantity <= 0)
                {
                    //제거
                    InfoManager.Instance.GameInfo.heeSungsItems.Remove(InfoManager.Instance.GameInfo.heeSungsItems[index]);
                }
            }
            else
            {
                //계산 후 결과가 0보다 작으면 오류 메시지를 출력한다.                
                Debug.Log("수량이 부족합니다");
            }
        }
        else
        {
            //해당 키가 들어있지 않으면 새로 추가한다.
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
            Debug.Log("돈이 없어...");
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
            Debug.Log("피곤해...");
        }
    }

    public void GetLike(int like)
    {
        InfoManager.Instance.GameInfo.like += like;
        mainController.uiLike.UpdateBar();
    }

    //얻은 아이템들은 콜렉션에 얻었다고 체크하기
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
            //기존에 탐색을 돌리고 있었다면
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
            mainController.ChangeBTextWithExit("그럼 다음에 봐.");
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
            //남은 대화횟수가 5보다 낮은데 지난 대화시간이 오늘이 아닌경우 5로 복구해준다.
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
