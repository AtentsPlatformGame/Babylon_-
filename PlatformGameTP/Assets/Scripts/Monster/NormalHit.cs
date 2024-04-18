using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHit : MonoBehaviour
{
    public LayerMask NormalAttackMask;
    //�븻 ���� ���ݽ� ����ũ ������
    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer& NormalAttackMask) !=0)
        {
            BattleSystem bs = other.GetComponent<BattleSystem>();
            if(bs != null)
            {
                bs.TakeDamage(1.0f);
            }
        }
    }
}
