using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            GetComponent<CanvasGroup>().alpha -= 0.2f;
            yield return new WaitForSeconds(0.1f);
            if (GetComponent<CanvasGroup>().alpha <= 0)
            {
                break;
            }
        }
    }
}
