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
    SPELL, // ����
    PASSIVE, // �нú� ������
    CURSEDACCE, // ���ֹ��� ��ű�
    BOSSTOKEN1,
    BOSSTOKEN2,
}

[System.Serializable]
public struct ItemStat
{
    [Header("�������� �����ѹ�")]
    [Tooltip("������ �����ѹ�(Weapon 0~9 / Amor 10 ~ 19 / Spell 20 ~ 29 / Passive 30 ~ 39 / Cursed 40 ~49")] public int ItemNumber;
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
    [Header("������(��ű�)�� �߰� ����ü �ӵ�")]
    [Tooltip("��ű��� �߰� ����ü�ӵ��� �����մϴ�.")] public float PlusProjectileSpeed; // �߰� ����ü�ӵ�
    [Header("������(��ű�)�� �߰� ���ݼӵ�")]
    [Tooltip("��ű��� �߰� ���ݼӵ��� �����մϴ�.")] public float PlusAttackDelay; // �߰� ���� �ӵ�
    [Header("������(��ű�)�� �߰� �����Ÿ�")]
    [Tooltip("��ű��� �߰� �����Ÿ��� �����մϴ�.")] public float PlusAttackRange; // �߰� �����Ÿ�
    [Header("������(��ű�) ����")]
    [Tooltip("�������� ������ �����մϴ�.")] public int ItemsPrice; // ������ ����
    [Header("������(����) VFX")]
    [Tooltip("���� �������� ������ ��� ���� �������� �־��ּ���.")] public Transform SpellObject; // ������ �� ���� vfx

    // �� �Ʒ��� ���� ���� �߰�
    [Header("��Ÿ ũ�� ����")]
    [Tooltip("������ ȿ���� ���̾ ũ�⸦ Ű��ϴ�."), Range(1,2)] public int PlusAttackSize; // ��Ÿ ũ��
    [Header("��Ÿ�� �ι� �߻� �� �� �ִ���")]
    [Tooltip("������ ȿ���� ���̾�� �ѹ��� �ι� �߻��մϴ�.")] public bool IsAttackTwice; // ������Ÿ ����
    [Header("��Ÿ�� ���� ����")]
    [Tooltip("������ ȿ���� ��Ÿ ���� �� 30% Ȯ���� ü���� ȸ���մϴ�.")] public bool IsHealAfterAttack; // ���� ����
    [Header("��Ȱ ����")]
    [Tooltip("������ ȿ���� 1ȸ ��Ȱ �� �� �ֽ��ϴ�.")] public bool IsResurrectionOneTime; // ��Ȱ ����
    [Header("��ĭ �ǰ� ���� ����")]
    [Tooltip("������ ȿ���� �ǰݽ� ü���� ��ĭ�� ��ϴ�.")] public bool IsHitOnlyHalf; // �ǰ� ��ĭ ����
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
