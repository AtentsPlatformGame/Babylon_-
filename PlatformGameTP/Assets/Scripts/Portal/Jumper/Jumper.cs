using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Jumper : MonoBehaviour
{
    /*
     �������� 1���� �ٸ� Ư�������� �̵��ϴ� �Լ����� ���ε��ϱ� ���ϵ��� ������� ��ũ��Ʈ
     savePlayerPositionAct�� ��ġ �����ϴ� ������Ʈ�� �ִ� CharacterSave ��ũ��Ʈ���� Save�Լ��� ���ε��ؼ� ���� ��
     */
    //public UnityEvent<Transform> savePlayerPositionAct;
    public UnityEvent<Transform> JumpToDestinationAct;
    public Transform player;
    [Header("���� ��ġ"), Space(5)] public Transform villagePos;
    [Header("�����۹� ��ġ"), Space(5)] public Transform itemRoomPos;
    [Header("��͹� ��ġ"), Space(5)] public Transform gimicRoomPos;
    [Header("������ ��ġ"), Space(5)] public Transform bossRoomPos;
    Transform savePoint;

    public void SetSavePoint(Transform _savePoint)
    {
        this.savePoint = _savePoint;
    }

    IEnumerator JumpAfterFade(Transform _player)
    {
        yield return null;
    }

    public void JumpToMainStage()
    {
    }
    public void JumpToVillage()
    {
    }
    public void JumpToItemRoom()
    {
    }
    public void JumpToGimicRoom()
    {
    }
    public void JumpToBoss()
    {
    }
}
