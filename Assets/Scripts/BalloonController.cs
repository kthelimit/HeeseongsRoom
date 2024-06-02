using System.Collections;
using TMPro;
using UnityEngine;

public class BalloonController : MessageController
{   

    public override IEnumerator PrintText()
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
        yield return new WaitForSeconds(2f);
        CloseBalloon();
    }

    void CloseBalloon()
    {
        this.gameObject.SetActive(false);
    }
}
