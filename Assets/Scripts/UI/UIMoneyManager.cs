using System.Collections;
using TMPro;
using UnityEngine;


public class UIMoneyManager : UIBase
{
    public TMP_Text moneyText;
    float money;
    float prevmoney;
    Coroutine coroutine1;
    Coroutine coroutine2;

    public void UpdateMoneyTextWithoutAnimation()
    {
        moneyText.text = InfoManager.Instance.GameInfo.money.ToString();
    }

    public void UpdateMoneyText()
    {
        if (money != InfoManager.Instance.GameInfo.money){
            if (coroutine2 != null)
            {
                StopCoroutine(coroutine2);
                moneyText.text = prevmoney.ToString();
            }
            GetComponent<CanvasGroup>().alpha = 1f;
            coroutine2 = StartCoroutine(ChangingAnimation());
        }
    }
    IEnumerator ChangingAnimation()
    {
        float moneytemp = money;
        float speed = Mathf.Abs(8 / (InfoManager.Instance.GameInfo.money - money));
        if (speed > 0.1f)
        {
            speed = 0.1f;
        }
        money = InfoManager.Instance.GameInfo.money;
        prevmoney = money;
        while (true)
        {       
            yield return new WaitForSeconds(speed);
            if (moneytemp < money)
            {
                moneytemp += 1;
            }
            else if (moneytemp == money)
            {
                moneyText.text = money.ToString();               
                break;
            }
            else
            {
                moneytemp -= 1;
            }
            moneyText.text = moneytemp.ToString();
        }
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        yield return new WaitForSeconds(1f);
        coroutine1 = StartCoroutine(FadeOut());
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
