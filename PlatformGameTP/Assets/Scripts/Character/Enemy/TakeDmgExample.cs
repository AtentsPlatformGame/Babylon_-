using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDmgExample : BattleSystem
{
    /*
     �ӽ� ��ũ��Ʈ
    ���߿� ������ ����
     
     */

    public Transform player;
    
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            player.GetComponent<BattleSystem>().TakeDamage(1.0f);
        }
    }
}
