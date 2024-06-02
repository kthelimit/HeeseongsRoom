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
        if (statInfo.name == "ü��")
        {
            desc.text = "���� �� �� �ִ� HP�� ����Ѵ�.";
            money.text = "";
        }
        else if (statInfo.name == "���")
        {
            desc.text = "���۹�ǰ�� �� �� ������ ���� �� �ְ� �ȴ�.";
            money.text = "";
        }
        else if (statInfo.name == "Ž��")
        {
            desc.text = "Ž���� �ɸ��� �ð��� �پ���";
            money.text = "";
        }
        else if (statInfo.name == "����")
        {
            desc.text = "�������� ���� �� ��ΰ� �� �� �ְ� �ȴ�.";
            money.text = "";
        }
    }

}
