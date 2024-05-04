using InventorySystem;
using LGH;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Heal : MonoBehaviour
{
    public UnityEvent<ItemStat> updateStatAct;
    public GameObject shopUI;
    public GameObject shopBuyQues;
    public GameObject CheckBuyItems;
    public GameObject NoMoney;
    public GameObject FinishBuy;
    public int NowGold = 0;

    public int healthToRestore = 100; // ȸ���� ü�� ��

    public PlayerController player;
    private GoldManager playerGoldManager;
    private int playerGold;

    private void Start()
    {
        shopUI.SetActive(false);
        shopBuyQues.SetActive(false);
        CheckBuyItems.SetActive(false);
        NoMoney.SetActive(false);
        FinishBuy.SetActive(false);

        // �÷��̾��� ��� �����ڸ� ã�� �Ҵ��մϴ�.
        playerGoldManager = FindObjectOfType<GoldManager>();
        //player = FindObjectOfType<PlayerController>();
        // ���� ��带 �����ɴϴ�.
        playerGold = playerGoldManager.GetPlayerGold();
        
    }

    public void OnPurchase()
    {
        
        int healthItemPrice = 0; // ü�� ȸ�� �������� ���� (����)
        playerGoldManager.ChangeGold(-100);
        Debug.Log("���� ���� �ݾ� : ");
        Debug.Log(playerGold);
        if (playerGoldManager.GetPlayerGold() > healthItemPrice)
        {
            // ����� ��尡 �ִ� ���
            //UpdatePlayerGold(-healthItemPrice); // ��� ����
            Nomoney();
            Debug.Log("���� ���� �ݾ� : ");
            Debug.Log(playerGold);
            RestoreHealth(); // ü�� ȸ��
            CheckBuyItems.SetActive(false);
            FinishBuy.SetActive(true);
        }
        else
        {
            // ��尡 ������ ���
            NoMoney.SetActive(true);
            Debug.Log("ü�� ȸ�� �������� ������ ��尡 �����մϴ�.");
        }
    }

    // �÷��̾��� ��带 ������Ʈ�մϴ�.
    private void UpdatePlayerGold(int amount)
    {
        playerGold += amount;
        playerGoldManager.ChangeGold(amount);
    }

    public void Nomoney()
    {
        playerGold = 0;
    }

    // �÷��̾��� ü���� ȸ���մϴ�.
    public void RestoreHealth()
    {
        if (player != null) player.HealWithFullHealth();
        CheckBuyItems.SetActive(true);
        Debug.Log("�÷��̾� ü�� ȸ�� : " + player.GetCurHP());
    }

    private void Update()
    {
        
    }
}
