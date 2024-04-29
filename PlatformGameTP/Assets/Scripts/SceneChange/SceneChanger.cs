using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;
/*
 * �� ��ũ��Ʈ�� �� ���� ���� �̵��� ������ �ε����� ��ġ�� �� ������ �̵��ϰ��� �ϴ� ���� ������ ������ �ε������� �ش� ������ �̵���Ű�� ����
 * �Ʒ��� ������ �̵��ϴ� �Լ����� Ư�� Ʈ����(��ư,����ĳ��Ʈ �� ���)�� ���ε� ���� ���� ���ϴ� ������ �̵��� �� �ֵ���
 */
public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance = null;
    public string nextSceneName;
    public Transform savePoint;
    public UnityEvent sceneChangeAct;
    public UnityEvent savePlayerProfileAct;
    /*[SerializeField, Header("�÷��̾� �κ��丮 ���� ���� ���� ��ġ")] string filepath_playerProfile;
    [SerializeField, Header("��������1 ��ġ ���� ���� ���� ��ġ")] string filepath_stage1;
    [SerializeField, Header("��������2 ��ġ ���� ���� ���� ��ġ")] string filepath_stage2;*/
    public string filepath_playerProfile;
    public string filepath_stage1;
    public string filepath_stage2;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            filepath_playerProfile = "PlayerProfile.json";
            filepath_stage1 = "stage1Profile.json";
            filepath_stage2 = "stage2Profile.json";
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void GoToIntro()
    {
        nextSceneName = "Intro"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    #region Ʃ�丮��

    public void GoToTutorialStage()
    {
        if (File.Exists(filepath_playerProfile))
        {
            File.Delete(filepath_playerProfile);
        }
        if (File.Exists(filepath_stage1))
        {
            File.Delete(filepath_stage1);
        }
        if (File.Exists(filepath_stage2))
        {
            File.Delete(filepath_stage2);
        }

        nextSceneName = "TutorialStage"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    #endregion

    #region Stage1
    public void GoToStage1()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage1"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }
    public void GoToStage1Village()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage1Village"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    public void GoToStage1GimicRoom()
    {
        Debug.Log("�ν��Ͻ��� ��ͷ� ����");
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage1GimicRoom"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    public void GoToStage1ItemRoom()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage1ItemRoom"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    public void GoToStage1Boss()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage1Boss"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }
    #endregion

    #region Stage2
    public void GoToStage2()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage2"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    /*public void GoToStage2Village()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage2Village"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }
    public void GoToStage2GimicRoom()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage2GimicRoom"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    public void GoToStage2ItemRoom()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage2ItemRoom"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }

    public void GoToStage2MiddleBoss()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "Stage2MiddleBoss"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }
*/

    public void GoToLastBoss()
    {
        savePlayerProfileAct?.Invoke();
        nextSceneName = "LastBoss"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }
    #endregion

    public void GoToOuttro()
    {
        nextSceneName = "GoToOuttro"; // �ϴ� �����µ� ���� ��Ȯ�� �̸����� �ٲ����
        sceneChangeAct?.Invoke();
    }


}
