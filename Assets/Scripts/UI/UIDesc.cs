using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDesc : MonoBehaviour
{
    public TMP_Text desc;
    public TMP_Text money;
    public Image goldImg;
    public void SetItem(ItemData item)
    {
        desc.text = item.description;
        money.text = ((int)Mathf.Ceil((item.money) * InfoManager.Instance.GameInfo.GetMoneyAdd())).ToString();
        goldImg.color = Color.white;
    }

    public void SetStat(StatInfo statInfo)
    {
        goldImg.color = new Color(0,0,0,0);
        if (statInfo.name == "체력")
        {
            desc.text = "레벨 업 시 최대 HP가 상승한다.";
            money.text = "";
        }
        else if (statInfo.name == "기술")
        {
            desc.text = "제작물품을 좀 더 빠르게 만들 수 있게 된다.";
            money.text = "";
        }
        else if (statInfo.name == "탐색")
        {
            desc.text = "탐색에 걸리는 시간이 줄어든다";
            money.text = "";
        }
        else if (statInfo.name == "협상")
        {
            desc.text = "아이템을 조금 더 비싸게 팔 수 있게 된다.";
            money.text = "";
        }
    }

}
