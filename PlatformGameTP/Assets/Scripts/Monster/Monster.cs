using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : BattleSystem
{
    private enum MonsterState
    {
        Idle,
        Roaming,
        Battle,
        Dead
    }

    private MonsterState currentState;
    private Transform PlayerTransfrom;
    [SerializeField] Vector3 leftLimitPos;
    [SerializeField] Vector3 rightLimitPos;
    Vector3 limitPos;
    private float RoamingRange = 3.0f;//�ι� �ݰ�
    private float BattleRange = 1.0f;//��Ʋ �ݰ�
    
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(MonsterState.Dead);
        StartCoroutine(MonsterStateMachine());
    }

    void ChangeState(MonsterState s)
    {

    }

    IEnumerator MonsterStateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case MonsterState.Idle: //���� ���� ����
                    currentState = MonsterState.Roaming;
                    break;
                case MonsterState.Roaming: //���� �ι� ����
                    yield return new WaitForSeconds(3f);
                    currentState = MonsterState.Battle;
                    break;
                case MonsterState.Battle: //���� ��Ʋ ����
                    yield return new WaitForSeconds(5f);
                    currentState = MonsterState.Dead;
                    break;
                case MonsterState.Dead: // ���� ���� ���� ���� ���� ��ȭ ����
                    yield return null;
                    break;
                default:
                    yield return null;
                    break;
            }
        }
    }

    void Roam()
    {
        // �ι� ����
    }

    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, PlayerTransfrom.position) > RoamingRange)
        {
            currentState = MonsterState.Idle;
        }
        else if(Vector3.Distance(transform.position, PlayerTransfrom.position)> BattleRange)
        {
            currentState = MonsterState.Battle;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
