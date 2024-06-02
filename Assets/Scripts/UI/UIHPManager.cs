using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPManager : MonoBehaviour
{
    public Image barImg;
    float hp;
    public void UpdateBar()
    {
        hp = InfoManager.Instance.GameInfo.hp;
        float hpMax = InfoManager.Instance.GameInfo.GetMaxHP();
        barImg.fillAmount = hp / hpMax;
    }
}
