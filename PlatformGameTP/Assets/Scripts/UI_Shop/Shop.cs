using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LGH
{
    public class Shop : MonoBehaviour
    {
        public UnityEvent<ItemStat> updateStatAct;
        GameObject itemToBuy;

        public GameObject shopUI;
        public GameObject shopBuyQues;
        public GameObject CheckBuyItems;
        public GameObject[] itemObj;
        public int[] itemPrice;

        private void Start()
        {
            shopUI.SetActive(false);
            shopBuyQues.SetActive(false);
            CheckBuyItems.SetActive(false);
            
        }

        public void OnPurchase()
        {
            //itemToBuy = EventSystem.current.currentSelectedGameObject; // ������ ������ ����
            ShopItem_LNH shopItem = itemToBuy.GetComponent<ShopItem_LNH>(); // ������ �������� ������ �ִ� ��ũ��Ʈ
            if (shopItem != null) // ���� �� ��ũ��Ʈ�� �����Ѵٸ�
            {
                ItemStat buyItemStat = shopItem.GetItemStat();
                updateStatAct?.Invoke(buyItemStat);
                Debug.Log($"{buyItemStat.ItemType} Ÿ��, ���ݷ� {buyItemStat.Ap}, �߰� ü�� {buyItemStat.PlusHeart}, �̼� {buyItemStat.PlusSpeed}");
            } // ������ ��� ó���� �ϸ� �ǰڽ��ϴ�. ����� �ܼ��� ���� ��¸� �մϴ�.

        }

        public void CheckBuyItem()
        {
            CheckBuyItems.SetActive(true);
            itemToBuy = EventSystem.current.currentSelectedGameObject;

        }
    }
}
/*
    UI ��ư(���� ������ WeaponBT,ArmorBT ������� ����)�� ������ �� ȣ��Ǵ� OnClick �Լ��� �� �Լ�
   ��ư�� ������ ������ ���� ��ư�� ������ �ִ� ������ ������ ����ϴµ�
   ���߿��� ��ư�� ������ ���� ���θ� ���� â�� ��Ÿ���� �� â���� �ٽ� Ȯ���� ������ ���� ��ǰ ���ݰ� �������� ���Ͽ�
   ���� ������ ���� -> �Ʒ� �ڵ忡�� updateStatAct?.Invoke(); ȣ�� �� ��ư�� ������ �����ϰų� ��ü�Ѵ�.
    -> Invoke�� �÷��̾� ������ �����ϴ� �Լ��� ������ ������ �˴ϴ�. �� �Լ��� ���߿� �κ��丮������ ��Ȳ�� ���� ����� �� �����Ű����ϴ�.
   ���� ������ ���� -> �׳� �ƹ��͵� ���ϱ�

   ȣ�� �� ��ư�� ������ �����ϰų� ��ü�Ѵ� ->
   ���� ��ܽ� ���׷��̵带 �����ϰ����ϴµ�, ���� -> �� -> ö -> ���̾� �̷����� ���׷��̵忡��
   ������ �����ϸ� ��ư�� ���� �ٲ��, ���� �����ϸ� ö��, ö���� ���̾Ʒ�, ���̾ƿ��� sold out �̹����� �ܰ躰�� �����ϴ� ���� �ǹ��մϴ�

   �ܰ躰�� �����ϱ� ���ؼ� 2���� ����� �������µ�,
   1 : ��ư�� ��� �̸� �������� �������� setactive�� true���� false, false���� true�� �ٲ㳪���°̴ϴ�

   2 : ��ư�� Ŭ���� ���Ű� �������� �� �̸� ������ �ܰ躰 �������� ������ �ϳ��� ��ư�� �������� �����س����� �̴ϴ�.

   2�� �ϳ��� ����� �̿��� �����ϴ� ������ ������ �����ϸ� �� �� �����ϴ�.
    */