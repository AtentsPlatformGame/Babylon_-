using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory_LNH : MonoBehaviour
{
    public UnityEvent<ItemStat> updatePlayerStatAct; // �÷��̾� ������ ���� �Է¹��� ������ ������ �����ϴ� UnityEvent
    public UnityEvent<ItemStat>[] updateItemStat; // �� �κ��丮 ���Ը��� ���� �޸� ������ �����ϴ� UnityEvent

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �������� �������� ��, ������ �Ĺֽ� ��� ����ϴ� �ڵ�
    public void UpdateInventory(ItemStat _itemStat) // ���� ����, �Ĺ��� �������� ���Դٸ�
    {
        // ������ Ÿ�Կ� ���� �� ĭ�� ���ε� �� �Լ��� ȣ���ϰ�
        // �÷��̾� ������ ������
        switch (_itemStat.ItemType)
        {
            case ITEMTYPE.WEAPON:
                UpdateSlot(0, _itemStat);
                break;
            case ITEMTYPE.ARMOR:
                UpdateSlot(1, _itemStat);
                break;
            case ITEMTYPE.ACCE:
                UpdateSlot(2, _itemStat);
                break;
            case ITEMTYPE.CURSEDACCE:
                UpdateSlot(3, _itemStat);
                break;
            case ITEMTYPE.PASSIVE:
                UpdateSlot(4, _itemStat);
                break;
            case ITEMTYPE.SPELL:
                UpdateSlot(5, _itemStat);
                break;
            default:
                break;
        }
    }

    void UpdateSlot(int idx, ItemStat _itemStat)
    {
        updateItemStat[idx]?.Invoke(_itemStat);
        updatePlayerStatAct?.Invoke(_itemStat);

    }
    
}
