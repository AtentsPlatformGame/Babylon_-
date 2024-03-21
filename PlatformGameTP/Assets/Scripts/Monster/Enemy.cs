using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Enemy : BattleSystem
{
    public enum State
    {
        Create, Normal, Roaming, Battle, Death
    }
    public Transform player;
    public float detectionRange = 4f; //���Ͱ� �÷��̾ �����ϴ� ����
    public float moveSpeed = 3f; // ���� ���ǵ�
    public float attackCooldown = 5f; // ���� ���� ������
    public float attackRange = 4f;// ������ ���� ����
    public float returnSpeed = 2f; // ���Ͱ� ���ڸ��� �����ϴ� �ӵ�
    public float deathDelay = 2f; // ���Ͱ� �׾ ������� �ð�
    public float MonsterHP = 5f; //���� ü��
    public float rotationSpeed = 360f;
    public Animator myanim;
    public LayerMask groundLayer;
   
   

    [SerializeField] float curtHP;
    private bool isChasing = false;
    private bool isAttacking = false;
    private Vector3 startPosition;
    private float lastAttackTime = 0f;
    [SerializeField]private bool isDead = false; //���Ͱ� �׾����� ���θ� ��Ÿ���� ����

    private void Start()
    {
        startPosition = transform.position;
        curtHP = MonsterHP;
        Initialize();
    }
    
    private void Update()
    {
        if (isDead || player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if ( distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            myanim.SetBool("Ismoving", true);
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            if (distanceToPlayer < 1.0f)
            {
                Battle();
                if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
                {
                    // �÷��̾ ���� ���� ���� �ְ�, ���� ��ٿ��� ���� ���
                    Battle();

                }
            }
            
        }
        else

        {
            // �÷��̾ ������ ������ ����� ���Ͱ� ���� ��ġ�� ����
            Vector3 directionToStart = (startPosition - transform.position).normalized;

            // ���Ͱ� ���� ��ġ�� �ٶ󺸵��� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(directionToStart);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // ȸ���� �Ϸ�Ǹ� ���ڸ��� �̵�
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(transform.position, startPosition) < 0.1f)
            {
                isChasing = false;
                myanim.SetBool("Ismoving", false);
            }

        }
    }
    private void Battle()
    {
        if (curHp > 0)
        {
            isAttacking = true;
            myanim.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
        else
        {
            // ���Ͱ� �̹� �׾����� ������ ����
            isDead = true;
            isChasing = false;
        }

    }
    public new void TakeDamage(float dmg)
    {
        
        base.TakeDamage(dmg);
        if(curHp <= 0 && !isDead) // ����
        {
            // ������ �������.
            Debug.Log("���� ����");
            Die();
        }
    }
    public void Die()
    {
        myanim.SetTrigger("Die");
        //����������� ������ ����
        isDead = true;
        Destroy(gameObject, deathDelay);
        
    }
    public void DisApear()
    {
        StartCoroutine(DisApearing(2.0f));
    }
    IEnumerator DisApearing(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);

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
    private void FixedUpdate()
    {
        // ���Ͱ� ���� ���� �ִ��� Ȯ��
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.distance < 0.1f)
            {
                // �� ���� �ʹ� ������ ��ġ ���� �ʿ�
                transform.position -= Vector3.forward * Time.deltaTime;
            }
        }
    }
}
