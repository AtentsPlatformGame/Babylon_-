using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory_LNH : MonoBehaviour
{
    public UnityEvent<BattleStat> updatePlayerStatAct; // �÷��̾� ������ ���� �Է¹��� ������ ������ �����ϴ� UnityEvent
    public UnityEvent<ItemStat> updatePlayerSpell;
    public UnityEvent<ItemStat>[] updateItemStat; // �� �κ��丮 ���Ը��� ���� �޸� ������ �����ϴ� UnityEvent
    public UnityEvent<float, bool> updatePlayerHP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*private void OnEnable()
    {
        BattleStat calStat = new BattleStat();
        calStat = CalculateInven();
        updatePlayerStatAct?.Invoke(calStat);
    }*/

    // �������� �������� ��, ������ �Ĺֽ� ��� ����ϴ� �ڵ�
    public void UpdateInventory(ItemStat _itemStat) // ���� ����, �Ĺ��� �������� ���Դٸ�
    {
        BattleStat calStat = new BattleStat();
        // ������ Ÿ�Կ� ���� �� ĭ�� ���ε� �� �Լ��� ȣ���ϰ�
        // �÷��̾� ������ ������
        switch (_itemStat.ItemType)
        {
            case ITEMTYPE.WEAPON:
                UpdateSlot(0, _itemStat);
                break;
            case ITEMTYPE.ARMOR:
                UpdateSlot(1, _itemStat);
                updatePlayerHP?.Invoke(_itemStat.PlusHeart, true);
                break;
            case ITEMTYPE.CURSEDACCE:
                UpdateSlot(2, _itemStat);
                break;
            case ITEMTYPE.PASSIVE:
                UpdateSlot(3, _itemStat);
                break;
            case ITEMTYPE.SPELL:
                UpdateSlot(4, _itemStat);
                updatePlayerSpell?.Invoke(_itemStat);
                break;
            case ITEMTYPE.BOSSTOKEN1:
                UpdateSlot(5, _itemStat);
                break;
            case ITEMTYPE.BOSSTOKEN2:
                UpdateSlot(6, _itemStat);
                break;

            default:
                break;
        }

        calStat = CalculateInven();
        updatePlayerStatAct?.Invoke(calStat);
    }

    void UpdateSlot(int idx, ItemStat _itemStat)
    {
        updateItemStat[idx]?.Invoke(_itemStat);

    }

    BattleStat CalculateInven()
    {
        BattleStat tmpStat = new BattleStat();

        for(int i = 0; i < 4; i++)
        {
            InventorySlot_LNH inventoryItemProperty = this.transform.GetChild(i).GetComponent<InventorySlot_LNH>();
            
            if (inventoryItemProperty != null)
            {
                ItemStat inventoryItemStat = inventoryItemProperty.GetItemStat();
                tmpStat.AP += inventoryItemStat.Ap;
                tmpStat.MaxHp += inventoryItemStat.PlusHeart;
                tmpStat.AttackRange += inventoryItemStat.PlusAttackRange;
                tmpStat.ProjectileSpeed += inventoryItemStat.PlusProjectileSpeed;
                tmpStat.AttackDelay += inventoryItemStat.PlusAttackDelay;
                tmpStat.MoveSpeed += inventoryItemStat.PlusSpeed;
                tmpStat.AttackSize += inventoryItemStat.PlusAttackSize;
                if(inventoryItemStat.IsAttackTwice) tmpStat.AttackTwice = inventoryItemStat.IsAttackTwice;
                if(inventoryItemStat.IsHealAfterAttack) tmpStat.HealAfterAttack = inventoryItemStat.IsHealAfterAttack;
                if(inventoryItemStat.IsResurrectionOneTime) tmpStat.ResurrectionOneTime = inventoryItemStat.IsResurrectionOneTime;
                if(inventoryItemStat.IsHitOnlyHalf) tmpStat.HitOnlyHalf = inventoryItemStat.IsHitOnlyHalf;
                if (inventoryItemStat.CA_AttackPenalty) tmpStat.CA_AttackPenalty = inventoryItemStat.CA_AttackPenalty;
                if (inventoryItemStat.CA_GoldPenalty) tmpStat.CA_AttackPenalty = inventoryItemStat.CA_GoldPenalty;
                if (inventoryItemStat.CA_HpPenalty) tmpStat.CA_AttackPenalty = inventoryItemStat.CA_HpPenalty;
            }
        }
        return tmpStat;
    }


    
}
