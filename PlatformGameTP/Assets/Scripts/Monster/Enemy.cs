using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Create, Normal, Battle, Death
    }

    public float moveSpeed = 5f;
    public float raycastDistance = 1f;
    public LayerMask groundMask;
    public Transform target;

    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;
    private Vector2 moveDirection;

    void Start()
    {
        //�÷��̾ Ÿ������ ����
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }




    void Update()
    {
        // ���Ͱ� �÷��̾ �����ϵ��� �̵� ������ ����
         MoveDirection();

        // �̵�
        transform.Translate(moveDirection *moveSpeed* Time.deltaTime);
    }

    void MoveDirection()
    {
        moveDirection = (target.position - transform.position).normalized;
        // ���� �������� Ray
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, groundMask);
        // ������ �������� Ray
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, groundMask);

        // Ray�� ���� ���ο� ���� �̵� ������ ������
        if (hitLeft.collider == null || hitRight.collider == null)
        {
            // �� �ʿ� ���� ������ �ݴ������� �̵���
            if (hitLeft.collider == null)
                moveDirection = Vector2.right;
            else
                moveDirection = Vector2.left;
        }
       
    }
    
    
}
