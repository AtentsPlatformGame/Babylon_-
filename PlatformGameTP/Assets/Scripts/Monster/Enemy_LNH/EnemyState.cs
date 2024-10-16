using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyState : EnemyMovement
{
    public enum State
    {
        Create, Normal, Roaming, Battle, Death, Missing, Detect, Phase, Phase2, Dizzy, Rush, SpecialPattern//�߰��� �κ�
    }
    public State myState = State.Create;
    public LayerMask groundMask;
    public LayerMask moveLimitMask;
    public Rigidbody rigid;
    public Transform hpViewPos;
    public GameObject detectUI;
    public GameObject missngUI;
    public GameObject Rewards;
    public UnityEvent<int> dropGoldAct;
    public float jumpForce;

    public int dropGold;
    protected Vector3 startPos;
    protected Vector3 leftLimitPos;
    protected Vector3 rightLimitPos;
    protected Vector3 limitPos;
    Color originColor;
    protected float playTime = 0.0f;
    protected bool isGround = true;
    protected float moveBackTime = 0.0f;
    bool warpBack = false;
    
    
    HpBar myHpBar;
    protected virtual void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Detect:
                // �÷��̾ �ƿ����ο� �ɸ� ��ġ�� �̵��Ѵ�.
                // �ش� ��ġ�� �̵��� Missing ������Ʈ�� ��ȯ
                // ���� �÷��̾ �� ���� �ɸ��� �˾Ƽ� battle�� ������
                MoveToPos(limitPos, () => ChangeState(State.Missing));
                break;
            case State.Missing:
                // �������ϱ� ����ǥ�� ����.
                //���� ��� �ð��� ������ ���� �ڸ��� ���ư���.
                // ���� �ڸ��� ���ư��� �ű⼭ �븻�� ������Ʈ�� ��ȯ
                StartCoroutine(MissingTarget());
                break;
            case State.Normal:
                //RaycastHit hit = Physics.Raycast(transform.position, Vector2.down, 100.0f, groundMask);
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 10.0f, groundMask))
                {
                    //Debug.Log($"{hit.point}, ���� ����");
                    if (hit.transform != null)
                    {
                        RaycastHit subHit;
                        if (Physics.Raycast(hit.point + Vector3.down * 0.5f, Vector3.forward, out subHit, 8.5f, moveLimitMask)) // ���̴°ɷ� ������, ��������� ������
                        {
                            if (subHit.transform != null)
                            {
                                rightLimitPos = new Vector3(0, transform.position.y, subHit.point.z - 0.1f);
                               //Debug.Log($"{subHit.point} : ������ ���� ��");
                            }
                        }


                        if (Physics.Raycast(hit.point + Vector3.down * 0.5f, Vector3.back, out subHit, 8.5f, moveLimitMask))
                        {
                            if (subHit.transform != null)
                            {
                                leftLimitPos = new Vector3(0, transform.position.y, subHit.point.z+0.1f);
                                //Debug.Log($"{subHit.point} : ���� ���� ��");
                            }
                        }

                    }

                    float leftDist = Vector3.Distance(transform.position, leftLimitPos);
                    float rightDist = Vector3.Distance(transform.position, rightLimitPos);

                    limitPos = (leftDist > rightDist) ? leftLimitPos : rightLimitPos;
                }

                playTime = Random.Range(1.0f, 3.0f);
                // dir�� �ݴ��, ȸ���� ��Ų��.
                StartCoroutine(DelayChangeState(State.Roaming, playTime)); // ���ư� �ڿ� �ι����� ������Ʈ�� ��ȯ
                //base.UpdateAnimState();
                // �ι����� �ٲ� �� ������ �ٲ� �ݴ�� �ι��ϰ� �Ѵ�. �̶� ȸ���� ���Ѿ���
                break;
            case State.Roaming:
                Debug.Log("�ι� ����");
                // normal state���� ����� �� �� �ִ� �ִ� ��ġ�� ������������ �� ��ġ���� �̵��Ѵ�.
                MoveToPos(limitPos, () => ChangeState(State.Normal));
                //MoveToPos(GetRndPos(), () => ChangeState(State.Normal)); // �ڱ��� �� �Ʒ� ���̸� �� �ش� ����� �¿� z ������ �̵��Ѵ�. ������ ��������
                // ��ó�� ������Ʈ�� �ٽ� �븻�� �ٲٰ� �븻������ ������ �ٲپ� �ٽ� �ι����� �ٲ۴�.
                break;
            case State.Battle:
                AttackTarget(myTarget);
                break;
            case State.Death:
                StopAllCoroutines();
                break;
           

        }
    }

    protected void StateProcess()
    {
        transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        switch (myState)
        {
            case State.Roaming:
                break;
            case State.Battle:
                break;
        }
    }

    protected IEnumerator DelayChangeState(State s, float t)
    {
        yield return new WaitForSeconds(t);
        ChangeState(s);
    }

    //�ڽ��� �ݰ� 3���� �̳��� ������ ��ġ�� ��� �ι� �Ѵ�.
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EnemyState Start");
        if (SoundManager.Instance != null && myAudioSource != null)
        {
            myAudioSource.volume = SoundManager.Instance.soundValue;
            SoundManager.Instance.SetVolumeAct.AddListener(SetVolumeSlider);
            Debug.Log("EnemyState Start, Sound check");

        }
        base.Initialize();
        //GameObject.Find("HpBars");
        GameObject obj = Instantiate(Resources.Load<GameObject>("HpStatus"),
         SceneData.Instance.hpBarsTransform);
        myHpBar = obj.GetComponent<HpBar>();
        myHpBar.myTarget = hpViewPos;
        base.changeHpAct.AddListener(myHpBar.ChangeHpSlider);

        startPos = transform.position;
        
        

        ChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        if (myState != State.Death)
        {
            IsGround();
        }
    }

    public void DetectTarget(Transform target)
    {
        if (myState == State.Death) return;
        StopAllCoroutines();
        StartCoroutine(DetectingTarget(target));
    }

    protected IEnumerator DetectingTarget(Transform target)
    {

        if (myAnim.GetBool("IsRoaming"))
        {
            myAnim.SetBool("IsRoaming", false);
        }

        if (myAnim.GetBool("IsRunning"))
        {
            myAnim.SetBool("IsRunning", false);
        }
        myAnim.SetTrigger("Detect");
        TurnOnDetectImg(detectUI);

        playTime = Random.Range(0.5f, 1.0f);
        limitPos = target.position;

        yield return StartCoroutine(DelayChangeState(State.Detect, playTime));
    }

    protected IEnumerator MissingTarget()
    {
        TurnOnDetectImg(missngUI);
        myAnim.SetBool("IsRoaming", false);
        myAnim.SetBool("IsRunning", false);

        yield return new WaitForSeconds(1.5f);
        playTime = Random.Range(0.5f, 1.0f);
        MoveToOriginPos(startPos, playTime);
    }

    public void TurnOnDetectImg(GameObject _imgSet)
    {
        if (_imgSet != null)
        {
            StartCoroutine(TurningDetectOnActionImg(_imgSet));
        }
    }

    protected IEnumerator TurningDetectOnActionImg(GameObject _imgSet)
    {
        _imgSet.gameObject.SetActive(true);
        int cnt = _imgSet.transform.childCount;

        for (int i = 0; i < cnt; i++)
        {
            Transform obj = _imgSet.transform.GetChild(i);
            obj.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FindTarget(Transform target)
    {
        if (myState == State.Death) return;
        myTarget = target;
        Debug.Log("FindTarget " + target.gameObject.name);
        ChangeState(State.Battle);
    }

    public void LostTarget()
    {
        if (myState == State.Death) return;
        myTarget = null;
        StopAttack();
        ChangeState(State.Missing);

    }


    protected override void OnDead()
    {
        base.OnDead();
        dropGoldAct?.Invoke(dropGold);
        // �÷��̾����� ��带 �ְ� �÷��̾ �κ��丮�� ų �� �κ��丮�� �� ������ �����ͼ� ��带 �����Ѵ�. �׸��� �ٽ� �÷��̾� ��嵵 �����Ѵ�.
        ChangeState(State.Death);
    }

    public void DisApear()
    {
        Debug.Log("disappear ����");
        StartCoroutine(DisApearing(2.0f));
    }

    protected virtual IEnumerator DisApearing(float delay)
    {
        Debug.Log("disappearing ���� ���");
        yield return new WaitForSeconds(delay);
        Debug.Log("disappearing ����");

        Destroy(myHpBar.gameObject);

        /*float dist = 2.0f;
        while (dist > 0.0f)
        {
            float delta = 0.5f * Time.deltaTime;
            dist -= delta;
            transform.Translate(Vector3.down * delta, Space.World);
            yield return null;
        }*/

        Destroy(gameObject);
        
    }

    protected void MoveToOriginPos(Vector3 originPos, float playTime)
    {
        StartCoroutine(MovingToOringPos(originPos, playTime));
    }

    protected IEnumerator MovingToOringPos(Vector3 originPos, float playTime)
    {
        myAnim.SetBool("IsRoaming", true);
        Vector3 dir = originPos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        if (rotate != null) StopCoroutine(rotate);
        rotate = StartCoroutine(Rotating(dir));
        

        while (!Mathf.Approximately(dist, 0.0f))
        {
            if(moveBackTime >= 5f)
            {
                warpBack = true;
                moveBackTime = 0.0f;
                break;
            }
            float delta = battleStat.MoveSpeed * Time.deltaTime;
            if (delta > dist) delta = dist;
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            // �� �տ��� ���̸� �� �űⰡ ���̶�� ������ �Ѵ�.
            if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.2f, 0.0f), transform.forward, 1.1f, groundMask))
            {
                if (isGround)
                {
                    Jump();
                    yield return new WaitForSeconds(0.3f);
                }
            }
            moveBackTime += Time.deltaTime;
            yield return null;
        }

        if (warpBack)
        {
            this.gameObject.transform.position = originPos;
            warpBack = false;
        }

        myAnim.SetBool("IsRoaming", false);
        yield return StartCoroutine(DelayChangeState(State.Normal, playTime));
    }

    protected void IsGround()
    {
        isGround = Physics.Raycast(transform.position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, 1.01f, groundMask);
        //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.down, Color.blue);

    }

    protected void Jump()
    {
        Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);

        rigid.AddForce(jumpVelocity, ForceMode.Impulse);
    }

    
}
