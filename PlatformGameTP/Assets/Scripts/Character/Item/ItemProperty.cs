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
    [Tooltip("�������� Ÿ��")]public ITEMTYPE ItemType; // ������ Ÿ��
    [Tooltip("������(����)�� ���ݷ�")] public int Ap; // ���ݷ�(����)
    [Tooltip("������(��)�� �߰� ü��")] public int PlusHeart; // �߰� ü��(��)
    [Tooltip("������(��ű�)�� �߰� �̼�")] public float PlusSpeed; // �߰� �̼�
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
