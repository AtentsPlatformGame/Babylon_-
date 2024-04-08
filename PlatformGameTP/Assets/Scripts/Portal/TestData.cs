using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TestData : MonoBehaviour
{
    public string loadFileName = "positionData.json"; // �ҷ��� JSON ���� �̸�

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPositionFromJson();
    }

    private void LoadPositionFromJson()
    {
        // JSON ���� ���
        string filePath = Path.Combine(Application.persistentDataPath, loadFileName);

        if (File.Exists(filePath))
        {
            // JSON ���Ϸκ��� ������ �б�
            string jsonData = File.ReadAllText(filePath);

            // JSON �����͸� ���ͷ� ��ȯ
            Vector3 position = JsonUtility.FromJson<Vector3>(jsonData);

            // ������Ʈ ��ġ �̵�
            transform.position = position;

            Debug.Log("Position loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("Position data file not found at: " + filePath);
        }
    }
}
