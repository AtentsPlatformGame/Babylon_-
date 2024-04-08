using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Savethis : MonoBehaviour
{
    public string saveFileName = "positionData.json"; // ������ JSON ���� �̸�

    private void SavePositionToJson()
    {
        // ������Ʈ�� ��ġ�� ������
        Vector3 position = transform.position;

        // ��ġ ������ JSON �������� ����
        string jsonData = JsonUtility.ToJson(position);

        // JSON ���Ϸ� ����
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Position data saved to: " + filePath);
    }
}
