using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LastBoss_FlyingDemonKing : EnemyState
{
    public UnityEvent changePlayerCameraSet;
    [Header("������ ���� vfx"), Space(5)]
    public Transform clawAttackEffect; // ������ ���� vfx
    [Header("������ ���� vfx"), Space(5)]
    public Transform biteAttackEffect; // ������ ���� vfx
    [Header("���Ÿ� ���� vfx"), Space(5)]
    public Transform fireBallEffect; // ���Ÿ� ���� vfx

    [Header("������ ����")]
    public AudioClip clawSound; // ������ ����
    [Header("������ ����")]
    public AudioClip biteSound; // ������ ����
    [Header("�ҽ�� ����")]
    public AudioClip fireBallSound; // �ҽ�� ����
    [Header("��ȿ ����")]
    public AudioClip roarSound; // ��ȿ ����
    [Header("���׿� ����")]
    public AudioClip meteorSound; // ���׿� ����
    [Header("�� ��ȯ ����")]
    public AudioClip zeoliteSound; // �񼮼�ȯ ����
    [Header("�ǰ� ����")]
    public AudioClip hitSound; // �ǰ� ����
    [Header("��� ����")]
    public AudioClip deadSound; // ��� ����
    [Header("�����")]
    public AudioClip bgSound; // ����� ����


    [Header("������ ���� ����Ʈ"), Space(5)]
    public Transform clawAttackPoint; // ������ ���� ����Ʈ
    [Header("������ ���� ����Ʈ"), Space(5)]
    public Transform biteAttackPoint; // ������ ���� ����Ʈ
    [Header("���Ÿ� ���� �߻� ����Ʈ"), Space(5)]
    public Transform fireBallPoint; // ���Ÿ� ���� �߻� ����Ʈ
    
    [SerializeField] Transform spawnTombStone;
    [SerializeField] Collider bossCollider;
    [SerializeField] bool isPhaseChanged = false;
    [SerializeField] bool isSpawnStart = false;
    [SerializeField, Header("�ƿ����� �ۼ���")] Transform outLinePerception;
    [SerializeField, Header("�ζ��� �ۼ���")] Transform inLinePerception;
    [SerializeField, Header("���׿� ���� ��Ÿ��")] float meteorDelay = 1.0f;
    [SerializeField, Header("Ŭ���� ��Ż")] Transform clearPortal;
    [SerializeField, Header("���� ���� UI")] GameObject bossHpBar;
    [SerializeField, Header("���� ���� �����̴�")] Slider bossHpSlider;

    float meteorCoolTime;
    Vector3 beforeSpawnPos;

    #region ChangeState
    protected override void ChangeState(State s)
    {
        base.ChangeState(s);
        switch (myState)
        {
            case State.Phase:
                StopAllCoroutines();
                
                foreach (Renderer renderer in allRenderer)
                {
                    Color tmpColor = new Color(0, 0, 0, 0);
                    renderer.material.color = tmpColor;
                }
                
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
                foreach (Renderer renderer in allRenderer)
                {
                    Color tmpColor = new Color(0, 0, 0, 0);
                    renderer.material.color = tmpColor;
                }
                myAnim.SetBool("IsRoaming", false);
                myAnim.SetBool("IsRunning", false);
                myAnim.SetTrigger("Phase2");
                rigid.useGravity = true;
                bossCollider.enabled = true;
                StartCoroutine(DelayChangeState(State.Battle, 5f));
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
            case State.Create:
                break;
            default:
                break;
        }
    }
    #endregion

    #region Enable,Start,Upate
    // Start is called before the first frame update

    private void OnEnable()
    {
        if (bossHpBar != null && !bossHpBar.activeSelf) bossHpBar.SetActive(true);
    }
    void Start()
    {
        base.Initialize();
        startPos = transform.position;
        base.ChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.myState == State.Death) return;
        base.StateProcess();

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
        PlaySound(clawSound);
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
            
            BattleSystem bs = target.gameObject.GetComponent<BattleSystem>();
            if(bs != null)
            {
                if (!bs.isAlive())
                {
                    ChangeState(State.Create);
                    StopAllCoroutines();
                }
            }
            myAnim.SetBool("IsRunning", true);
            int pattern = 0;
            if (isPhaseChanged)
            {
                if (meteorCoolTime >= meteorDelay)
                {
                    meteorCoolTime = 0.0f;
                    // ���׿� �߻� Ʈ���Ÿ� �Ǵ�.
                    PlaySound(roarSound);
                    myAnim.SetTrigger("SpecialAttack");
                }
            }
            pattern = Random.Range(0,3);
            
            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude - battleStat.AttackRange;
            if (dist < 0.00001f) dist = 0.0f;
            float delta;
            if (!myAnim.GetBool("IsAttacking"))
            {
                battleTime += Time.deltaTime;
                meteorCoolTime += Time.deltaTime;
            }
            
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
                    delta = battleStat.MoveSpeed * Time.deltaTime;
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
                myAnim.SetBool("IsRunning", false);
                if (pattern == 2)
                {
                    if (battleTime >= battleStat.AttackDelay)
                    {
                        battleTime = 0.0f;
                        myAnim.SetTrigger("Attack3");
                    }
                }
                
            }

            float angle = Vector3.Angle(transform.forward, dir);
            float rotDir = Vector3.Dot(transform.right, dir) < 0.0f ? -1.0f : 1.0f;
            delta = rotSpeed * Time.deltaTime;
            if (delta > angle) delta = angle;
            transform.Rotate(Vector3.up * rotDir * delta);

            Debug.Log($"�÷��̾���� �Ÿ� : {dist}");
            yield return null;
        }
        myAnim.SetBool("IsRunning", false);
    }
    #endregion

    #region ���� �� �������

    public override void TakeDamage(float _dmg)
    {
        curHP -= _dmg;
        Debug.Log(curHP);
        PlaySound(hitSound);
        if (curHP <= 0.0f)
        {
            // ü���� �� �� ������
            //PlaySound(deadSound);
            OnDead();
            myAnim.SetTrigger("Dead");
            clearPortal.gameObject.SetActive(true);
        }
        else
        {
            myAnim.SetTrigger("Damage");
            //StartCoroutine(DamagingDemon());
        }
        bossHpSlider.value = this.curHP / this.battleStat.MaxHp;
    }
    IEnumerator DamagingDemon()
    {
        foreach (Renderer renderer in allRenderer)
        {
            Color tmpColor = new Color(255,0,0,0.3f);
            
            renderer.material.color = tmpColor;
        }
        yield return new WaitForSeconds(0.3f);
        foreach (Renderer renderer in allRenderer)
        {
            Color tmpColor = new Color(0, 0, 0, 0);
            renderer.material.color = tmpColor;
        }
    }
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
        if(bossHpBar != null)bossHpBar.SetActive(false);
        Destroy(gameObject);
    }
    #endregion

    #region �⺻���� ����Ʈ ����
    public void ClawAttackEffect()
    {
        PlaySound(clawSound);
        Transform obj;
        obj = Instantiate(clawAttackEffect, clawAttackPoint.transform.position, Quaternion.Euler(-60.0f, transform.rotation.eulerAngles.y, -90.0f), null);
        obj.localScale = new Vector3(2, 2, 2);
    }

    public void BiteAttackEffect()
    {
        PlaySound(biteSound);
        Transform obj;
        obj = Instantiate(biteAttackEffect, biteAttackPoint.transform.position, Quaternion.identity, null);
        obj.localScale = new Vector3(2, 2, 2);
    }

    public void FireBallAttackEffect()
    {
        PlaySound(fireBallSound);
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
        Debug.Log("�� ��ȯ ���� ����");
        //��ȿ�� �ϰ�, �� ��ȯ
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
        Debug.Log("�̵� ��");
        yield return new WaitForSeconds(1f);
        Debug.Log("�� ��ȭ�Ⱦָ�");
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
        
        ChangeState(State.Dizzy);
        // ������ �ɸ����� ���� �߰�
        yield return new WaitForSeconds(5f);
        outLinePerception.gameObject.SetActive(true);
        inLinePerception.gameObject.SetActive(true);
    }

    IEnumerator MoveForGimic(Vector3 _pos)
    {
        //Debug.Log($"{_pos}�� �̵���");
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
