using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBoss_FlyingDemonKing : EnemyState
{
    public Transform clawAttackEffect; // ������ ���� vfx
    public Transform biteAttackEffect; // ������ ���� vfx
    public Transform fireBallEffect; // ���Ÿ� ���� vfx

    public Transform clawAttackPoint; // ������ ���� ����Ʈ
    public Transform biteAttackPoint; // ������ ���� ����Ʈ
    public Transform fireBallPoint; // ���Ÿ� ���� �߻� ����Ʈ

    [SerializeField] Transform spawnMonster1;
    [SerializeField] Transform spawnMonster2;
    [SerializeField] bool isPhaseChanged = false;

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
                //StartCoroutine(SpawnSkeleton(spawnPoint));
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
