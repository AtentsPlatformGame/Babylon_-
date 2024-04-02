using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct BattleStat
{
    public float AP; // ���ݷ�
    public float MaxHp; // �ִ� ü��
    public float AttackRange; // ���� ��Ÿ�
    public float AttackDelay; // ���� �ӵ�
    public float ProjectileSpeed; // ����ü �ӵ�
    public float MoveSpeed; // �̵��ӵ�
}

public interface IDamage
{
    void TakeDamage(float _dmg);
}

public class BattleSystem : CharacterProperty, IDamage
{
    [SerializeField] protected BattleStat battleStat;
    public event UnityAction deathAlarm; // event Ű���尡 ������ �� Ŭ���� �ۿ��� �ʱ�ȭ, ������ �Ұ�����. ���� ���� �����ڿ� ����� ����? �Ǽ� ����
    public Transform attackPoint;
    public LayerMask enemyMask;

    protected float curHp; // �����κ� protected float curHp = 0.0f
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
    #region get,set
    public float GetAp()
    {
        return this.battleStat.AP;
    }

    public float GetAttackRange()
    {
        return this.battleStat.AttackRange;
    }

    public float GetProjectileSpeed()
    {
        return this.battleStat.ProjectileSpeed;
    }
    #endregion

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
        Debug.Log(curHp);
        if (curHp <= 0.0f)
        {
            // ü���� �� �� ������
            OnDead();
            myAnim.SetTrigger("Dead");
        }
        else
        {
            myAnim.SetTrigger("Damage");
            StartCoroutine(DamagingEffect(Color.red));
        }
    }

    IEnumerator DamagingEffect(Color effColor)
    {
        foreach(Renderer renderer in allRenderer)
        {
            renderer.material.color = effColor;
        }
        yield return new WaitForSeconds(0.3f);
        foreach (Renderer renderer in allRenderer)
        {
            renderer.material.color = Color.white;
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
        GetComponent<Rigidbody>().useGravity = false;
    }


    public bool isAlive()
    {
        return curHp > 0.0f;
    }

}

