using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpellData
{
    [Header("�������� �����ѹ�")]
    [Tooltip("������ �����ѹ�(Weapon 0~9 / Amor 10 ~ 19 / Spell 20 ~ 29 / Passive 30 ~ 39 / Cursed 40 ~49")] public int ItemNumber;
    [Header("�������� ������")]
    [Tooltip("������ �̹����� ���ε����ּ���")] public Sprite itemIcon;
    [Header("������ ���� �̹���")]
    [Tooltip("������ ���� �̹����� ���ε����ּ���")] public Sprite itemDescriptionImage;
    [Header("�������� Ÿ��")]
    [Tooltip("�������� Ÿ���� �����մϴ�.")] public ITEMTYPE ItemType; // ������ Ÿ��
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

    [Header("��Ÿ ũ�� ����")]
    [Tooltip("������ ȿ���� ���̾ ũ�⸦ Ű��ϴ�."), Range(1, 2)] public int PlusAttackSize; // ��Ÿ ũ��
    [Header("��Ÿ�� �ι� �߻� �� �� �ִ���")]
    [Tooltip("������ ȿ���� ���̾�� �ѹ��� �ι� �߻��մϴ�.")] public bool IsAttackTwice; // ������Ÿ ����
    [Header("��Ÿ�� ���� ����")]
    [Tooltip("������ ȿ���� ��Ÿ ���� �� 30% Ȯ���� ü���� ȸ���մϴ�.")] public bool IsHealAfterAttack; // ���� ����
    [Header("��Ȱ ����")]
    [Tooltip("������ ȿ���� 1ȸ ��Ȱ �� �� �ֽ��ϴ�.")] public bool IsResurrectionOneTime; // ��Ȱ ����
    [Header("��ĭ �ǰ� ���� ����")]
    [Tooltip("������ ȿ���� �ǰݽ� ü���� ��ĭ�� ��ϴ�.")] public bool IsHitOnlyHalf; // �ǰ� ��ĭ ����

    // ���ֹ��� ��ű� ����, 
    [Header("���ݷ� +2, �ǰݽ� ������ 2�� ")]
    [Tooltip("������ ȿ���� ���ݷ��� 2 �������� �ǰ� �������� 2�谡 �˴ϴ�.")] public bool CA_AttackPenalty; // ���ݷ� ���� �г�Ƽ
    [Header("ȹ�� ��差 3��, �ǰݽ� ��� ��� �ҽ� ")]
    [Tooltip("������ ȿ���� ȹ�� ��差�� 3�谡 ������ �ǰݽ� ��� ��带 �ҽ��մϴ�.")] public bool CA_GoldPenalty; // ��� ���� �г�Ƽ
    [Header("���ݷ� 1, ü�� 1�� ���������� ���� �̵��� �� ���� �ǰݴ���")]
    [Tooltip("������ ȿ���� ���ݷ°� ü���� 1�� �������� ���� �̵��� ������ �ǰݴ��մϴ�.")] public bool CA_HpPenalty; // ü�� ���� �г�Ƽ
}
[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObject/SpellData", order = 1)]
public class OriginSpellData : ScriptableObject
{
    [SerializeField] SpellData spellDataInfo;
    public ItemStat GetSpellDataInfo()
    {
        ItemStat tmpStat = new ItemStat();
        tmpStat.ItemNumber = spellDataInfo.ItemNumber;
        tmpStat.itemIcon = spellDataInfo.itemIcon;
        tmpStat.itemDescriptionImage = spellDataInfo.itemDescriptionImage;
        tmpStat.ItemType = spellDataInfo.ItemType;
        tmpStat.Ap = spellDataInfo.Ap;
        tmpStat.PlusHeart = spellDataInfo.PlusHeart;
        tmpStat.PlusSpeed = spellDataInfo.PlusSpeed;
        tmpStat.PlusProjectileSpeed = spellDataInfo.PlusProjectileSpeed;
        tmpStat.PlusAttackDelay = spellDataInfo.PlusAttackDelay;
        tmpStat.PlusAttackRange = spellDataInfo.PlusAttackRange;
        tmpStat.ItemsPrice = spellDataInfo.ItemsPrice;
        tmpStat.SpellObject = spellDataInfo.SpellObject;
        tmpStat.PlusAttackSize = spellDataInfo.PlusAttackSize;
        tmpStat.IsAttackTwice = spellDataInfo.IsAttackTwice;
        tmpStat.IsHealAfterAttack = spellDataInfo.IsHealAfterAttack;
        tmpStat.IsResurrectionOneTime = spellDataInfo.IsResurrectionOneTime;
        tmpStat.IsHitOnlyHalf = spellDataInfo.IsHitOnlyHalf;
        tmpStat.CA_AttackPenalty = spellDataInfo.CA_AttackPenalty;
        tmpStat.CA_GoldPenalty = spellDataInfo.CA_GoldPenalty;
        tmpStat.CA_HpPenalty = spellDataInfo.CA_HpPenalty;

        return tmpStat;
    }
}
