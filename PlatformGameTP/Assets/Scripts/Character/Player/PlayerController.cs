using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : BattleSystem
{

    [SerializeField][Header("�÷��̾� �̵� �ӵ�")] float moveSpeed = 4.0f;
    [SerializeField][Header("�÷��̾� ȸ�� �ӵ�")] float rotSpeed = 1.0f;
    [SerializeField][Header("�÷��̾� ���� ����")] float jumpForce = 2.0f;
    [SerializeField][Header("�÷��̾� ���� ���")] Transform[] spellObject; // ���� ��¥�� 1�� ��� �ٴϴϱ� �̰� �迭�� �ƴ϶� �׳� �Ѱ��� �����ؾ���
    [SerializeField]
    [Header("�÷��̾� 2D �̵� ��� ���")] bool controll2D = true;
    [SerializeField] Vector2 rotYRange = new Vector2(0.0f, 180.0f);
    

    public GameObject orgFireball;
    public LayerMask groundMask;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;
    public UnityEvent<int> switchTrackedOffset;
    public bool isSpellReady = false;

    float curRotY;
    float ap;
    bool isGround;
    float attackDeltaTime = 0.0f;
    float teleportDeltaTime = 0.0f;

    Rigidbody rigid;
    Fireball fireBall;
    Coroutine attackDelay;
    Coroutine teleportDelay;
    Coroutine rotating;
    // Start is called before the first frame update
    private void Awake()
    {
        Initialize();
    }
    void Start()
    {
        curRotY = transform.localRotation.eulerAngles.y;
        rigid = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive())
        {
            if (controll2D) // �յ� 2�������� �����̰� �����ϴ� �ڵ�
            {
                IsGround();
                TryJump();
                Rotate();
                Move(); // ȸ���� ���ÿ� �����̱�
                        //if (canMove) Move(); // ȸ���� ������ �����̱�
            }
            else // �յ�, �翷 4�������� �����̴� �ڵ�, ������ �ȸ���
            {
                Rotate3D();
                Move3D();
            }
        }
    }
    #region ControllChange
    public void SwitchControllType2D(bool _type)
    {
        controll2D = _type;
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
        Vector3 deltaPos = transform.forward * x * Time.deltaTime * moveSpeed;

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
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.down, Color.blue);

        myAnim.SetBool("IsGround", isGround);
        if (isGround)
        {
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
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            myAnim.SetTrigger("Jumping");
        }
    }

    void Jump()
    {

        rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    #endregion

    #region MoveOn3D
    void Move3D()
    {
        Constraints3D();
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");
        Vector3 deltaXPos = Vector3.forward * x * Time.deltaTime * moveSpeed;
        Vector3 deltaYPos = Vector3.right * y * Time.deltaTime * moveSpeed;

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
        }

        transform.Translate(deltaXPos + deltaYPos, Space.World); // �յ� �̵�.
        myAnim.SetFloat("SpeedX", Mathf.Abs(x));
        myAnim.SetFloat("SpeedY", Mathf.Abs(y));
    }

    void Rotate3D()
    { // ���� �������� ȸ���Ѵ�.
        /*
        #region �밡�� ������ 8���� ȸ��
        Vector3 dir = Vector3.zero;
        // �Ʒ� �ӽ� ȸ�� �ڵ�, ���߿� �ϴ� ���� ��������
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // ���� �� ���� ����
        {
            dir = Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))// ���� �� �ڸ� ����
        {
            dir = Vector3.back;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // ���� �� �������� ����
        {
            dir = Vector3.right;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // ���� �� ������ ����
        {
            dir = Vector3.left;
        }

        if((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) ||
            (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))) // ����� ���� ���� ����
        {
            dir = Vector3.forward + Vector3.left;
        }

        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))) // ����� ���� ���� ����
        {
            dir = Vector3.forward + Vector3.right;
        }

        if ((Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) ||
            (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))) // ����� ���� �ڸ� ����
        {
            dir = Vector3.back + Vector3.left;
        }

        if ((Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))) // ����� ���� �ڸ� ����
        {
            dir = Vector3.back + Vector3.right;
        }



        if (rotating != null)
        {
            StopCoroutine(rotating);
            rotating = null;
        }
        else
        {
            rotating = StartCoroutine(Rotating(dir));
        }
        #endregion
        */
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");

        Vector3 rotDir = new Vector3(y, 0, x);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotSpeed);
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
        ap = GetAp();
        // �ִϸ��̼� �̺�Ʈ
        // ���̾(?)�� �����Ǿ� ������ �߻�Ǵ� �Լ�
        GameObject obj = Instantiate(orgFireball, rightAttackPoint);
        obj.transform.SetParent(null);
        obj.GetComponent<Fireball>().SetFireBallAP(ap); // ���̾ ���ݷ� ����
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


    public void ReadyToUseSpell(bool isReady)
    {
        isSpellReady = isReady;
        myAnim.SetBool("IsSpellReady", isReady);
    }

    public void UsingSpell(Vector3 spellPoint) // ���⼭ ������ ����Ѵ�.
    {
        if (spellObject[0] != null)
        {
            myAnim.SetTrigger("UseSpell");
            if (spellObject[0].gameObject.tag == "AttackSpell")Instantiate(spellObject[0], new Vector3(0, spellPoint.y + 0.1f, spellPoint.z), Quaternion.identity);
            else Instantiate(spellObject[0],this.transform);
            spellObject[0] = null;

            if (spellObject[1] != null)
            {
                spellObject[0] = spellObject[1];
                spellObject[1] = null;
            }
            
        }
        
    }

    public void ResetSpellTrigger()
    {
        myAnim.ResetTrigger("UseSpell");
    }

    public Transform GetCurrentSpell()
    {
        return this.spellObject[0];
    }

    public void HealBuff()
    {

    }

    public void SpeedBuff()
    {

    }
    public void UpdatePlayerStat(ItemStat _itemStat)
    {
        if (_itemStat.ItemType == ITEMTYPE.NONE) return;
        switch (_itemStat.ItemType)
        {
            case ITEMTYPE.WEAPON:
                if (_itemStat.Ap != 0) this.battleStat.AP = _itemStat.Ap; // ���ݷ� ����
                break;
            case ITEMTYPE.ARMOR:
                if(!Mathf.Approximately(_itemStat.PlusHeart,0.0f)) this.battleStat.MaxHp = _itemStat.PlusHeart; // �ִ� ü�� ����
                break;
            case ITEMTYPE.ACCE:
                if (!Mathf.Approximately(_itemStat.PlusSpeed, 0.0f)) this.battleStat.MoveSpeed = _itemStat.PlusSpeed; // �̼� ����
                break;
            case ITEMTYPE.PASSIVE:
                break;
            case ITEMTYPE.CURSEDACCE:
                break;
            case ITEMTYPE.SPELL:
                break;
            default:
                break;
        }
        Initialize();
    }

}
