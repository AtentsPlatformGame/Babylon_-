using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public struct BattleStat
{
    public float AP; // ���ݷ�
    public float MaxHp; // �ִ� ü��
    public float AttackRange; // ���� ��Ÿ�
    public float AttackDelay; // ���� �ӵ�
    public float ProjectileSpeed; // ����ü �ӵ�
    public float MoveSpeed; // �̵��ӵ�
    // �������� ������� ���Ϳ� �÷��̾ �����ϴ� �κ�
    // �Ʒ����ʹ� ���� �÷��̾� ���� ���ݵ�

    /*
        ũ�� ���� -> int ���� ���� ó��
        ������ �ѹ� �� �� -> bool ���� ���� ó��
        ü�� ȸ�� -> bool ���� ���� ó��
        ���� �� ü�� 3���� ��Ȱ -> bool ���� �߰�
        �ǰ� ��ĭ ���� -> bool ���� �߰�
     */
    public int AttackSize; // ��Ÿ ũ��
    public bool AttackTwice; // ������Ÿ ����
    public bool HealAfterAttack; // ���� ����
    public bool ResurrectionOneTime; // ��Ȱ ����
    public bool HitOnlyHalf; // �ǰ� ��ĭ ����

    public bool CA_AttackPenalty; // ���ݷ� +2, �ǰݵ� *2
    public bool CA_GoldPenalty; // ��� ȹ�� 3��, �ǰݽ� ��� ��� �ҽ�
    public bool CA_HPPenalty; // ��1 ü��1 ����, �� �̵����� �ǰݴ���

    public Transform StatWindows;
}

public interface IDamage
{
    void TakeDamage(float _dmg);
}

public class BattleSystem : CharacterProperty, IDamage
{
    public UnityEvent<float> changeHpAct;
    [SerializeField] protected BattleStat battleStat;
    [SerializeField] float _curHP = 0.0f;
    public event UnityAction deathAlarm; // event Ű���尡 ������ �� Ŭ���� �ۿ��� �ʱ�ȭ, ������ �Ұ�����. ���� ���� �����ڿ� ����� ����? �Ǽ� ����
    
    public Transform attackPoint;
    public LayerMask enemyMask;
    [Header("�÷��̾� ���� ����")]
    public AudioClip fireballClip;
    public AudioClip jumpClip;
    public AudioClip healbuffClip;
    public AudioClip spellClip;
    public AudioClip speedbuffClip;
    public AudioClip deadClip;
    public AudioClip hitClip;
    //public AudioSource jumpSound;

    protected float curHp; // �����κ� protected float curHp = 0.0f
    protected float battleTime = 0.0f;
    Transform _target = null;

    public AudioSource myAudioSource;
    public AudioClip deadsound;
    public AudioClip hitsound;
    Slider volumeSlider;
    private void Start()
    {
        
    }
    protected float curHP
    {
        get => _curHP;
        set
        {
            _curHP = value;
            changeHpAct?.Invoke(_curHP / battleStat.MaxHp);
        }
    }

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

    public int GetAttackSize()
    {
        return this.battleStat.AttackSize;
    }

    public bool GetAttackTwice()
    {
        return this.battleStat.AttackTwice;
    }

    public bool GetHealAfterAttack()
    {
        return this.battleStat.HealAfterAttack;
    }

    public bool GetResurrectionOneTime()
    {
        return this.battleStat.ResurrectionOneTime;
    }

    public bool GetHitOnlyHalf()
    {
        return this.battleStat.HitOnlyHalf;
    }

    public float GetCurHP()
    {
        return this.curHP;
    }

    public float GetMaxHP()
    {
        return this.battleStat.MaxHp;
    }

    public float GetMoveSpeed()
    {
        return this.battleStat.MoveSpeed;
    }
    public bool GetCA_AttackPenalty()
    {
        return this.battleStat.CA_AttackPenalty;
    }

    public bool GetCA_HpPenalty()
    {
        return this.battleStat.CA_HPPenalty;
    }

    public bool GetCA_GoldPenalty()
    {
        return this.battleStat.CA_GoldPenalty;
    }

    #endregion

    private void TargetDead()
    {
        StopAllCoroutines();
    }

    protected void Initialize()
    {
        curHP = battleStat.MaxHp;
    }

    

    public virtual void TakeDamage(float _dmg)
    {
        curHP -= _dmg;
        Debug.Log(curHP);
        PlaySound(hitsound);
        if (curHP <= 0.0f)
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
        foreach (Renderer renderer in allRenderer)
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
        PlaySound(deadsound);
        deathAlarm?.Invoke();
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
    }


    public bool isAlive()
    {
        return curHP > 0.0f;
    }

    public void SetAudioSourceVolume(float value)
    {
        myAudioSource.volume = value;
    }

    public void SetVolumeSlider(Slider _slider)
    {
        volumeSlider = _slider;
        Debug.Log("ȿ���� �����̴� ����");
        volumeSlider.onValueChanged.AddListener(SetAudioSourceVolume);
    }

    public void PlaySound(AudioClip clip)
    {
        if (myAudioSource != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
        }
    }

}

