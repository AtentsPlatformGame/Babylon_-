using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �κ��丮 ����â�� �� ������ ���� ���� ��ũ��Ʈ
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
    [Tooltip("������ �̹����� ���ε����ּ���")] public Sprite itemIcon;
    [Header("������ ���� �̹���")]
    [Tooltip("������ ���� �̹����� ���ε����ּ���")] public Sprite itemDescriptionImage;
    [Header("�������� Ÿ��")]
    [Tooltip("�������� Ÿ���� �����մϴ�.")]public ITEMTYPE ItemType; // ������ Ÿ��
    [Header("������(����)�� ���ݷ�")] 
    [Tooltip("������ ���ݷ��� �����մϴ�.")] public float Ap; // ���ݷ�(����)
    [Header("������(��)�� �߰� ü��")] 
    [Tooltip("�� �߰� ü�� ������ �����մϴ�.")] public float PlusHeart; // �߰� ü��(��)
    [Header("������(��ű�)�� �߰� �̼�")] 
    [Tooltip("��ű��� �߰� �̵��ӵ��� �����մϴ�.")] public float PlusSpeed; // �߰� �̼�
    [Header("������(��ű�) ����")]
    [Tooltip("�������� ������ �����մϴ�.")] public int ItemsPrice; // �߰� �̼�
    // �� �Ʒ��� ���� ���� �߰�
}
public class ItemProperty : MonoBehaviour
{
    [SerializeField] protected ItemStat itemStat;
    public static int PlayerGold;


    #region GEt�Լ� �Ʒ� �Լ����� �̿��� �ʿ��� �������� ���ϴ�.

    public ItemStat GetItemStat()
    {
        return this.itemStat;
    }
   
    #endregion

}
