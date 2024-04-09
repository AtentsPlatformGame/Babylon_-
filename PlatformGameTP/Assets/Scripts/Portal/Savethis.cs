using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Savethis : MonoBehaviour
{
    // ĳ������ Transform ������Ʈ
    public LayerMask Player;
    public float raycastDistance = 100;
    public Transform characterTransform;
    public string savePath = "character_position.json";

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� ���̾ Ȯ��
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SaveCharacterPosition();
            Debug.Log("���� ��ġ�� �÷��̾ �����");
        }
    }
    void SaveCharacterPosition()
    {
        // ĳ������ ��ġ ������ ���� ������ ���� ����
        CharacterPositionData positionData = new CharacterPositionData();
        positionData.position = characterTransform.position;
        positionData.rotation = characterTransform.rotation.eulerAngles;

        // �����͸� JSON �������� ����ȭ
        string json = JsonUtility.ToJson(positionData);

        // JSON ���Ϸ� ����
        File.WriteAllText(savePath, json);
    }


    // ĳ���� ��ġ ������ ���� ������ ����
    [System.Serializable]
    public class CharacterPositionData
    {
        public Vector3 position;
        public Vector3 rotation;
    }
}
