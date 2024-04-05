using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LastBoss_FlyingDemonKing : EnemyState
{
    public UnityEvent changePlayerCameraSet;
    [Header("������ ���� vfx"), Space(5)]
    public Transform clawAttackEffect; // ������ ���� vfx
    [Header("������ ���� vfx"), Space(5)]
    public Transform biteAttackEffect; // ������ ���� vfx
    [Header("���Ÿ� ���� vfx"), Space(5)]
    public Transform fireBallEffect; // ���Ÿ� ���� vfx
    [Header("���׿� ��ġ vfx"), Space(5)]
    public Transform demonMeteorPointEffect; // ���׿� vfx
    [Header("���׿� vfx"), Space(5)]
    public Transform demonMeteorEffect; // ���׿� vfx

    [Header("������ ���� ����Ʈ"), Space(5)]
    public Transform clawAttackPoint; // ������ ���� ����Ʈ
    [Header("������ ���� ����Ʈ"), Space(5)]
    public Transform biteAttackPoint; // ������ ���� ����Ʈ
    [Header("���Ÿ� ���� �߻� ����Ʈ"), Space(5)]
    public Transform fireBallPoint; // ���Ÿ� ���� �߻� ����Ʈ
    [Header("���� ���׿� ���� ��ġ"), Space(5)]
    public Transform demonMeteorPoint; // ���� ���׿� ���� ��ġ

    [SerializeField] Transform spawnTombStone;
    [SerializeField] Collider bossCollider;
    [SerializeField] bool isPhaseChanged = false;
    [SerializeField] bool isSpawnStart = false;
    [SerializeField, Header("�ƿ����� �ۼ���")] Transform outLinePerception;
    [SerializeField, Header("�ζ��� �ۼ���")] Transform inLinePerception;
    [SerializeField, Header("���׿� ���� ��Ÿ��")] float meteorCoolTime;

    float meteorTime;
    Vector3 beforeSpawnPos;

    #region ChangeState
    protected override void ChangeState(State s)
    {
        base.ChangeState(s);
        switch (myState)
        {
            case State.Phase:
                StopAllCoroutines();
                rigid.useGravity = false;
                bossCollider.enabled = false;
                myAnim.SetBool("IsRoaming", false);
                myAnim.SetBool("IsRunning", false);
                myAnim.SetTrigger("Spawn");
                // ���� ��ȯ��
                // ���� ��ȯ�ϰ� �ڱ�� �ϴ� ���� �ö󰣴�.
                // �ð� ���� �� ����� Ŭ�������� ���ϸ� �Ǹ� �����̻� ȸ���ϰ� �����ϸ� �ٽ� �Ʒ��� ������ ������
                break;
            case State.Phase2:
                StopAllCoroutines();
                myAnim.SetBool("IsRoaming", false);
                myAnim.SetBool("IsRunning", false);
                myAnim.SetTrigger("Phase2");
                // ������2 �ڷ�ƾ �����ϱ�, ���⼱ �÷��̾� ��Ʈ�� ��� �ٲٰ� ī�޶� �ٲٰ� ���̵�ƿ��� �ؾ��ϰ� �Ұ� ����
                // ��� �ٲ� �� ��Ʈ�� ���ϰ� �ߴٰ� Ư�� ��ư�̳� ������ Ŭ���ϴ� �� �ּ� �װ� ������ �ٽ� ������ �� �ִ�.
                // ������2���� �Ϲ� ���� 3���� ���� ��Ÿ���� �ִ� ���� ��ġ�� ���׿� ����ϱ� ������ �߰��Ѵ�.
                // ������2���� �Ϲ� ���� �������� �ӵ��� �����Ѵ�.
                break;
            case State.Dizzy:
                myAnim.SetTrigger("Dizzy");
                rigid.useGravity = true;
                bossCollider.enabled = true;
                StartCoroutine(DelayChangeState(State.Battle, 5f));
                break;
            default:
                break;
        }
    }
    #endregion

    #region Start,Upate
    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        startPos = transform.position;
        base.ChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        base.StateProcess();
        if (this.myState == State.Death) return;
        if (!isSpawnStart && this.curHP <= (this.battleStat.MaxHp * 0.6))
        {
            ChangeState(State.Phase);
            isSpawnStart = true;
        }
        else if (isSpawnStart && !isPhaseChanged && this.curHP <= (this.battleStat.MaxHp) * 0.4)
        {
            ChangeState(State.Phase2);
            isPhaseChanged = true;
        }
        if (myState != State.Death)
        {
            base.IsGround();
        }
    }
    #endregion

    #region ���� ����
    public void OnClawAttack()
    {
        Collider[] list = Physics.OverlapSphere(clawAttackPoint.position, 4.5f, enemyMask);

        foreach (Collider col in list)
        {
            IDamage act = col.GetComponent<IDamage>();
            if (act != null)
            {
                act.TakeDamage(this.battleStat.AP);
            }
        }
    }

    public void OnBiteAttack()
    {
        Collider[] list = Physics.OverlapSphere(biteAttackPoint.position, 3.0f, enemyMask);

        foreach (Collider col in list)
        {
            IDamage act = col.GetComponent<IDamage>();
            if (act != null)
            {
                act.TakeDamage(this.battleStat.AP);
            }
        }
    }

    protected override IEnumerator AttackingTarget(Transform target)
    {
        while (target != null)
        {
            myAnim.SetBool("IsRunning", true);
            int pattern = Random.Range(0, 4);
            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude - battleStat.AttackRange;
            if (dist < 0.00001f) dist = 0.0f;
            float delta;
            if (!myAnim.GetBool("IsAttacking")) battleTime += Time.deltaTime;
            meteorCoolTime += Time.deltaTime;
            
            if (pattern != 2) // ���Ÿ� ������ �ƴ϶��
            {
                if (Mathf.Approximately(dist, 0.0f)) // ���� ����
                {
                    myAnim.SetBool("IsRunning", false);
                    if (battleTime >= battleStat.AttackDelay)
                    {
                        battleTime = 0.0f;
                        if (pattern == 0)
                        {
                            myAnim.SetTrigger("Attack1");
                        }
                        else if (pattern == 1)
                        {
                            myAnim.SetTrigger("Attack2");
                        }
                    }
                }
                else
                {
                    dir.Normalize();
                    delta = moveSpeed * Time.deltaTime;
                    if (delta > dist) delta = dist;
                    transform.Translate(dir * delta, Space.World);
                    if (Mathf.Approximately(dist, 0.0f))
                    {
                        myAnim.SetBool("IsRunning", false);
                    }
                }
            }
            else
            {
                if (pattern == 3)
                {
                    myAnim.SetBool("IsRunning", false);
                    if (battleTime >= battleStat.AttackDelay)
                    {
                        battleTime = 0.0f;
                        myAnim.SetTrigger("Attack3");
                    }
                }
                else
                {
                    if (meteorTime >= meteorCoolTime && isPhaseChanged)
                    {
                        meteorTime = 0.0f;
                        // ���׿� �߻� Ʈ���Ÿ� �Ǵ�.
                        //myAnim.SetTrigger("SpecialAttack");


                    }
                }

            }

            float angle = Vector3.Angle(transform.forward, dir);
            float rotDir = Vector3.Dot(transform.right, dir) < 0.0f ? -1.0f : 1.0f;
            delta = rotSpeed * Time.deltaTime;
            if (delta > angle) delta = angle;
            transform.Rotate(Vector3.up * rotDir * delta);


            yield return null;
        }
        myAnim.SetBool("IsRunning", false);
    }
    #endregion

    #region ���� �� �������
    protected override IEnumerator DisApearing(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Destroy(myHpBar.gameObject);
        float _fillAmount = 0.0f;
        while (_fillAmount < 1.0f)
        {
            _fillAmount = Mathf.Clamp(_fillAmount + Time.deltaTime, 0.0f, 1.0f);
            foreach (Renderer renderer in allRenderer)
            {
                renderer.material.SetFloat("_DissolveAmount", _fillAmount);
            }
            yield return null;
        }
        Destroy(gameObject);
    }
    #endregion

    #region �⺻���� ����Ʈ ����
    public void ClawAttackEffect()
    {
        Transform obj;
        obj = Instantiate(clawAttackEffect, clawAttackPoint.transform.position, Quaternion.Euler(-60.0f, transform.rotation.eulerAngles.y, -90.0f), null);
        obj.localScale = new Vector3(2, 2, 2);
    }

    public void BiteAttackEffect()
    {
        Transform obj;
        obj = Instantiate(biteAttackEffect, biteAttackPoint.transform.position, Quaternion.identity, null);
        obj.localScale = new Vector3(2, 2, 2);
    }

    public void FireBallAttackEffect()
    {
        Debug.Log(myTarget.transform.position);
        if (myTarget != null)
        {
            Vector3 dir = myTarget.position - fireBallPoint.position;
            dir.Normalize();
            float angle = Vector3.Angle(dir, fireBallPoint.forward);
            Debug.Log(angle);
            Transform obj;
            obj = Instantiate(fireBallEffect, fireBallPoint.transform.position, Quaternion.Euler(angle, transform.rotation.eulerAngles.y, 0.0f), null);
            obj.localScale = new Vector3(2, 2, 2);
        }
    }
    #endregion

    #region �� ��ȯ ����

    public void SpawnTombStone()
    {
        StartCoroutine(SpawningTombStone());
    }

    IEnumerator SpawningTombStone()
    {
        beforeSpawnPos = transform.position;
        outLinePerception.gameObject.SetActive(false);
        inLinePerception.gameObject.SetActive(false);
        ChangeState(State.Create);
        rigid.useGravity = false;
        Vector3 upPos = transform.position + new Vector3(0, 20, 0);
        yield return StartCoroutine(MoveForGimic(upPos));
        /*
        Vector3 dir = upPos - fireBallPoint.position;
        float dist = dir.magnitude;
        dir.Normalize();
        float delta = Time.deltaTime * 20.0f;
        while (!Mathf.Approximately(dist, 0.0f))
        {
            if (dist < delta) dist = delta;
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }*/
        yield return new WaitForSeconds(1f);
        if (spawnTombStone != null) spawnTombStone.gameObject.SetActive(true);
        yield return null;
    }

    public void Dizzy()
    {
        StartCoroutine(Dizzing());
    }

    IEnumerator Dizzing()
    {
        yield return StartCoroutine(MoveForGimic(beforeSpawnPos));
        /*Vector3 dir = beforeSpawnPos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        float delta = Time.deltaTime * 20.0f;
        while (!Mathf.Approximately(dist, 0.0f))
        {
            if (dist < delta) dist = delta;
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            yield return null;
        } // �� ������*/
        ChangeState(State.Dizzy);
        // ������ �ɸ����� ���� �߰�
        yield return new WaitForSeconds(5f);
        outLinePerception.gameObject.SetActive(true);
        inLinePerception.gameObject.SetActive(true);
    }

    IEnumerator MoveForGimic(Vector3 _pos)
    {
        Debug.Log($"{_pos}�� �̵���");
        Vector3 dir = _pos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        float delta = Time.deltaTime * 20.0f;
        while (!Mathf.Approximately(dist, 0.0f))
        {
            if (dist < delta) dist = delta;
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }
    }

    public void FailGimic()
    {
        StartCoroutine(Healing());
    }

    IEnumerator Healing()
    {
        bossCollider.enabled = true;
        // ���� ���� �߰�
        yield return StartCoroutine(MoveForGimic(beforeSpawnPos));
        ChangeState(State.Battle);
        outLinePerception.gameObject.SetActive(true);
        inLinePerception.gameObject.SetActive(true);
        rigid.useGravity = true;
        this.curHP += this.battleStat.MaxHp * 0.2f;
    }
    #endregion

    #region ������2
    public void ChangePhase2()
    {
        changePlayerCameraSet?.Invoke();
    }
    #endregion
}
