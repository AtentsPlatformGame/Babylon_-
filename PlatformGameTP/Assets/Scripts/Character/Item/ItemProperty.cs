using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ݿ� ���������� ������ �ִ� ����,��,��ű��� ���� ���� ��ũ��Ʈ
public enum ITEMTYPE
{
    NONE, // �������� �ƴ�
    WEAPON, // ����
    ARMOR, // ��
    ACCE, // ��ű�(�Ǽ�)
    SPELL, // ����
    PASSIVE, // �нú� ������
    CURSEDACCE // ���ֹ��� ��ű�
}

[System.Serializable]
public struct ItemStat
{
    [Header("�������� ������")]
    [Tooltip("������ �̹����� ���ε����ּ���")] public Image itemIcon;
    [Header("�������� Ÿ��")]
    [Tooltip("�������� Ÿ���� �����մϴ�.")]public ITEMTYPE ItemType; // ������ Ÿ��
    [Header("������(����)�� ���ݷ�")] 
    [Tooltip("������ ���ݷ��� �����մϴ�.")] public int Ap; // ���ݷ�(����)
    [Header("������(��)�� �߰� ü��")] 
    [Tooltip("�� �߰� ü�� ������ �����մϴ�.")] public float PlusHeart; // �߰� ü��(��)
    [Header("������(��ű�)�� �߰� �̼�")] 
    [Tooltip("��ű��� �߰� �̵��ӵ��� �����մϴ�.")] public float PlusSpeed; // �߰� �̼�
    // �� �Ʒ��� ���� ���� �߰�
}
public class ItemProperty : MonoBehaviour
{
    [SerializeField] protected ItemStat itemStat;

    #region GEt�Լ� �Ʒ� �Լ����� �̿��� �ʿ��� �������� ���ϴ�.

    public ItemStat GetItemStat()
    {
        return this.itemStat;
    }
    /*public Image GetItemIcon()
    {
        return this.itemStat.itemIcon;
    }
    public ITEMTYPE GetItemType()
    {
        return itemStat.ItemType;
    }
    public int GetAp()
    {
        return itemStat.Ap;
    }

    public float GetPlusHeart()
    {
        return itemStat.PlusHeart;
    }

    public float GetPlusSpeed()
    {
        return itemStat.PlusSpeed;
    }*/
    #endregion
}
