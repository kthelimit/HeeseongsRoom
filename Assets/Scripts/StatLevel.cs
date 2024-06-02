using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatLevel : MonoBehaviour
{
    public TMP_Text statName;
    public TMP_Text level;
    public TMP_Text money;
    public StatLevelData levelData;
    StatInfo statInfo;

    public void Init()
    {
        this.GetComponent<Button>().onClick.RemoveAllListeners();
        this.GetComponent<Button>().interactable = false;
    }
    public void SetStat(StatInfo info)
    {
        Init();
        statInfo = info;
        levelData = DataManager.Instance.GetStatLevelData(info.level);
        statName.text = info.name;
        level.text = levelData.levelScript;
        money.text = levelData.money.ToString();
        if (InfoManager.Instance.GameInfo.money - levelData.money >= 0)
        {
            this.GetComponent<Button>().interactable = true;
        }
        if (levelData.level==5)
        {
            money.text = "MAX";
            this.GetComponent<Button>().interactable = false;
        }
    }   

    public void Clicked()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();

            mainController.gameManager.GetMoney(-levelData.money);
            int index = InfoManager.Instance.GameInfo.stats.FindIndex(x => x.name.Equals(statInfo.name));
            InfoManager.Instance.GameInfo.stats[index].level++;
            statInfo= InfoManager.Instance.GameInfo.stats[index];
            SetStat(statInfo);
     
    }

    public void OnMouseEnter()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        mainController.ActiveDesc(transform);
        mainController.UIDesc.SetStat(statInfo);
    }
    public void OnMouseExit()
    {
        MainController mainController = GameObject.FindWithTag("GameController").GetComponent<MainController>();
        mainController.CloseDesc();
    }


}
