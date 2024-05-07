using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : BattleSystem
{
    [SerializeField][Header("�÷��̾� ȸ�� �ӵ�")] float rotSpeed = 1.0f;
    [SerializeField][Header("�÷��̾� ���� ����")] float jumpForce = 2.0f;
    [SerializeField][Header("�÷��̾� ����")] Transform spellObject;
    [SerializeField]
    [Header("�÷��̾� 2D �̵� ��� ���")] bool controll2D = true;
    [SerializeField] Vector2 rotYRange = new Vector2(0.0f, 180.0f);
    [SerializeField, Header("�÷��̾� ��Ʈ�� ����")] bool canMove = true;
    [SerializeField, Header("�ڷ���Ʈ ����Ʈ")] Transform teleportVFX;
    [SerializeField, Header("�ڷ���Ʈ �ܻ� ����Ʈ"), Space(5)] Transform teleportFogVFX;
    [SerializeField, Header("�÷��̾� ���� ����")] PlayerStatData playerStatData;

    [SerializeField, Header("���� �ʱ�ȭ�� ���� �κ��丮 ������Ʈ �Լ�")] UnityEvent<ItemStat> invenUpdate;
    [SerializeField, Header("���� �ʱ�ȭ�� ���� empty spell")] OriginSpellData spellData;
    [Header("��Ÿ ��Ÿ�� �̹���")] public Image fireballCoolTimeImg;
    [Header("�ڷ���Ʈ ��Ÿ�� �̹���")] public Image teleportCoolTimeImg;

    public UnityEvent OnStatsChanged;
    public GameObject orgFireball;
    public LayerMask groundMask;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;
    public UnityEvent<int> switchTrackedOffset;
    public GameObject DeathIMG;
    

    public bool isSpellReady = false;
    public bool is3d = true;
    public float jumpCoolTime = 0.5f;
    public float teleportCoolTime = 3.0f;

    float curRotY;
    float ap;
    bool isGround;
    bool canAttack = true;
    float attackDeltaTime = 0.0f;
    public float teleportDeltaTime = 0.0f;
    GoldManager goldManager;

    //���� ���� ��������Ʈ
    /*public delegate void StatsChangedEvent(float _ap, float _spd);
    public static event StatsChangedEvent OnStatsChanged;*/
    
    //���ݰ��� �������� �Լ���
    /*private float GetAP() { return battleStat.AP; }
    private new float GetMoveSpeed() { return battleStat.MoveSpeed; }
    //������ �����ϴ� �Լ�
    public void ModfyAP(float amount) { battleStat.AP += amount; }
    public void ModfySPD(float amount) { battleStat.MoveSpeed += amount; }*/
    //������� ���� �߰� ��

    BattleStat originalStat;
    Rigidbody rigid;
    Fireball fireBall;
    Coroutine attackDelay;
    Coroutine teleportDelay;
    Coroutine rotating;

    private void Awake()
    {
        OriginalStatInit(playerStatData.GetPlayerStatInfo());
        //Initialize();
        NotifyStatsChanged();
    }

    private void NotifyStatsChanged()
    {
        //���� ���� �� �̺�Ʈ ȣ���ϴ� �Լ�
        OnStatsChanged?.Invoke();
    }

    void Start()
    {
        curRotY = transform.localRotation.eulerAngles.y;
        rigid = this.GetComponent<Rigidbody>();
        DeathIMG.GetComponent<CanvasGroup>().alpha = 0.0f;
        goldManager = FindObjectOfType<GoldManager>();
        if (SoundManager.Instance != null && myAudioSource != null)
        {
            myAudioSource.volume = SoundManager.Instance.soundValue;
            SoundManager.Instance.SetVolumeAct.AddListener(SetVolumeSlider);
            Debug.Log("Player Start, Sound check");
        }
        teleportDelay = null;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowStat();
            Debug.Log($"curHP = {curHP}");
        }
        
        if (isAlive())
        {
            if (canMove)
            {
                if (controll2D) // �յ� 2�������� �����̰� �����ϴ� �ڵ�
                {
                    IsGround();
                    TryJump();
                    Rotate();
                    Move(); // ȸ���� ���ÿ� �����̱�
                            //if (canMove) Move(); // ȸ���� ������ �����̱�
                }
                else // �յ�, �翷 4�������� �����̴� �ڵ�,
                {
                    // Rotate3D();
                    IsGround();
                    TryJump();
                    Move3D();
                }
            }
        }
    }

    public void Initialize(float _hp)
    {
        curHP = _hp;
        PlayerUIwindows.Instance.UpdateHpbar();
    }

    public void Initialize(float _plusHp, bool isArmor)
    {
        if (isArmor)
        {
            curHP = _plusHp + this.originalStat.MaxHp;
            
            PlayerUIwindows.Instance.UpdateHpbar();
        }
        
    }

    public void Initialize(PlayerStatData basicStat)
    {
        this.battleStat.AP = basicStat.GetPlayerStatInfo().AP;
        this.battleStat.MaxHp = basicStat.GetPlayerStatInfo().MaxHp;
        this.battleStat.AttackRange = basicStat.GetPlayerStatInfo().AttackRange;
        this.battleStat.AttackDelay = basicStat.GetPlayerStatInfo().AttackDelay;
        this.battleStat.ProjectileSpeed = basicStat.GetPlayerStatInfo().ProjectileSpeed;
        this.battleStat.MoveSpeed = basicStat.GetPlayerStatInfo().MoveSpeed;
        this.battleStat.AttackSize = basicStat.GetPlayerStatInfo().AttackSize;
        this.battleStat.AttackTwice = basicStat.GetPlayerStatInfo().AttackTwice;
        this.battleStat.HealAfterAttack = basicStat.GetPlayerStatInfo().HealAfterAttack;
        this.battleStat.ResurrectionOneTime = basicStat.GetPlayerStatInfo().ResurrectionOneTime;
        this.battleStat.HitOnlyHalf = basicStat.GetPlayerStatInfo().HitOnlyHalf;
        this.battleStat.CA_AttackPenalty = basicStat.GetPlayerStatInfo().CA_AttackPenalty;
        this.battleStat.CA_GoldPenalty = basicStat.GetPlayerStatInfo().CA_GoldPenalty;
        this.battleStat.CA_HPPenalty = basicStat.GetPlayerStatInfo().CA_HPPenalty;

        
        Initialize(this.battleStat.MaxHp);

        
    }

    void OriginalStatInit(PlayerBattleStat pb)
    {
        this.originalStat.AP = pb.AP;
        this.originalStat.MaxHp = pb.MaxHp;
        this.originalStat.AttackRange = pb.AttackRange;
        this.originalStat.AttackDelay = pb.AttackDelay;
        this.originalStat.ProjectileSpeed = pb.ProjectileSpeed;
        this.originalStat.MoveSpeed = pb.MoveSpeed;
        this.battleStat.AttackSize = pb.AttackSize;
        this.battleStat.AttackTwice = pb.AttackTwice;
        this.battleStat.HealAfterAttack = pb.HealAfterAttack;
        this.battleStat.ResurrectionOneTime = pb.ResurrectionOneTime;
        this.battleStat.HitOnlyHalf = pb.HitOnlyHalf;
        this.battleStat.CA_AttackPenalty = pb.CA_AttackPenalty;
        this.battleStat.CA_GoldPenalty = pb.CA_GoldPenalty;
        this.battleStat.CA_HPPenalty = pb.CA_HPPenalty;
    }
    #region ControllChange
    public void SwitchControllType2D(bool _type)
    {
        controll2D = _type;
        if (controll2D)
        {
            Constraints2D();

        }
        else
        {
            Constraints3D();

        }
    }

    void Constraints2D()
    {
        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Constraints3D()
    {
        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
    #endregion

    #region MoveOn2D
    void Move()
    {
        Constraints2D();
        float x = Input.GetAxis("Horizontal");
        Vector3 deltaPos = transform.forward * x * Time.deltaTime * battleStat.MoveSpeed;

        if (!Mathf.Approximately(x, 0.0f) && Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Approximately(teleportDeltaTime, 0.0f)) // �ڷ���Ʈ, ���� ��Ÿ�� �߰� ����
        {
            if (teleportDelay != null)
            {
                StopCoroutine(teleportDelay);
                teleportDelay = null;
            }
            teleportDelay = StartCoroutine(CoolingTelePort());
            deltaPos += deltaPos.normalized * 1.5f;
            if (Physics.Raycast(new Ray(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward), out RaycastHit hit,
                1.5f, groundMask))
            {
                Debug.Log("���� ����");
                deltaPos = deltaPos.normalized * hit.distance;
            }
            if(teleportVFX != null)
            {
                teleportVFX.GetComponent<ParticleSystem>().Play();
                Instantiate(teleportFogVFX, transform.position, Quaternion.identity);
            }
        }

        transform.Translate(deltaPos); // �յ� �̵�.
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        myAnim.SetFloat("SpeedX", Mathf.Abs(x));

    }

    void Rotate()
    {

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // ���������� ȸ��, +
        {
            curRotY = 0.0f; // ��ٷ� ���� ��ȯ
            switchTrackedOffset?.Invoke(1); // 
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // �������� ȸ��, -
        {
            curRotY = 180.0f; // ��ٷ� ���� ��ȯ
            switchTrackedOffset?.Invoke(-1);
        }
        curRotY = Mathf.Clamp(curRotY, rotYRange.x, rotYRange.y); // rotYRange�ȿ� ������ ���ѵ�
        transform.localRotation = Quaternion.Euler(0, curRotY, 0);

    }

    void IsGround()
    {

        isGround = Physics.Raycast(transform.position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, 1.1f + Mathf.Epsilon, groundMask);
        //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.down, Color.blue);

        myAnim.SetBool("IsGround", isGround);
        if (isGround)
        {
            jumpCoolTime += Time.deltaTime;
            Debug.Log("hit");
            myAnim.ResetTrigger("Jumping");
        }
    }

    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("���� Ʈ���� ��");
            if (isGround && jumpCoolTime >= 0.15f)
            {
                Debug.Log("����");
                // ���� ���� Loop, Play on Awake ���ߵ�
                if (myAudioSource != null)
                {
                    myAudioSource.clip = jumpClip;
                    myAudioSource.PlayOneShot(jumpClip);
                }
                jumpCoolTime = 0.0f;
                Jump();
                myAnim.SetTrigger("Jumping");
                if (myAudioSource.isPlaying) Debug.Log("���� ȿ����");
            }
        }
    }

    void Jump()
    {

        Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);
        rigid.AddForce(jumpVelocity, ForceMode.Impulse);
    }
    #endregion

    #region MoveOn3D
    void Move3D()
    {

        Constraints3D();
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");


        Vector3 deltaXPos = Vector3.forward * x * Time.deltaTime * battleStat.MoveSpeed;
        Vector3 deltaYPos = Vector3.right * y * Time.deltaTime * battleStat.MoveSpeed;

        if ((!Mathf.Approximately(x, 0.0f) || !Mathf.Approximately(y, 0.0f)) &&
            Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Approximately(teleportDeltaTime, 0.0f)) // �ڷ���Ʈ, ���� ��Ÿ�� �߰� ����
        {
            if (teleportDelay != null)
            {
                StopCoroutine(teleportDelay);
                teleportDelay = null;
            }
            teleportDelay = StartCoroutine(CoolingTelePort());
            deltaXPos += deltaXPos.normalized * 1.5f;
            deltaYPos += deltaYPos.normalized * 1.5f;
            if (Physics.Raycast(new Ray(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward), out RaycastHit hit,
                1.5f, groundMask))
            {
                Debug.Log("���� ����");
                deltaXPos = deltaXPos.normalized * hit.distance;
                deltaYPos = deltaYPos.normalized * hit.distance;
            }
            if (teleportVFX != null)
            {
                teleportVFX.GetComponent<ParticleSystem>().Play();
                Instantiate(teleportFogVFX, transform.position, Quaternion.identity);
            }
        }

        transform.Translate(deltaXPos + deltaYPos, Space.World);
        myAnim.SetFloat("SpeedX", Mathf.Abs(x));
        myAnim.SetFloat("SpeedY", Mathf.Abs(y));
        Rotate3D(x, y);

    }

    void Rotate3D(float x, float y)
    {
        // ���� �������� ȸ���Ѵ�.

        if (!Mathf.Approximately(x, 0.0f) || !Mathf.Approximately(y, 0.0f))
        {
            Vector3 lookDir = new Vector3(y, 0, x);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * rotSpeed);
        }

    }

    IEnumerator Rotating(Vector3 dir)
    {
        float angle = Vector3.Angle(transform.forward, dir);
        float rotDir = 1.0f;
        if (Vector3.Dot(transform.right, dir) < 0.0f)
        {
            rotDir = -1.0f;
        }

        while (!Mathf.Approximately(angle, 0.0f))
        {
            float delta = rotSpeed * Time.deltaTime;
            if (delta > angle)
            {
                delta = angle;
            }
            angle -= delta;
            transform.Rotate(Vector3.up * rotDir * delta);
            yield return null;
        }
    }
    #endregion

    public void ControllPlayerAttack(bool _isPossible)
    {
        canAttack = _isPossible;
    }

    public bool CanAttack() 
    { 
        return canAttack; 
    }

    // �� �Ʒ����� ���߿� ��ũ��Ʈ �и��� ���� ����
    protected void Attack() // ���� �Լ�, ������ ��Ȯ�� �ٶ󺼶��� ���� ����
    {
        if (Mathf.Approximately(attackDeltaTime, 0.0f)) // ��Ÿ�� ���, ������ �ȵ�µ� �ϴ� �ļ���
        {
            if (attackDelay != null)
            {
                StopCoroutine(attackDelay);
            }
            //attackDelay = StartCoroutine(CoolingTime(battleStat.AttackDelay, attackDeltaTime));
            attackDelay = StartCoroutine(CoolingAttack());
        }
        else
        {
            return;
        }
        if (controll2D)
        {
            float angle_F = Vector3.Angle(transform.forward, Vector3.forward); // -> ������ ���� 0, �ݴ�� 180
            if (Mathf.Approximately(angle_F, 0.0f) || Mathf.Approximately(angle_F, 180.0f))
                myAnim.SetTrigger("Attack");
        }
        else
        {
            myAnim.SetTrigger("Attack");
        }
    }

    public new void OnAttack()
    {
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        // �ִϸ��̼� �̺�Ʈ
        // ���̾(?)�� �����Ǿ� ������ �߻�Ǵ� �Լ�
        // �Ҳ� �߻� ���� Loop, Play on Awake ���ߵ�
        if (myAudioSource != null)
        {
            myAudioSource.clip = fireballClip;
            myAudioSource.PlayOneShot(fireballClip);
            if (myAudioSource.isPlaying) Debug.Log("���̾ ȿ����");
        }
        GameObject obj = Instantiate(orgFireball, rightAttackPoint);
        obj.transform.SetParent(null);
        obj.GetComponent<Fireball>().SetFireBallAP(GetAp()); // ���̾ ���ݷ� ����
        obj.GetComponent<Fireball>().SetAttackRange(GetAttackRange()); // ���̾ ���� ��Ÿ� ����
        obj.GetComponent<Fireball>().SetProjectileSpeed(GetProjectileSpeed()); // ���̾ ����ü �ӵ� ����
        obj.GetComponent<Fireball>().SetFireBallScale(GetAttackSize()); // ���̾ ũ�� ����
        obj.GetComponent<Fireball>().SetFireCanConsume(GetHealAfterAttack()); // ���̾ ���� ���� ����
        yield return new WaitForSeconds(0.25f);
        if (GetAttackTwice())
        {
            GameObject obj2 = Instantiate(orgFireball, rightAttackPoint);
            obj2.transform.SetParent(null);
            obj2.GetComponent<Fireball>().SetFireBallAP(GetAp()); // ���̾ ���ݷ� ����
            obj2.GetComponent<Fireball>().SetAttackRange(GetAttackRange()); // ���̾ ���� ��Ÿ� ����
            obj2.GetComponent<Fireball>().SetProjectileSpeed(GetProjectileSpeed()); // ���̾ ����ü �ӵ� ����
            obj2.GetComponent<Fireball>().SetFireBallScale(GetAttackSize()); // ���̾ ũ�� ����
            obj2.GetComponent<Fireball>().SetFireCanConsume(GetHealAfterAttack()); // ���̾ ���� ���� ����
        }

    }

    // �Ʒ��� �ߺ��ڵ�, ���� �ʿ���
    IEnumerator CoolingAttack()
    {

       // while (!Mathf.Approximately(battleStat.AttackDelay, attackDeltaTime))
        while (battleStat.AttackDelay >= attackDeltaTime)
        {
            attackDeltaTime += Time.deltaTime;
            fireballCoolTimeImg.fillAmount = attackDeltaTime / (battleStat.AttackDelay );
            yield return null;
        }
        attackDeltaTime = 0f;
    }

    IEnumerator CoolingTelePort()
    {

        while (teleportDeltaTime <= teleportCoolTime)
        {
            teleportDeltaTime += Time.deltaTime;
            teleportCoolTimeImg.fillAmount = teleportDeltaTime / teleportCoolTime;
            yield return null;
        }
        teleportDeltaTime = 0f;
    }

    public void SetSpell(ItemStat _itemStat)
    {
        this.spellObject = _itemStat.SpellObject;
    }
    public void ReadyToUseSpell(bool isReady)
    {
        isSpellReady = isReady;
        myAnim.SetBool("IsSpellReady", isReady);
    }

    public void UsingSpell(Vector3 spellPoint) // ���⼭ ������ ����Ѵ�.
    {
        if (spellObject != null)
        {
            //���� ��� ���� Loop, Play on Awake ���ߵ�
            myAnim.SetTrigger("UseSpell");
            if (spellObject.gameObject.tag == "AttackSpell")
            {
                if (controll2D)
                {
                    Instantiate(spellObject, new Vector3(0.0f, spellPoint.y + 0.1f, spellPoint.z), Quaternion.identity);
                }
                else
                {
                    Instantiate(spellObject, new Vector3(spellPoint.x, spellPoint.y + 0.1f, spellPoint.z), Quaternion.identity);
                }
            }
            else Instantiate(spellObject, this.transform);
            spellObject = null;

            ItemStat tmpStat = spellData.GetSpellDataInfo();
            invenUpdate?.Invoke(tmpStat);
        }

    }

    public void ResetSpellTrigger()
    {
        myAnim.ResetTrigger("UseSpell");
    }

    public Transform GetCurrentSpell()
    {
        return this.spellObject;
    }

    public void HealBuff()
    {
        this.curHP += this.battleStat.MaxHp * 0.5f;
        if (this.curHP >= this.battleStat.MaxHp)
        {
            this.curHP = this.battleStat.MaxHp;
        }
        PlayerUIwindows.Instance.UpdateHpbar();
        Debug.Log("ġ�� �ֹ�");
        //�� ����, Loop Play on Awake ���ߵ�
        if (myAudioSource != null)
        {
            myAudioSource.clip = healbuffClip;
            myAudioSource.PlayOneShot(healbuffClip);
        }
    }

    public void HealWithFullHealth()
    {
        this.curHP = this.battleStat.MaxHp;
        PlayerUIwindows.Instance.UpdateHpbar();
        if (myAudioSource != null)
        {
            myAudioSource.clip = healbuffClip;
            myAudioSource.PlayOneShot(healbuffClip);
        }
    }


    public void HealWithConsume()
    {
        this.curHP++;
        if (this.curHP >= this.battleStat.MaxHp)
        {
            this.curHP = this.battleStat.MaxHp;
        }
        PlayerUIwindows.Instance.UpdateHpbar();
    }

    public void SpeedBuff()
    {
        StartCoroutine(SpeedBuffActing());
        if (myAudioSource != null)
        {
            myAudioSource.clip = speedbuffClip;
            myAudioSource.PlayOneShot(speedbuffClip);
        }
        if (myAudioSource.isPlaying) Debug.Log("���ǵ� ���� ȿ����");
        // ���ǵ� ���� ���� Loop Play on Awake ���ߵ�
    }
    IEnumerator SpeedBuffActing()
    {
        float originSpeed = this.battleStat.MoveSpeed;
        this.battleStat.MoveSpeed += this.battleStat.MoveSpeed * 0.5f;
        yield return new WaitForSeconds(5f);
        this.battleStat.MoveSpeed = originSpeed;
    }
    public void UpdatePlayerStat(BattleStat _itemStat) // ���⼭ _itemStat�� �κ��丮���� �ڱ� �ڽĵ��� stat�� ���� ������ �־����
    {
        // ��ó�� ���� ���� �κ��丮�� ����ִ� �������� �ٲ� ��쿡�� �κ��丮 �ȿ� �ִ� �����۵��� ���� ������ ���ؼ� �Ʒ��� 5��¥�� �ڵ带 ������
        this.battleStat.AP = this.originalStat.AP + _itemStat.AP;
        this.battleStat.MaxHp = this.originalStat.MaxHp + _itemStat.MaxHp;
        this.battleStat.MoveSpeed = this.originalStat.MoveSpeed + _itemStat.MoveSpeed;
        this.battleStat.AttackRange = this.originalStat.AttackRange + _itemStat.AttackRange;
        this.battleStat.ProjectileSpeed = this.originalStat.ProjectileSpeed + _itemStat.ProjectileSpeed;
        this.battleStat.AttackDelay = this.originalStat.AttackDelay - _itemStat.AttackDelay;
        this.battleStat.AttackSize = this.originalStat.AttackSize + _itemStat.AttackSize;

        this.battleStat.AttackTwice = _itemStat.AttackTwice;
        this.battleStat.HealAfterAttack = _itemStat.HealAfterAttack;
        this.battleStat.ResurrectionOneTime = _itemStat.ResurrectionOneTime;
        this.battleStat.HitOnlyHalf = _itemStat.HitOnlyHalf;
        this.battleStat.CA_AttackPenalty = _itemStat.CA_AttackPenalty;
        this.battleStat.CA_GoldPenalty = _itemStat.CA_GoldPenalty;
        this.battleStat.CA_HPPenalty = _itemStat.CA_HPPenalty;

        if (this.battleStat.CA_AttackPenalty)
        {
            this.battleStat.AP += 2;
        }
        if (this.battleStat.CA_HPPenalty)
        {
            this.battleStat.AP += 1;
            this.battleStat.MaxHp += 20;
        }

        
        //Initialize();
       PlayerUIwindows.Instance.UpdateHpbar();
        Debug.Log(this.battleStat.AP + " ���ݷ� ��ȭ �Ͼ");
        
        //NotifyStatsChanged();
    }

    public bool GetControllType()
    {
        return this.controll2D;
    }

    public bool GetMovePossible()
    {
        return this.canMove;
    }

    public void MoveTrue()
    {
        this.canMove = true;
    }

    public void MoveFalse()
    {
        this.canMove = false;
    }
    public override void TakeDamage(float _dmg)
    {
        if (GetCA_GoldPenalty()) goldManager.ChangeGold(0);
        if (GetHitOnlyHalf())
        {
            //curHP -= _dmg*0.5f;
            _dmg *= 0.5f;
        }

        if (GetCA_HpPenalty())
        {
            curHP -= _dmg * 2f;
        }
        else
        {
            curHP -= _dmg;
        }
        PlayerUIwindows.Instance.UpdateHpbar();

        if (curHP <= 0.0f)
        {
            if (GetResurrectionOneTime())
            {
                curHP = 3.0f;
                PlayerUIwindows.Instance.UpdateHpbar();
                this.battleStat.ResurrectionOneTime = false;
            }
            else
            {
                // ü���� �� �� ������
                OnDead();
                myAnim.SetTrigger("Dead");
                if (myAudioSource != null)
                {
                    myAudioSource.clip = deadClip;
                    myAudioSource.PlayOneShot(deadClip);
                }
                if (myAudioSource.isPlaying) Debug.Log("�����ֱ�");
            }
        }
        else
        {
            myAnim.SetTrigger("Damage");
            if (myAudioSource != null)
            {
                myAudioSource.clip = hitClip;
                myAudioSource.PlayOneShot(hitClip);
                
            }
            if (myAudioSource.isPlaying) Debug.Log("�ƾ�");
        }
        Debug.Log($"�÷��̾� ����, ���� ü�� {this.curHP}");
    }
    protected override void OnDead()
    {
        //rigid.velocity = Vector3.zero;
        base.OnDead();
        StartCoroutine(ChangeAlpha());
    }

    public IEnumerator ChangeAlpha()
    {
        yield return new WaitForSeconds(2.0f);
        DeathIMG.GetComponent<CanvasGroup>().alpha = 1.0f;
        DeathIMG.GetComponent<CanvasGroup>().blocksRaycasts = true;

    }


    // ���� ���� ����
    void ShowStat()
    {
        Debug.Log($"���ݷ� : {this.battleStat.AP}");
        Debug.Log($"���� ü�� : {this.curHP}");
        Debug.Log($"�ִ� ü�� : {this.battleStat.MaxHp}");
        Debug.Log($"���� ��Ÿ� : {this.battleStat.AttackRange}");
        Debug.Log($"���� �ӵ� : {this.battleStat.AttackDelay}");
        Debug.Log($"����ü �ӵ� : {this.battleStat.ProjectileSpeed}");
        Debug.Log($"�̵��ӵ� : {this.battleStat.MoveSpeed}");
    }

    
}
