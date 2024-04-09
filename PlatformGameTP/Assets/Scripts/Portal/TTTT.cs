using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TTTT : MonoBehaviour
{
    
    // ĳ������ Transform ������Ʈ
    public Transform characterTransform;

    // JSON ���Ϸ� ������ ���
    public string savePath = "character_position.json";
    CharacterPositionData positionData;

    private void Awake()
    {
        ReadDataInfo();
    }
    void Start()
    {
        LoadCharacterPosition();
    }

    // Update is called once per frame
    void LoadCharacterPosition()
    {
        // JSON ���Ϸκ��� ������ �б�
        if (File.Exists(savePath))
        {
            // ĳ���� ��ġ ����
            characterTransform.position = positionData.position;
            characterTransform.rotation = Quaternion.Euler(positionData.rotation);

            Debug.Log("����� �÷��̾��� ��ġ�� �ҷ���" + savePath);
        }
        else
        {
            Debug.LogWarning("No saved character position found at " + savePath);
        }
    }

    void ReadDataInfo()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            // JSON�� ������ ������ ������ȭ
            positionData = JsonUtility.FromJson<CharacterPositionData>(json);
        }
    }
}
