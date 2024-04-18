using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public class CharaterSave : MonoBehaviour
{
    // ĳ������ Transform ������Ʈ
    public Transform characterTransform;
    // JSON ���Ϸ� ������ ���
    public string savePath;
    //public Vector3 startingPosition = new Vector3(0f, 1f, 0f);
    public Transform startPoint;

    void Start()
    {
        // �÷��̾� ������Ʈ�� ��ġ�� ���� ��ġ�� ����
        LoadCharacterPosition();
    }

    public void SaveCharacterPosition(Transform savePoint)
    {
        // ĳ������ ��ġ ������ ���� ������ ���� ����
        CharacterPositionData positionData = new CharacterPositionData();
        positionData.position = savePoint.position;
        positionData.rotation = characterTransform.rotation.eulerAngles;

        // �����͸� JSON �������� ����ȭ
        string json = JsonUtility.ToJson(positionData);

        // JSON ���Ϸ� ����
        File.WriteAllText(savePath, json);
    }

    void LoadCharacterPosition()
    {
        // JSON ���Ϸκ��� ������ �б�
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            // JSON�� ������ ������ ������ȭ
            CharacterPositionData positionData = JsonUtility.FromJson<CharacterPositionData>(json);

            // ĳ���� ��ġ ����
            characterTransform.position = positionData.position;
            characterTransform.rotation = Quaternion.Euler(positionData.rotation);

            Debug.Log("Character position loaded from " + savePath);
        }
        else
        {
            Debug.LogWarning("No saved character position found at " + savePath);
            characterTransform.position = startPoint.position;
        }
    }



    /*// Ű���� �Է����� ���� �� �ҷ����� ����
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveCharacterPosition();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadCharacterPosition();
        }
    }*/
}

// ĳ���� ��ġ ������ ���� ������ ����
[System.Serializable]
public class CharacterPositionData
{
    public Vector3 position;
    public Vector3 rotation;
}
