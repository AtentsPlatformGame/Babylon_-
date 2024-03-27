using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyState : EnemyMovement
{
    public enum State
    {
        Create, Normal, Roaming, Battle, Death, Missing
    }
    public State myState = State.Create;
    public LayerMask groundMask;
    public LayerMask moveLimitMask;
    public Rigidbody rigid;
    public Transform hpViewPos;
    public float jumpForce;

    Vector3 startPos;
    Vector3 leftLimitPos;
    Vector3 rightLimitPos;
    Vector3 limitPos;

    float playTime = 0.0f;
    bool isGround = true;

    //HpBar myHpBar;
    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Missing:
                // �������ϱ� ����ǥ�� ����.
                //2~5�� ������ ��� �ð��� ������ ���� �ڸ��� ���ư���.
                playTime = Random.Range(2.0f, 5.0f);
                MoveToOriginPos(startPos, playTime);
                // ���� �ڸ��� ���ư��� �ű⼭ �븻�� ������Ʈ�� ��ȯ
                break;
            case State.Normal:
                //RaycastHit hit = Physics.Raycast(transform.position, Vector2.down, 100.0f, groundMask);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector2.down, out hit, 10.0f, groundMask))
                {
                    if (hit.transform != null)
                    {
                        RaycastHit subHit;
                        if (Physics.Raycast(hit.point + Vector3.down * 0.5f, Vector3.forward, out subHit, 1000.0f, moveLimitMask)) // ���̴°ɷ� ������, ��������� ������
                        {
                            if (subHit.transform != null)
                            {
                                rightLimitPos = new Vector3(0, transform.position.y, subHit.point.z);
                            }
                        }


                        if(Physics.Raycast(hit.point + Vector3.down * 0.5f, Vector3.back, out subHit, 1000.0f, moveLimitMask))
                        {
                            if (subHit.transform != null)
                            {
                                leftLimitPos = new Vector3(0, transform.position.y, subHit.point.z);
                            }
                        }
                        
                    }
                    
                    float leftDist = Vector3.Distance(transform.position, leftLimitPos);
                    float rightDist = Vector3.Distance(transform.position, rightLimitPos);

                    limitPos = (leftDist > rightDist) ? leftLimitPos: rightLimitPos ;

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

    Vector3 GetRndPos()
    {
        Vector3 dir = Vector3.forward;
        dir = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0) * dir;
        dir *= Random.Range(0.0f, 3.0f);
        return startPos + dir;
    }

    void StateProcess()
    {
        switch (myState)
        {
            case State.Roaming:
                break;
            case State.Battle:
                break;
        }
    }

    IEnumerator DelayChangeState(State s, float t)
    {
        yield return new WaitForSeconds(t);
        ChangeState(s);
    }

    //�ڽ��� �ݰ� 3���� �̳��� ������ ��ġ�� ��� �ι� �Ѵ�.
    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        //GameObject.Find("HpBars");
        /*GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HpStatus"),
         SceneData.Instance.hpBarsTransform);
        myHpBar = obj.GetComponent<HpBar>();
        myHpBar.myTarget = hpViewPos;
        base.changeHpAct.AddListener(myHpBar.ChangeHpSlider);*/

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

    public void FindTarget(Transform target)
    {
        if (myState == State.Death) return;
        myTarget = target;
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
        ChangeState(State.Death);
    }

    public void DisApear()
    {
        StartCoroutine(DisApearing(2.0f));
    }

    IEnumerator DisApearing(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Destroy(myHpBar.gameObject);

        float dist = 2.0f;
        while (dist > 0.0f)
        {
            float delta = 0.5f * Time.deltaTime;
            dist -= delta;
            transform.Translate(Vector3.down * delta, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }

    void MoveToOriginPos(Vector3 originPos, float playTime)
    {
        StartCoroutine(MovingToOringPos(originPos, playTime));
    }

    IEnumerator MovingToOringPos(Vector3 originPos, float playTime)
    {
        Vector3 dir = originPos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        if (rotate != null) StopCoroutine(rotate);
        rotate = StartCoroutine(Rotating(dir));


        while (!Mathf.Approximately(dist, 0.0f))
        {
            float delta = moveSpeed * Time.deltaTime;
            if (delta > dist) delta = dist;
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            // �� �տ��� ���̸� �� �űⰡ ���̶�� ������ �Ѵ�.
            if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.2f, 0.0f), transform.forward, 1.1f, groundMask))
            {
                if (isGround)
                {
                    Jump();
                }
            }
            yield return null;
        }

        yield return StartCoroutine(DelayChangeState(State.Normal, playTime));
    }

    void IsGround()
    {
        isGround = Physics.Raycast(transform.position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, 1.1f, groundMask);
        //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.down, Color.blue);

        myAnim.SetBool("IsGround", isGround);
        if (isGround)
        {
            Debug.Log("hit");
        }
    }

    void Jump()
    {
        rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
