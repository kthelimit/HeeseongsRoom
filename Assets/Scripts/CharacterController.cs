using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    public enum State { Work, Sleep, Search, None };
    public RectTransform canvasRect; // 말풍선 회전을 수정하기 위한 변수

    public GameManager gameManager;
    public MainController mainController;
    public Animator animator;
    Transform tr; //자기 자신의 좌표
    public Vector3 targetPos; //목적지의 좌표
    public Coroutine coroutine; //걷기 동작을 위한 코루틴
    public bool isReact = false; //캐릭터 클릭시 이동을 막기 위한 변수
    public State state = State.None;
    public int likeLevel;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        tr = transform;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameManager.GetComponent<AutoCharacterController>().lastClickTime = 0;
        }
        if (Input.GetMouseButtonDown(0) && !isReact)
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //만약 이동중이었다면 멈춘다.
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    if (state == State.Sleep)
                    {
                        mainController.ChangeBText("응...? 아직 자지 말라는 거야...?");
                    }
                    else if (state == State.Work)
                    {
                        mainController.ChangeBText("응...? 일하라는 거 아니었어...?");
                    }
                    state = State.None;

                }
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.x = Mathf.Clamp(mousePos.x, -3.66f, 3.66f);
                targetPos = new Vector3(mousePos.x, tr.position.y, 0);
                coroutine = StartCoroutine(Move());
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                if (state == State.Sleep)
                {
                    mainController.ChangeBText("응...? 아직 자지 말라는 거야...?");
                }
                else if (state == State.Work)
                {
                    mainController.ChangeBText("응...? 일하라는 거 아니었어...?");
                }
                state = State.None;
                animator.SetBool("isMove", false);


            }
        }

    }

    //이동용 코루틴
    public IEnumerator Move()
    {
        while (true)
        {
            yield return null;
            float distance = tr.position.x - targetPos.x;
            if (distance > 0)
            {
                tr.rotation = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                tr.rotation = new Quaternion(0, -180, 0, 0);
            }
            if (Mathf.Abs(distance) < 0.1f)
            {
                animator.SetBool("isMove", false);
                tr.position = targetPos;
                break;
            }
            else
            {
                animator.SetBool("isMove", true);
                tr.position += (targetPos - tr.position).normalized * Time.deltaTime;
            }
        }
        if (state == State.Work)
        {
            isReact = true;
            gameObject.SetActive(false);
            gameManager.StartWork();
        }
        else if (state == State.Sleep)
        {
            isReact = true;
            gameObject.SetActive(false);
            gameManager.StartSleep();
        }
    }

    //클릭시 반응
    public void Reaction()
    {
        CheckLike();

        animator.SetTrigger("isClicked");
        LineData lineData = DataManager.Instance.GetRandomLineData(likeLevel);
        mainController.ChangeBText(lineData.line);
        gameManager.GetLike(lineData.like);    

    }

    public void CheckLike()
    {
        if (InfoManager.Instance.GameInfo.like >= 500)
        {
            likeLevel = 5;
        }
        else if (InfoManager.Instance.GameInfo.like >= 400)
        {
            likeLevel = 4;
        }
        else if (InfoManager.Instance.GameInfo.like >= 300)
        {
            likeLevel = 3;
        }
        else if (InfoManager.Instance.GameInfo.like >= 200)
        {
            likeLevel = 2;
        }
        else if (InfoManager.Instance.GameInfo.like >= 100)
        {
            likeLevel = 1;
        }
        else
        {
            likeLevel = 0;
        }
        animator.SetInteger("likeLevel", likeLevel);
    }



    private void OnMouseDown()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            state = State.None;
            animator.SetBool("isMove", false);
        }
        Reaction();
    }

    private void OnMouseEnter()
    {
        isReact = true;
    }

    private void OnMouseExit()
    {
        isReact = false;
    }

    public void ChangeTargetPos(float x)
    {
        targetPos = new Vector3(x,tr.position.y, 0);
    }

}
