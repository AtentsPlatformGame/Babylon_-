using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct BattleStat
{
    public float AP;
    public float MaxHp;
    public float AttackRange;
    public float AttackDelay;
    public float MoveSpeed;
}

public interface IDamamge
{
    void TakeDamage(float _dmg);
}

public class BattleSystem : CharacterProperty, IDamamge
{
    [SerializeField] protected BattleStat battleStat;
    public event UnityAction deathAlarm; // event Ű���尡 ������ �� Ŭ���� �ۿ��� �ʱ�ȭ, ������ �Ұ�����. ���� ���� �����ڿ� ����� ����? �Ǽ� ����

    protected float curHp = 0.0f;
    protected float battleTime = 0.0f;
    Transform _target = null;

    protected Transform myTarget
    {
        get => _target;
        set
        {
            _target = value;
            if (_target != null)
            {
                BattleSystem bs = _target.GetComponent<BattleSystem>();
                if (bs != null)
                {
                    bs.deathAlarm += TargetDead;
                }
            }
        }
    }

    private void TargetDead()
    {
        StopAllCoroutines();

    }

    protected void Initialize()
    {
        curHp = battleStat.MaxHp;
    }
    public void TakeDamage(float _dmg)
    {
        curHp -= _dmg;
        if (curHp <= 0.0f)
        {
            // ü���� �� �� ������
            OnDead();
            myAnim.SetTrigger("Dead");
        }
        else
        {
            myAnim.SetTrigger("Damage");
        }
    }

    public void OnAttack()
    {
        if (myTarget == null) return;
        BattleSystem bs = myTarget.GetComponent<BattleSystem>();
        if (bs != null)
        {
            bs.TakeDamage(battleStat.AP);
        }
    }

    protected virtual void OnDead()
    {
        deathAlarm?.Invoke();
        GetComponent<Collider>().enabled = false;

    }

    public bool isAlive()
    {
        return curHp > 0.0f;
    }
}

