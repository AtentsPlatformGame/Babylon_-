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
    public UnityEvent<Transform> savePlayerPositionAct;
    Transform savePoint;
    #region Stage1 Jumper
    public void JumpToStage1()
    {
        SceneChanger.instance.GoToStage1();
    }

    public void JumpToStage1Gimic()
    {
        Debug.Log("������� ����");
        savePlayerPositionAct?.Invoke(savePoint);
        SceneChanger.instance.GoToStage1GimicRoom();
    }

    public void JumpToStage1Item()
    {
        savePlayerPositionAct?.Invoke(savePoint);
        SceneChanger.instance.GoToStage1ItemRoom();
    }

    public void JumpToStage1Boss()
    {
        savePlayerPositionAct?.Invoke(savePoint);
        SceneChanger.instance.GoToStage1Boss();
    }
    #endregion

    public void SetSavePoint(Transform _savePoint)
    {
        this.savePoint = _savePoint;
    }
}
