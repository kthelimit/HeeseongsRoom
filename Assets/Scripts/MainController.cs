using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class MainController : MonoBehaviour
{
    public GameManager gameManager;
    public UILikeManager uiLike;
    public CharacterController characterController;
    public WorkStationController workStationController;
    public BedController bedController;
    public UIMoneyManager UIMoneyManager;
    public BalloonController balloonController;
    public UIMessageManager messageManager;
    public UIButtonManager buttonManager;
    public UITopMessageManager topMessageManager;
    public UICollection UICollection;
    public UICollectionDesc UICollectionDesc;
    public UISearchManager searchManager;
    public UIResult UIResult;
    public Button menuButton;
    public Button showButton;
    public UIInventoryManager inventoryManager;
    public UIShowNotHere uIShowNotHere;
    public UIStatLevelManager statLevelManager;
    public UIHPProgress UIHPProgress;
    public UIDesc UIDesc;
    public float desc_PosX;
    public float desc_PosY;
    public float desc_Width;
    public float desc_Height;

    public void ActiveDesc(Transform tr)
    {
        UIDesc.GetComponent<CanvasGroup>().alpha = 1f;
        if (tr.position.x > desc_Width)
        {
            desc_PosX = -desc_PosX;
        }
        //if(tr.position.y> desc_Height)
        //{
        //    desc_PosY = -desc_PosY;
        //}
        UIDesc.transform.position = tr.position + new Vector3(desc_PosX, desc_PosY, 0);
        if(tr.position.x>200)
        {
            UIDesc.transform.position = tr.position + new Vector3(-70, 50, 0);
        }

    }
    public void CloseDesc()
    {
        UIDesc.GetComponent<CanvasGroup>().alpha = 0;
    }
    public void ActiveTalk()
    {
        if (characterController.state == CharacterController.State.Search)
        {
            ChangeTText("현재 부재중이라 대화가 불가능하다.");
            return;
        }
        messageManager.gameObject.SetActive(true);
        ChangeMText("안녕. 오늘은 무슨 일이야?");
        DeActiveMenuButton();
        CloseButtons();

    }
    public void CloseTalk()
    {
        messageManager.gameObject.SetActive(false);
        ActiveMenuButton();
    }
    public void ActiveSearch()
    {
        CloseButtons();
        searchManager.gameObject.SetActive(true);
        searchManager.Init();
    }

    public void ActiveMenuButton()
    {
        menuButton.gameObject.SetActive(true);
        showButton.gameObject.SetActive(true);
    }
    public void DeActiveMenuButton()
    {
        menuButton.gameObject.SetActive(false);
        showButton.gameObject.SetActive(false);
    }

    public void ActiveResult()
    {
        UIResult.gameObject.SetActive(true);
        UIResult.Init();
    }
    public void CloseResult()
    {
        UIResult.gameObject.SetActive(false);
        ActiveMenuButton();
        characterController.state = CharacterController.State.None;
        CloseSearch();
    }
    public void CloseSearch()
    {
        searchManager.gameObject.SetActive(false);
    }
    public void ActiveCharacter()
    {
        characterController.gameObject.SetActive(true);
        balloonController.gameObject.SetActive(false);
    }

    public void ActiveCollection()
    {
        if (UICollection.GetComponent<CanvasGroup>().alpha == 0)
        {
            UICollection.GetComponent<CanvasGroup>().alpha = 1;
            UICollection.GetComponent<CanvasGroup>().interactable = true;
            UICollection.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            CloseCollection();
        }
    }
    public void CloseCollection()
    {
        UICollection.GetComponent<CanvasGroup>().alpha = 0;
        UICollection.GetComponent<CanvasGroup>().interactable = false;
        UICollection.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UICollection.ResetSelectedSlot();
        UICollectionDesc.currentItem = 0;
        UICollectionDesc.Init();
    }

    public void ActiveInventory()
    {
        if (inventoryManager.GetComponent<CanvasGroup>().alpha == 0)
        {
            ActiveButtons();
            inventoryManager.GetComponent<CanvasGroup>().alpha = 1;
            inventoryManager.GetComponent<CanvasGroup>().interactable = true;
            inventoryManager.GetComponent<CanvasGroup>().blocksRaycasts = true;
            inventoryManager.ShowInventory();
        }
        else
        {
            CloseInventory();
        }
    }
    public void CloseInventory()
    {
        inventoryManager.GetComponent<CanvasGroup>().alpha = 0;
        inventoryManager.GetComponent<CanvasGroup>().interactable = false;
        inventoryManager.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //inventoryManager.ResetSelectedSlot();
        //UICollectionDesc.currentItem = 0;
        //UICollectionDesc.Init();
    }



    public void ChangeTText(string text)
    {
        topMessageManager.gameObject.SetActive(true);
        topMessageManager.ChangeText(text);
    }
    public void ChangeBTextWithExit(string text)
    {
        balloonController.gameObject.SetActive(true);
        balloonController.ChangeTextWithExit(text);
    }
    public void ChangeBText(string text)
    {
        balloonController.gameObject.SetActive(true);
        balloonController.ChangeText(text);
    }
    public void ChangeMText(string text)
    {
        messageManager.gameObject.SetActive(true);
        messageManager.ChangeText(text);
    }

    public void UpdateAllBar()
    {
        uiLike.UpdateBarWithoutAnimation();
        workStationController.uIWorkManager.UpdateBar();
        bedController.hpManager.UpdateBar();
        UIMoneyManager.UpdateMoneyTextWithoutAnimation();
        UIHPProgress.UpdateHPBar();
        UIHPProgress.UpdateProgressBar();
    }

    public void ChangeState(CharacterController.State state)
    {
        characterController.state = state;
    }

    public void ActiveButtons()
    {
        if (buttonManager.gameObject.transform.position.y >= 0)
        {
                 
            CloseButtons(); 

        }
        else
        {
            if(InfoManager.Instance.GameInfo.hp<30)
            {
                buttonManager.transform.GetChild(1).GetComponent<Button>().enabled = false;
            }
            else
            {
                buttonManager.transform.GetChild(1).GetComponent<Button>().enabled = true;
            }
            CloseStatLv();
            CloseInventory();
            buttonManager.gameObject.transform.DOMoveY(0f, 0.3f);
        }
    }
    public void CloseButtons()
    {       
        buttonManager.gameObject.transform.DOMoveY(-200f, 0.3f);
        CloseCollection();             
    }

    public void ActiveHPProgressBar()
    {
        if(UIHPProgress.gameObject.transform.GetChild(0).transform.position.y >= 550)
        {
            UIHPProgress.gameObject.transform.GetChild(0).transform.DOMoveY(430f, 0.3f);
        }
        else
        {
            UIHPProgress.gameObject.transform.GetChild(0).transform.DOMoveY(600f, 0.3f);
        }
    }

    public void ActiveNotHere()
    {
        if (uIShowNotHere.gameObject.activeInHierarchy)
        {
            uIShowNotHere.gameObject.SetActive(false);
        }
        else
        {
            uIShowNotHere.gameObject.SetActive(true);
        }
    }

    public void ActiveStatLv()
    {
        if (characterController.state == CharacterController.State.Search)
        {
            ChangeTText("현재 탐색중이라 불가능하다.");
            return;
        }
        statLevelManager.gameObject.SetActive(true);        
        statLevelManager.ShowStats();
        CloseButtons();
    }
    public void CloseStatLv()
    {
        statLevelManager.gameObject.SetActive(false);
    }
}
