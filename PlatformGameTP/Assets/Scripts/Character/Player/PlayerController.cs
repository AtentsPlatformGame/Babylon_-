using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : BattleSystem
{

    [SerializeField] float moveSpeed = 4.0f;
    [SerializeField] float rotSpeed = 1.0f;
    [SerializeField] float jumpForce = 2.0f;
    [SerializeField] float jumpCharge = 1.0f;
    [SerializeField] Vector2 rotYRange = new Vector2(0.0f, 180.0f);
    public GameObject orgFireball;
    public LayerMask groundMask;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;
    public UnityEvent<int> switchTrackedOffset;
    public bool isSpellReady = false;
    float curRotY;
    int ap;
    bool isGround;
    float attackDeltaTime = 0.0f;
    Rigidbody rigid;
    Fireball fireBall;
    Coroutine attackDelay;
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
            IsGround();
            TryJump();
            Rotate();
            Move(); // ȸ���� ���ÿ� �����̱�
                    //if (canMove) Move(); // ȸ���� ������ �����̱�
        }
    }


    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        Vector3 deltaPos = transform.forward * x * Time.deltaTime * moveSpeed;

        if (!Mathf.Approximately(x,0.0f) && Input.GetKeyDown(KeyCode.LeftShift)) // �ڷ���Ʈ, ���� ��Ÿ�� �߰� ����
        {
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
        myAnim.SetFloat("Speed", Mathf.Abs(x));

    }

    void Rotate()
    {
        /*
        1. ȸ���� ���ÿ� �����̰� �ϱ� : ȸ���ϴ°� �����ִ� ���� ������� �Ʒ� �� ���ǹ� �ݱ�
                                  -> Update���� ������ Move���
        2. ȸ�� �Ⱥ����ֱ�. : ��ٷ� ���� ��ȯ ���� ������� �Ʒ� �� ���ǹ� �ݰ� Update���� ������ Move���
        3. ȸ�� �� �̵��ϱ� : ȸ���ϴ°� ������ < ���� ������� ���ǹ� ���� Update���� ������ Move���
        */
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // ���������� ȸ��, +
        {
            //curRotY -= Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime; // ȸ���ϴ°� ������
            curRotY = 0.0f; // ��ٷ� ���� ��ȯ
            switchTrackedOffset?.Invoke(1); // 
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // �������� ȸ��, -
        {
            //curRotY += -1 * Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime; // ȸ���ϴ°� ������
            curRotY = 180.0f; // ��ٷ� ���� ��ȯ
            switchTrackedOffset?.Invoke(-1);
        }
        curRotY = Mathf.Clamp(curRotY, rotYRange.x, rotYRange.y); // rotYRange�ȿ� ������ ���ѵ�
        transform.localRotation = Quaternion.Euler(0, curRotY, 0);

        /*
        // �Ʒ� ������ ���� �������� ȸ���� �������� �����ϼ� �ֵ��� ���� ���ǹ�
        if (Mathf.Approximately(curRotY, rotYRange.x) || Mathf.Approximately(curRotY, rotYRange.y))
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
        */
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
        /*if (jumpCharge >= 2.0f) jumpCharge = 2.0f;
        rigid.AddForce(transform.up * jumpForce * jumpCharge, ForceMode.Impulse);
        jumpCharge = 1.0f;*/
        rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // �� �Ʒ����� ���߿� ��ũ��Ʈ �и��� ���� ����
    protected void Attack() // ���� �Լ�, ������ ��Ȯ�� �ٶ󺼶��� ���� ����
    {
        if(Mathf.Approximately(attackDeltaTime, 0.0f)) // ��Ÿ�� ���, ������ �ȵ�µ� �ϴ� �ļ���
        {
            if(attackDelay != null)
            {
                StopCoroutine(attackDelay);
            }
            attackDelay = StartCoroutine(CoolingAttack());
        }
        else
        {
            return;
        }

        float angle_F = Vector3.Angle(transform.forward, Vector3.forward); // -> ������ ���� 0, �ݴ�� 180
        float angle_B = Vector3.Angle(transform.forward, Vector3.back); // -> ������ ���� 0, �ݴ�� 180
        Debug.Log(angle_F);
        Debug.Log(angle_B);
        if (Mathf.Approximately(angle_F, 0.0f) || Mathf.Approximately(angle_F, 180.0f))
            myAnim.SetTrigger("Attack");
        /*
        Debug.Log(Camera.main.ScreenPointToRay(Input.mousePosition));*/
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

    IEnumerator CoolingAttack()
    {
        
        while(!Mathf.Approximately(battleStat.AttackDelay, attackDeltaTime))
        {
            attackDeltaTime += 1f;
            yield return new WaitForSeconds(1f);
        }
        attackDeltaTime = 0f;
    }

    public void ReadyToUseSpell(bool isReady)
    {
        isSpellReady = isReady;
        myAnim.SetBool("IsSpellReady", isReady);
    }

    public void UsingSpell()
    {
        myAnim.SetTrigger("UseSpell");
    }

    public void ResetSpellTrigger()
    {
        myAnim.ResetTrigger("UseSpell");
    }
}
