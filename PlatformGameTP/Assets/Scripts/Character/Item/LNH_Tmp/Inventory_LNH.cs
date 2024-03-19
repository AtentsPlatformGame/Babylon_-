using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory_LNH : MonoBehaviour
{
    public UnityEvent<ItemStat> updatePlayerStatAct;
    public InventorySlot_LNH[] inventorySlots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �������� �������� ��, ������ �Ĺֽ� ��� ����ϴ� �ڵ�
    public void UpdateInventory(ItemStat _itemStat)
    {
        Debug.Log($"���� ���� ������ : {_itemStat.ItemType} Ÿ��, ���ݷ� {_itemStat.Ap}, �߰� ü�� {_itemStat.PlusHeart}, �̼� {_itemStat.PlusSpeed}");
        switch (_itemStat.ItemType)
        {
            case ITEMTYPE.WEAPON:
                inventorySlots[0].SetItemStat(_itemStat);
                updatePlayerStatAct?.Invoke(_itemStat);
                break;
            case ITEMTYPE.ARMOR:
                inventorySlots[1].SetItemStat(_itemStat);
                updatePlayerStatAct?.Invoke(_itemStat);
                break;
            case ITEMTYPE.ACCE:
                break;
            case ITEMTYPE.CURSEDACCE:
                break;
            case ITEMTYPE.PASSIVE:
                break;
            case ITEMTYPE.SPELL:
                break;
        }
    }

    
}
