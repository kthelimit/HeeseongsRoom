using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : MonoBehaviour
{
    public Animator animator;
    public UIHPManager hpManager;
    public GameObject sleepEffect;
    public Coroutine coroutine;
   public MainController mainController;
    int recoveryHp=1;

    public void Init()
    {
        animator.SetBool("isSleep", false);
        hpManager.gameObject.SetActive(false);
        sleepEffect.SetActive(false);
        mainController.ActiveCharacter();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public IEnumerator Sleep()
    {
        animator.SetBool("isSleep", true);
        sleepEffect.SetActive(true);
        hpManager.gameObject.SetActive(true);
        hpManager.UpdateBar();
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        recoveryHp = InfoManager.Instance.GameInfo.GetMaxHP() / 100;
        while (true)
        {

            yield return new WaitForSeconds(1f);
            InfoManager.Instance.GameInfo.hp += recoveryHp;
            hpManager.UpdateBar();
            mainController.UIHPProgress.UpdateHPBar();
            if (InfoManager.Instance.GameInfo.hp >= InfoManager.Instance.GameInfo.GetMaxHP())
            {
                mainController.ActiveCharacter();
                mainController.ChangeState(CharacterController.State.None);
                Init();
                break;
            }
        }
    }
    //굳이 깨울 필요가 없는거 같아서 이건 생략함
    //private void OnMouseDown()
    //{
    //    wakeCall++;
    //    if(wakeCall>=5)
    //    {
    //        StopCoroutine(coroutine);            
    //        Init();
    //        mainController.ActiveCharacter();
    //        mainController.ChangeState(CharacterController.State.None);
    //        wakeCall = 0;
    //    }
    //}

}
