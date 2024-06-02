using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILikeManager : UIBase
{
    public Image barImg;
    float like;
    public void UpdateBarWithoutAnimation()
    {
        barImg.fillAmount = InfoManager.Instance.GameInfo.like / 500f;
    }
    public void UpdateBar()
    {
        if (Mathf.Abs(like - InfoManager.Instance.GameInfo.like)/500f>0.025f)
        {
            GetComponent<CanvasGroup>().alpha = 1f;
            StartCoroutine(ChangingAnimation());
        }
    }

    IEnumerator ChangingAnimation()
    {
        float liketemp = like;
        float add = (InfoManager.Instance.GameInfo.like - like) / 10;
        int addCount = 0;
        like = InfoManager.Instance.GameInfo.like;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            liketemp += add;
            addCount++;
            barImg.fillAmount = liketemp / 500f;
            if (addCount==10)
            {
                barImg.fillAmount = like / 500f;
                break;
            }
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOut());

    }

    public void OnMouseEnter()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void OnMouseExit()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
}
