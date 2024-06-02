using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorkManager : MonoBehaviour
{
    public Image barImg;
    float progress;
    public void UpdateBar()
    {
        progress = InfoManager.Instance.GameInfo.progress;
        barImg.fillAmount = InfoManager.Instance.GameInfo.progress / InfoManager.Instance.GameInfo.GetMakingMax();
    }
}
