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

        private void Start()
        {
            if (shopUI != null)
            {
                shopUI.SetActive(false);
            }
        }

        void BuyItem()
        {

        }

        public void OnPurchase()
        {
            itemToBuy = EventSystem.current.currentSelectedGameObject; // ������ ������ ����
            ShopItem_LNH shopItem = itemToBuy.GetComponent<ShopItem_LNH>(); // ������ �������� ������ �ִ� ��ũ��Ʈ
            if (shopItem != null) // ���� �� ��ũ��Ʈ�� �����Ѵٸ�
            {
                ItemStat buyItemStat = shopItem.GetItemStat();
                updateStatAct?.Invoke(buyItemStat);
                Debug.Log($"{buyItemStat.ItemType} Ÿ��, ���ݷ� {buyItemStat.Ap}, �߰� ü�� {buyItemStat.PlusHeart}, �̼� {buyItemStat.PlusSpeed}");
            } // ������ ��� ó���� �ϸ� �ǰڽ��ϴ�. ����� �ܼ��� ���� ��¸� �մϴ�.

        }
    }
}

