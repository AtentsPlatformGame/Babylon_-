using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_LNH : ItemProperty
{
    int _price; // ���߿� ���ݿ� �� ������Ƽ, ������ ���� ������ �������� �ʿ����� ������ ���Ƽ� ���� ��
    public int price {
        get => this._price;

        set
        {
            this._price = value;
        }
    }

}
