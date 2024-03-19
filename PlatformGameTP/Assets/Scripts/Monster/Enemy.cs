using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : BattleSystem
{
    public enum State
    {
        Create, Normal, Battle, Death
    }

    public Transform player;
    public float detectionRange = 4f; //���Ͱ� �÷��̾ �����ϴ� ����
    public float moveSpeed = 1f; // ���� ���ǵ�
    public float attackCooldown = 5f; // ���� ���� ������
    public float attackRange = 2f;// ������ ���� ����
    public float returnSpeed = 2f; // ���Ͱ� ���ڸ��� �����ϴ� �ӵ�
    public float deathDelay = 2f; // ���Ͱ� �׾ ������� �ð�
    public float MonsterHP = 5f; //���� ü��
    public Animator myanim;
    public LayerMask groundLayer;

    private float curtHP;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isBattle = false;
    private Vector3 startPosition;
    private float lastAttackTime = 0f;

    private void Start()
    {
        startPosition = transform.position;
        curtHP = MonsterHP;
    }
    private void Update()
    {
        if (player == null)
            return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
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
            if (distanceToPlayer > detectionRange)
            {
                //�÷��̾ ������ ������ ����� ���Ͱ� ������ġ�� ����
                transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);

                if (transform.position == startPosition)
                {
                    isChasing = false;
                }
                myanim.SetBool("Ismoving", false);
            }
        
       
    }
    private void Battle()
    {
        isAttacking = true;
        myanim.SetTrigger("Attack");
        lastAttackTime = Time.time;
        
    }
    public void TakeDamage(int damage)
    {
        curtHP -= damage;
        myanim.SetTrigger("Damage");
        // ���Ͱ� ���̻� �������� ���� �� ������ ���
        if (curtHP <= 0)
        {
            Die();
            myanim.SetTrigger("Die");
        }
        else
        {
            myanim.SetTrigger("Damage");
        }
    }
    public void Die()
    {
        // ���� ����� �ִϸ��̼� �߰�
        myanim.SetTrigger("Die");
        Destroy(gameObject, deathDelay);
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
