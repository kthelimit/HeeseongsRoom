using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatLevelManager : MonoBehaviour
{
    public Transform content;
    public GameObject statPrefab;
    List<StatLevel> statLevels;
    public GameManager gameManager;
    public void Init()
    {        
        if (statLevels == null)
        {
            statLevels = new List<StatLevel>();
        }
        else
        {
            foreach (StatLevel level in statLevels)
            {
                GameObject.Destroy(level.gameObject);
            }
        }

    }

    public void ShowStats()
    {
        Init();
        List<StatInfo> stats = InfoManager.Instance.GameInfo.stats;
        int b;
        for(int i=0; i<stats.Count; i++)
        {
            b = i;
            StatInfo info = stats[i];
            StatLevelData leveldata = DataManager.Instance.GetStatLevelData(info.level);
            GameObject a = Instantiate(statPrefab, content);
            a.GetComponent<StatLevel>().SetStat(stats[i]);
            statLevels.Add(a.GetComponent<StatLevel>());            
        }

    }
}
