using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPicking : MonoBehaviour
{
    public Transform spellImg;
    public UnityEvent attackAct; // PlayerController Attack()
    public LayerMask layerMask;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isAlive() == false || playerController == null) return;
        
        if (playerController.isSpellReady == false) // ���� ��� �غ� ��
        {
            
            if (Input.GetMouseButtonDown(0)) // ���� �ȵ� ���߿� �����ҵ� unityevents�� ����غ� ����, �ٵ� isAlive�� ��� ���� �𸣰���
            {
                attackAct?.Invoke();
                Debug.Log("Click");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("���� ��� �غ�");
                playerController.ReadyToUseSpell(true);
            }
        }
        else // ���� ��� �غ� ��
        {
            
            if (Input.GetMouseButtonDown(0)) // ���� ���
            {
                playerController.UsingSpell();
                playerController.ReadyToUseSpell(false);
                Debug.Log("���� ���");
                //playerController.ResetSpellTrigger();
            }
            else if (Input.GetMouseButtonDown(1)) // ���� ��� ���
            {
                Debug.Log("���� ��� ���");
                playerController.ReadyToUseSpell(false);
            }
            
        }
        

    }

   
}
