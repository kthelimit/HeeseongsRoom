using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UIHPProgress : MonoBehaviour
{
    public RectTransform rect;
    public Image hPbar;
    public Image hPbarWhite;
    public Image progressBar;
    public Image progressBarWhite;
    public MainController mainController;
    float hp;
    float progress;

    public void UpdateHPBar()
    {
        if (hp != InfoManager.Instance.GameInfo.hp)
        {
            if (Mathf.Abs(InfoManager.Instance.GameInfo.hp - hp) >= 10)
            { StartCoroutine(HPChangingAnimation()); }
            else
            {
                float hpMax = InfoManager.Instance.GameInfo.GetMaxHP();
                hp = InfoManager.Instance.GameInfo.hp;
                hPbar.fillAmount = hp / hpMax;
                hPbarWhite.fillAmount= hp / hpMax;
            }
        }
    }

    public void UpdateProgressBar()
    {
        progressBar.fillAmount = InfoManager.Instance.GameInfo.progress / InfoManager.Instance.GameInfo.GetMakingMax();
    }     

    IEnumerator HPChangingAnimation()
    {
        float hpTemp = hp;
        float hpMax = InfoManager.Instance.GameInfo.GetMaxHP();
        hp = InfoManager.Instance.GameInfo.hp;
        float add = (hp - hpTemp) / 10;
        int addCount = 0;
        //색있는 바는 미리 깎아둔다.
        hPbar.fillAmount = hp / hpMax;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            hpTemp += add;
            hPbarWhite.fillAmount = hpTemp / hpMax;
            addCount++;
            if (addCount == 10)
            {
                hPbarWhite.fillAmount = hp / hpMax;
                break;
            }
        }
    }

    public void ShowUpdate()
    {
        if (gameObject.transform.GetChild(0).transform.position.y >= 330)
        {
            mainController.ActiveHPProgressBar();
        }
        StartCoroutine(ClosePanel());
    }

    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(3f);
        mainController.ActiveHPProgressBar();
    }
}
