using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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

    public GameObject orgFireball;
    public LayerMask groundMask;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;
    public UnityEvent<int> switchTrackedOffset;
    public GameObject DeathIMG;


    public bool isSpellReady = false;
    public bool is3d = true;
    public float jumpCoolTime = 0.5f;

    float curRotY;
    float ap;
    bool isGround;
    float attackDeltaTime = 0.0f;
    float teleportDeltaTime = 0.0f;

    
    BattleStat originalStat;
    Rigidbody rigid;
    Fireball fireBall;
    Coroutine attackDelay;
    Coroutine teleportDelay;
    Coroutine rotating;



    // Start is called before the first frame update
    private void Awake()
    {
        OriginalStatInit(playerStatData.GetPlayerStatInfo());
        Initialize();
    }
    void Start()
    {
        curRotY = transform.localRotation.eulerAngles.y;
        rigid = this.GetComponent<Rigidbody>();
        DeathIMG.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            ShowStat();
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

    void OriginalStatInit(PlayerBattleStat pb)
    {
        this.originalStat.AP = pb.AP;
        this.originalStat.MaxHp = pb.MaxHp;
        this.originalStat.AttackRange = pb.AttackRange;
        this.originalStat.AttackDelay = pb.AttackDelay;
        this.originalStat.ProjectileSpeed = pb.ProjectileSpeed;
        this.originalStat.MoveSpeed = pb.MoveSpeed;
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

        isGround = Physics.Raycast(transform.position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, 1.1f, groundMask);
        //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.down, Color.blue);

        myAnim.SetBool("IsGround", isGround);
        if (isGround)
        {
            jumpCoolTime += Time.deltaTime;
            Debug.Log("hit");
        }
    }

    void TryJump()
    {
        /*if (isGround && Input.GetKey(KeyCode.Space))
        {
          jumpCharge += Time.deltaTime;
        }

        if (isGround && Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
            myAnim.SetTrigger("Jumping");
        }
        UnityEngine.Debug.Log(isGround);*/
        if (isGround && Input.GetKeyDown(KeyCode.Space) && jumpCoolTime >= 0.25f)
        {
            jumpCoolTime = 0.0f;
            Jump();
            myAnim.SetTrigger("Jumping");
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
        // �ִϸ��̼� �̺�Ʈ
        // ���̾(?)�� �����Ǿ� ������ �߻�Ǵ� �Լ�
        GameObject obj = Instantiate(orgFireball, rightAttackPoint);
        obj.transform.SetParent(null);
        obj.GetComponent<Fireball>().SetFireBallAP(GetAp()); // ���̾ ���ݷ� ����
        obj.GetComponent<Fireball>().SetAttackRange(GetAttackRange()); // ���̾ ���� ��Ÿ� ����
        obj.GetComponent<Fireball>().SetProjectileSpeed(GetProjectileSpeed()); // ���̾ ����ü �ӵ� ����
    }

    // �Ʒ��� �ߺ��ڵ�, ���� �ʿ���
    IEnumerator CoolingAttack()
    {

        while (!Mathf.Approximately(battleStat.AttackDelay, attackDeltaTime))
        {
            attackDeltaTime += 1f;
            yield return new WaitForSeconds(1f);
        }
        attackDeltaTime = 0f;
    }

    IEnumerator CoolingTelePort()
    {

        while (!Mathf.Approximately(3.0f, teleportDeltaTime))
        {
            teleportDeltaTime += 1f;
            yield return new WaitForSeconds(1f);
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
        Debug.Log("�� ���� ���");
    }

    public void SpeedBuff()
    {
        StartCoroutine(SpeedBuffActing());
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
        /*BattleStat tmpBattleStat = new BattleStat();
        if (_itemStat.ItemType == ITEMTYPE.NONE) return;
        switch (_itemStat.ItemType)
        {
            case ITEMTYPE.WEAPON:
                if (_itemStat.Ap != 0) tmpBattleStat.AP += _itemStat.Ap; // ���ݷ� ����
                break;
            case ITEMTYPE.ARMOR:
                if (!Mathf.Approximately(_itemStat.PlusHeart, 0.0f)) tmpBattleStat.MaxHp += _itemStat.PlusHeart; // �ִ� ü�� ����
                break;
            case ITEMTYPE.SPELL:
                if (_itemStat.SpellObject != null) this.spellObject = _itemStat.SpellObject; // ���� ����
                break;
            case ITEMTYPE.PASSIVE:
                if (!Mathf.Approximately(_itemStat.PlusSpeed, 0.0f)) tmpBattleStat.MoveSpeed += _itemStat.PlusSpeed; // �̵��ӵ� ����
                if (!Mathf.Approximately(_itemStat.PlusAttackRange, 0.0f)) tmpBattleStat.AttackRange += _itemStat.PlusAttackRange; // �����Ÿ� ����
                if (!Mathf.Approximately(_itemStat.PlusProjectileSpeed, 0.0f)) tmpBattleStat.ProjectileSpeed += _itemStat.PlusProjectileSpeed; // ����ü �ӵ� ����
                break;
            case ITEMTYPE.CURSEDACCE:
                if (_itemStat.Ap != 0) tmpBattleStat.AP += _itemStat.Ap; // ���ݷ� ����
                if (!Mathf.Approximately(_itemStat.PlusHeart, 0.0f)) tmpBattleStat.MaxHp += _itemStat.PlusHeart; // �ִ� ü�� ����
                if (!Mathf.Approximately(_itemStat.PlusSpeed, 0.0f)) tmpBattleStat.MoveSpeed += _itemStat.PlusSpeed; // �̵��ӵ� ����
                if (!Mathf.Approximately(_itemStat.PlusAttackRange, 0.0f)) tmpBattleStat.AttackRange += _itemStat.PlusAttackRange; // �����Ÿ� ����
                if (!Mathf.Approximately(_itemStat.PlusProjectileSpeed, 0.0f)) tmpBattleStat.ProjectileSpeed += _itemStat.PlusProjectileSpeed; // ����ü �ӵ� ����
                break;
            default:
                break;
        }
        this.battleStat.AP = this.originalStat.AP + tmpBattleStat.AP;
        this.battleStat.MaxHp = this.originalStat.MaxHp + tmpBattleStat.MaxHp;
        this.battleStat.MoveSpeed = this.originalStat.MoveSpeed + tmpBattleStat.MoveSpeed;
        this.battleStat.AttackRange = this.originalStat.AttackRange + tmpBattleStat.AttackRange;
        this.battleStat.ProjectileSpeed = this.originalStat.ProjectileSpeed + tmpBattleStat.ProjectileSpeed;*/

        // ��ó�� ���� ���� �κ��丮�� ����ִ� �������� �ٲ� ��쿡�� �κ��丮 �ȿ� �ִ� �����۵��� ���� ������ ���ؼ� �Ʒ��� 5��¥�� �ڵ带 ������
        this.battleStat.AP = this.originalStat.AP + _itemStat.AP;
        this.battleStat.MaxHp = this.originalStat.MaxHp + _itemStat.MaxHp;
        this.battleStat.MoveSpeed = this.originalStat.MoveSpeed + _itemStat.MoveSpeed;
        this.battleStat.AttackRange = this.originalStat.AttackRange + _itemStat.AttackRange;
        this.battleStat.ProjectileSpeed = this.originalStat.ProjectileSpeed + _itemStat.ProjectileSpeed;
        

        //Initialize();
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

    protected override void OnDead()
    {
        

        base.OnDead();

        StartCoroutine(ChangeAlpha());
  
    }

    public IEnumerator ChangeAlpha()
    {
        yield return new WaitForSeconds(2.0f);
        DeathIMG.GetComponent<CanvasGroup>().alpha = 1.0f;
        
       
             
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
