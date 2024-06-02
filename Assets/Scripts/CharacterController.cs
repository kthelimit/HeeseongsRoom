using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    public enum State { Work, Sleep, Search, None };
    public RectTransform canvasRect; // ��ǳ�� ȸ���� �����ϱ� ���� ����

    public GameManager gameManager;
    public MainController mainController;
    public Animator animator;
    Transform tr; //�ڱ� �ڽ��� ��ǥ
    public Vector3 targetPos; //�������� ��ǥ
    public Coroutine coroutine; //�ȱ� ������ ���� �ڷ�ƾ
    public bool isReact = false; //ĳ���� Ŭ���� �̵��� ���� ���� ����
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
                //���� �̵����̾��ٸ� �����.
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    if (state == State.Sleep)
                    {
                        mainController.ChangeBText("��...? ���� ���� ����� �ž�...?");
                    }
                    else if (state == State.Work)
                    {
                        mainController.ChangeBText("��...? ���϶�� �� �ƴϾ���...?");
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
                    mainController.ChangeBText("��...? ���� ���� ����� �ž�...?");
                }
                else if (state == State.Work)
                {
                    mainController.ChangeBText("��...? ���϶�� �� �ƴϾ���...?");
                }
                state = State.None;
                animator.SetBool("isMove", false);


            }
        }

    }

    //�̵��� �ڷ�ƾ
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

    //Ŭ���� ����
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
