using System.Collections;
using UnityEngine;

public class AutoCharacterController : MonoBehaviour
{
    public GameManager gameManager;
    public float startClickTime = 15f;
    public float lastClickTime = 0;
    public float lastCommand = 0;
    public float likeUpTime = 0;
    WeightRandom WeightRandom = new WeightRandom();
    private void Start()
    {
        StartCoroutine(AutoControll());
    }

    IEnumerator AutoControll()
    {
        WeightRandom.inputDatas.Add("Search", 150);
        WeightRandom.inputDatas.Add("Move1", 1000);
        WeightRandom.inputDatas.Add("Move2", 1000);
        WeightRandom.inputDatas.Add("Work", 450);
        WeightRandom.inputDatas.Add("Sleep", 250);
        WeightRandom.Init();
        string whatToDo;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            likeUpTime++;

            if (likeUpTime >= 300)
            {
                gameManager.GetLike(5);
                likeUpTime = 0;
            }

            if (gameManager.mainController.characterController.state == CharacterController.State.None)
            {
                lastClickTime += 1;
                lastCommand += 1;
            }
            if (lastClickTime >= startClickTime)
            {
                if (lastCommand >= 7)
                {
                    if (InfoManager.Instance.GameInfo.hp <= 20)
                    {
                        gameManager.GotoSleep();
                        lastCommand = 0;
                    }


                    whatToDo = WeightRandom.PickRandom();
                    if (whatToDo == "Search")
                    {
                        gameManager.Search();
                        lastCommand = 0;
                    }
                    else if (whatToDo == "Move1")
                    {
                        gameManager.mainController.characterController.ChangeTargetPos(-Random.Range(1, 3));
                        gameManager.StartMove();
                        lastCommand = 0;
                    }
                    else if (whatToDo == "Move2")
                    {
                        gameManager.mainController.characterController.ChangeTargetPos(Random.Range(1, 3));
                        gameManager.StartMove();
                        lastCommand = 0;
                    }
                    else if (whatToDo == "Work")
                    {
                        gameManager.GotoWork();
                        lastCommand = 0;
                    }
                    else if (whatToDo == "Sleep")
                    {
                        gameManager.GotoSleep();
                        lastCommand = 0;
                    }
                    //int rand = Random.Range(1, 5);
                    //switch (rand)
                    //{
                    //    case 1:
                    //        gameManager.GotoSleep();
                    //        lastCommand = 0;
                    //        break;
                    //    case 2:
                    //        gameManager.GotoWork();
                    //        lastCommand = 0;
                    //        break;
                    //    case 3:
                    //        gameManager.Search();
                    //        lastCommand = 0;
                    //        break;
                    //    case 4:
                    //        gameManager.mainController.characterController.targetPos = transform.position + new Vector3(Random.Range(1, 3), 0, 0);
                    //        gameManager.StartMove();
                    //        break;
                    //    case 5:
                    //        gameManager.mainController.characterController.targetPos = transform.position - new Vector3(Random.Range(1, 3), 0, 0);
                    //        gameManager.StartMove();
                    //        break;
                    //}

                }
            }
        }

    }
}


