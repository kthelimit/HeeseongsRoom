using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Myevan;

public class MessageController : MonoBehaviour
{
    public TMP_Text displayText;
    public string text;
    public Coroutine coroutine;

    public virtual void ChangeText(string text)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        displayText.text = "";
        this.text = text;
        coroutine = StartCoroutine(PrintText());
    }

    public virtual IEnumerator PrintText()
    {
        int count = 0;
        while (count != text.Length)
        {
            if (count < text.Length)
            {
                displayText.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(0.1f);
        }        
      
    }

    public virtual void ChangeTextWithExit(string text)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        displayText.text = "";
        this.text = text;
        coroutine = StartCoroutine(PrintTextWithExit());
    }

    public virtual IEnumerator PrintTextWithExit()
    {
        int count = 0;
        while (count != text.Length)
        {
            if (count < text.Length)
            {
                displayText.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(0.1f);
        }
        Application.Quit();
    }

}
