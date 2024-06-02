using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShowNotHere : MonoBehaviour
{
    public TMP_Text text;
    Coroutine coroutine;
    private void OnEnable()
    {
        coroutine= StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        int count = text.text.Length;
        string strOrigin = "희성이는 지금 부재중이다";
        text.text = strOrigin;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(count> text.text.Length)
            {
                text.text += ".";
            }
            else
            {
                text.text = strOrigin;
            }            
        }
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }
}
