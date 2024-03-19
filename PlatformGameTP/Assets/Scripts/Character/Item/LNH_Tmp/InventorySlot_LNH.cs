using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot_LNH : ItemProperty
{
    int slotNumber;
    Image myImg;
    // Start is called before the first frame update
    void Start()
    {
        myImg = GetComponent<Image>(); // ������ ������ �̹���
        slotNumber = GetSlotNumber(); // �ڽ��� �θ�κ��� ���° �ڽ����� ��Ÿ���� ����
        Inventory_LNH myInventory = FindObjectOfType<Inventory_LNH>(); // �κ��丮 ��ũ��Ʈ�� ã�Ƽ�
        if(myInventory != null) // ���� �ƴ϶��
            myInventory.updateItemStat[slotNumber].AddListener(SetItemStat); // UnityEvent�� ���ε�
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // UnityEvent�� ���ε��� �Լ�. ���� ĭ�� ������ ������ ���� ���� ������ ������ �����մϴ�.
    void SetItemStat(ItemStat _itemStat)
    {
        this.itemStat = _itemStat;
        SetInventorySlot();
    }

    void SetInventorySlot() // ������ ������ �̹����� �ٲߴϴ�.
    {
        myImg.sprite = this.itemStat.itemIcon;
    }

    /*
     �Ʒ� �Լ��� �θ�κ��� �ڽ��� ���° �ڽ����� �˾Ƴ��� �ڵ�
    UnityEvent ����ÿ� ����մϴ�
     */
    int GetSlotNumber()
    {
        var cnt = -1;
        for (var i = 0; i < transform.parent.childCount; i++)
        {
            var view = transform.parent.GetChild(i);
            if (view.gameObject.activeSelf)
            {
                cnt++;
                if (view.transform == transform) return cnt;
            }
        }
        return cnt;
    }
}
