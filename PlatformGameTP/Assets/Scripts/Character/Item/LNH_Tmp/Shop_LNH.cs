using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop_LNH : MonoBehaviour
{
    public UnityEvent<int, float, float> updateStatAct; // ���߿� �÷��̾� ������ �����ϱ� ���� ���� unityevent, ���� �����Ұ���
    GameObject itemToBuy;
    /*
     ��� �̹���(��ư)�� ������ ��, �� ����(� ���� ��������)�� �����ϱ� ���� ���� GameObject ����
     */

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void OnPurchase()
    {
        itemToBuy = EventSystem.current.currentSelectedGameObject; // ������ ������ ����
        ShopItem_LNH shopItme = itemToBuy.GetComponent<ShopItem_LNH>(); // ������ �������� ������ �ִ� ��ũ��Ʈ
        if (shopItme != null) // ���� �� ��ũ��Ʈ�� �����Ѵٸ�
        {
            //updateStatAct?.Invoke();
            Debug.Log($"{shopItme.GetItemType()} Ÿ��, ���ݷ� {shopItme.GetAp()}, �߰� ü�� {shopItme.GetPlusHeart()}, �̼� {shopItme.GetPlusSpeed()}");
        } // ������ ��� ó���� �ϸ� �ǰڽ��ϴ�. ����� �ܼ��� ���� ��¸� �մϴ�.
        
       
    }
}
