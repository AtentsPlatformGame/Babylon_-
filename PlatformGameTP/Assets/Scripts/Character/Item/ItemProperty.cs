using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ݿ� ���������� ������ �ִ� ����,��,��ű��� ���� ���� ��ũ��Ʈ
public enum ITEMTYPE
{
    WEAPON, // ����
    ARMOR, // ��
    ACCE // ��ű�(�Ǽ�)
}

[System.Serializable]
public struct ItemStat
{
    [Header("�������� Ÿ��")][Tooltip("�������� Ÿ���� �����մϴ�.")]public ITEMTYPE ItemType; // ������ Ÿ��
    [Header("������(����)�� ���ݷ�")] [Tooltip("������ ���ݷ��� �����մϴ�.")] public int Ap; // ���ݷ�(����)
    [Header("������(��)�� �߰� ü��")] [Tooltip("�� �߰� ü�� ������ �����մϴ�.")] public int PlusHeart; // �߰� ü��(��)
    [Header("������(��ű�)�� �߰� �̼�")] [Tooltip("��ű��� �߰� �̵��ӵ��� �����մϴ�.")] public float PlusSpeed; // �߰� �̼�
    // �� �Ʒ��� ���� ���� �߰�
}
public class ItemProperty : MonoBehaviour
{
    [SerializeField] protected ItemStat itemStat;

    #region GEt�Լ�
    public ITEMTYPE GetItemType()
    {
        return itemStat.ItemType;
    }
    public int GetAp()
    {
        return itemStat.Ap;
    }

    public int GetPlusHeart()
    {
        return itemStat.PlusHeart;
    }

    public float GetPlusSpeed()
    {
        return itemStat.PlusSpeed;
    }
    #endregion
}
