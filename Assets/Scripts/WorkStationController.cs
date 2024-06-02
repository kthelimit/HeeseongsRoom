using System.Collections;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class WorkStationController : MonoBehaviour
{
    public UIWorkManager uIWorkManager;
    public GameObject workeffect;
    public SpriteRenderer chair;
    public Sprite[] sprites;
    public Coroutine coroutine; //작업진행용 코루틴
    public MainController mainController;
    public void Init()
    {
        workeffect.SetActive(false);
        uIWorkManager.gameObject.SetActive(false);
        chair.sprite = sprites[0];
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnMouseDown()
    {
        if (coroutine != null)
        {
            Init();
            mainController.ActiveCharacter();
            mainController.ChangeState(CharacterController.State.None);
            StopCoroutine(coroutine);
        }
    }

    public IEnumerator Working()
    {
        uIWorkManager.UpdateBar();
        workeffect.SetActive(true);
        uIWorkManager.gameObject.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        chair.sprite = sprites[1];
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (InfoManager.Instance.GameInfo.hp <= 0)
            {
                mainController.ActiveCharacter();
                mainController.characterController.isReact = false;
                mainController.ChangeBText("으으, 피곤해...");
                mainController.ChangeState(CharacterController.State.None);
                Init();
                break;
            }
            InfoManager.Instance.GameInfo.progress += 1;
            InfoManager.Instance.GameInfo.hp -= 1;
            uIWorkManager.UpdateBar();
            mainController.UIHPProgress.UpdateHPBar();
            mainController.UIHPProgress.UpdateProgressBar();
            if (InfoManager.Instance.GameInfo.progress >= InfoManager.Instance.GameInfo.GetMakingMax())
            {
                InfoManager.Instance.GameInfo.progress = 0;
                //여기서 보상을 지급할것.              
                ItemData item = DataManager.Instance.GetMakeItemData(Random.Range(1, DataManager.Instance.GetCountMakeItem()));                
                GameManager.Instance.GetItemWithMessage(item);
                //캐릭터 애니메이션
                mainController.ActiveCharacter();
                mainController.characterController.isReact = false;
                mainController.ChangeState(CharacterController.State.None);
                mainController.characterController.animator.SetTrigger("isJoy");
                Init();
                break;
            }
        }
    }

}
