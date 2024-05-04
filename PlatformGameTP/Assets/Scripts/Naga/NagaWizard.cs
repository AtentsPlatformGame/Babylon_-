using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NagaWizard : EnemyState
{
    
    public Transform virticalAttackEffect;
    public Transform horizontalAttackEffect;

    public Transform slashPoint;
    public Transform spawnPoint;
    public GameObject TsunamiLeft;
    public GameObject TsunamiRight;
    public GameObject BeforeTsunami;
    public GameObject WarningSign;
    public GameObject TidalWave;
    public GameObject TidalWaveWarning;
    public GameObject SafeArea1;
    public GameObject NagaToken;
    public GameObject NagaClearText;

    [Header("���׿� ����")]
    public AudioClip meteorSound; // ���׿� ����
    [Header("������ ����")]
    public AudioClip gunSound; // ������ ����
    [Header("�ĵ� ����")]
    public AudioClip wavesSound; // �ĵ� ����
    [Header("���� ����")]
    public AudioClip tsunamiSound; // ���� ����
    [Header("��ȿ ����")]
    public AudioClip roarSound; // ��ȿ ����
    [Header("���� ����")]
    public AudioClip warningSound; // ����(���) ����
    [Header("�ǰ� ����")]
    public AudioClip hitSound; // �ǰ� ����
    [Header("��� ����")]
    public AudioClip deadSound; // ��� ����
    [Header("���� �� �Ѿ��")]
    public GameObject nextStage;

    public int PhaseCount = 0;
    public GameObject GreetingBoss;
    static int greetingCounts = 0;

    [SerializeField] bool isPhaseChanged = false;

    public Vector3 SpecialPatternPos;
    // Start is called before the first frame update
    protected override void ChangeState(State s)
    {
        base.ChangeState(s);
        switch (myState)
        {
            case State.Phase:
                StopAllCoroutines();
                myAnim.SetBool("IsRoaming", false);
                myAnim.SetBool("IsRunning", false);
                myAnim.SetTrigger("Spawn");
                
                break;
            default:
                break;
        }

    }
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
        TsunamiLeft.SetActive(false);
        TsunamiRight.SetActive(false);
        BeforeTsunami.SetActive(false);
        WarningSign.SetActive(false);
        GreetingBoss.SetActive(false) ;
        TidalWave.SetActive(false) ;
        SafeArea1.SetActive(false) ;
        Rewards.SetActive(false);
        NagaToken.SetActive(false);
        NagaClearText.SetActive(false);
        
        
        TidalWaveWarning.SetActive(false);
        SpecialPatternPos = new (0.0f, 2.5f, 41.0f);
        startPos = transform.position;
        base.ChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        base.StateProcess();
        if (!isPhaseChanged && this.curHp <= (this.battleStat.MaxHp * 0.5))
        {
            ChangeState(State.Phase);
            isPhaseChanged = true;
        }
        if (myState != State.Death) 
        {
            base.IsGround();
        }
        if (myState == State.Battle && greetingCounts == 0) 
        {
            GreetingBoss.SetActive(true);
            Invoke("TurnOffGreetings", 3.0f);
        
        }

    
    }

 

    public new void OnAttack()
    {
        Collider[] list = Physics.OverlapSphere(attackPoint.position, 1.0f, enemyMask);

        foreach (Collider col in list)
        {
            IDamage act = col.GetComponent<IDamage>();
            if (act != null)
            {
                act.TakeDamage(30.0f);
            }
        }
    }
    protected override IEnumerator AttackingTarget(Transform target)
    {
        while (target != null)
        {
            Debug.Log("�������Ѵ�");
            myAnim.SetBool("IsRunning", true);
            int pattern = Random.Range(0, 5);

            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude - battleStat.AttackRange;
            if (dist < 0.0001f) dist = 0.0f;
            float delta;
            if (!myAnim.GetBool("IsAttacking")) battleTime += Time.deltaTime;
            Debug.Log(dist);

            BattleSystem bs = target.gameObject.GetComponent<BattleSystem>();
            
            if (bs.isAlive() != true)
            {
            }

            //Ư������
            if (bs.isAlive() == true)
            {
                if (this.curHP < this.battleStat.MaxHp * 0.8 && PhaseCount == 0)
                {
                    Debug.Log("Ư������ �ߵ�");

                    this.transform.position = SpecialPatternPos;
                    
                    myAnim.SetTrigger("SpecialPattern");
                    PlaySound(warningSound);
                    PlaySound(roarSound);
                    WarningSign.SetActive(true);
                    BeforeTsunami.SetActive(true);
                    yield return new WaitForSeconds(5.0f);
                    BeforeTsunami.SetActive(false);
                    WarningSign.SetActive(false);
                    PlaySound(tsunamiSound);
                    TsunamiLeft.SetActive(true);
                    TsunamiRight.SetActive(true);
                    yield return new WaitForSeconds(2.0f);

                    TsunamiLeft.SetActive(false);
                    TsunamiRight.SetActive(false);
                    
                    PhaseCount = 1;
                }
                if (this.curHP < this.battleStat.MaxHp * 0.5 && PhaseCount == 1)
                {
                    this.transform.position = SpecialPatternPos;
                    myAnim.SetTrigger("SpecialPattern");
                    Debug.Log("Ư������ �ߵ�");
                    PlaySound(warningSound);
                    PlaySound(roarSound);
                    TidalWaveWarning.SetActive(true);
                    SafeArea1.SetActive(true);

                    yield return new WaitForSeconds(3.0f);
                    TidalWaveWarning.SetActive(false);
                    PlaySound(wavesSound);
                    TidalWave.SetActive(true);
                    yield return new WaitForSeconds(5.0f);

                    SafeArea1.SetActive(false);
                    TidalWave.SetActive(false);
                    myAudioSource.Stop();
                    PhaseCount = 2;
                }
                if (this.curHP < this.battleStat.MaxHp * 0.3 && PhaseCount == 2)
                {
                    Debug.Log("Ư������ �ߵ�");

                    this.transform.position = SpecialPatternPos;

                    myAnim.SetTrigger("SpecialPattern");
                    PlaySound(warningSound);
                    PlaySound(roarSound);
                    WarningSign.SetActive(true);
                    BeforeTsunami.SetActive(true);

                    yield return new WaitForSeconds(5.0f);
                    BeforeTsunami.SetActive(false);
                    WarningSign.SetActive(false);
                    PlaySound(tsunamiSound);
                    TsunamiLeft.SetActive(true);
                    TsunamiRight.SetActive(true);
                    yield return new WaitForSeconds(2.0f);

                    TsunamiLeft.SetActive(false);
                    TsunamiRight.SetActive(false);
                    PhaseCount = 3;
                }

            }
            if (Mathf.Approximately(dist, 0.0f))
            {
                myAnim.SetBool("IsRunning", false);
                if (battleTime >= battleStat.AttackDelay)
                {
                    TsunamiLeft.SetActive(false);
                    TsunamiRight.SetActive(false);
                    battleTime = 0.0f;
                    if (pattern == 0 || pattern == 1 || pattern == 2 )
                    {
                        Debug.Log("�Ϲ�1��");
                        myAnim.SetTrigger("Attack1");
                        PlaySound(meteorSound);
                    }
                    else if(pattern == 3)
                    {

                    }
                    else if(pattern == 4)
                    {
                        Debug.Log("�Ϲ�2��");
                        myAnim.SetTrigger("Attack2");
                        PlaySound(gunSound);
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
            float angle = Vector3.Angle(transform.forward, dir);
            float rotDir = Vector3.Dot(transform.right, dir) < 0.0f ? -1.0f : 1.0f;
            delta = rotSpeed * Time.deltaTime;
            if (delta > angle) delta = angle;
            transform.Rotate(Vector3.up * rotDir * delta);


            

            yield return null;
        }
        myAnim.SetBool("IsRunning", false);
    }

    public void VirticalAttackEffect()
    {
        Instantiate(virticalAttackEffect, slashPoint.transform.position, Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f), null);
    }

    public void HorizontalAttackEffect()
    {
        Instantiate(horizontalAttackEffect, slashPoint.transform.position, Quaternion.Euler(270.0f, transform.rotation.eulerAngles.y, 0.0f), null);
    }

    public void TurnOffGreetings()
    {
        GreetingBoss.SetActive(false);
        greetingCounts = 1;
    }
    protected override void OnDead()
    {
        base.OnDead();
        StartCoroutine(ClearText());
        StartCoroutine(ClearTextFinish());

        StopCoroutine(ClearText());
        StopCoroutine(ClearTextFinish());
        dropGoldAct?.Invoke(dropGold);
        Debug.Log("����");
        Rewards.SetActive(true);
        nextStage.SetActive(true);

        // �÷��̾����� ��带 �ְ� �÷��̾ �κ��丮�� ų �� �κ��丮�� �� ������ �����ͼ� ��带 �����Ѵ�. �׸��� �ٽ� �÷��̾� ��嵵 �����Ѵ�.
        ChangeState(State.Death);
    }
    public IEnumerator ClearText()
    {
        NagaClearText.SetActive(true);
        yield return null;
    }
    public IEnumerator ClearTextFinish()
    {
        yield return new WaitForSeconds(3.0f);
        NagaClearText.SetActive(false);
        yield return null;
    }
}

