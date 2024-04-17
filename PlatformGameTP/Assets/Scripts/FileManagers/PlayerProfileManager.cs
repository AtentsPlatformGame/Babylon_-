using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class PlayerProfileManager : MonoBehaviour
{
    public ItemProperty[] playerItems = new ItemProperty[7];
    public string savePath;
    public UnityEvent<ItemStat> updatePlayerInvenAct;

    PlayerController player;
    ItemProperty[] saveItems = new ItemProperty[7];
    ItemProperty[] loadItems = new ItemProperty[7];


    // load, load�� �� ���� ����� �÷��̾��� ���� ü�°� �κ��丮 ������ �о�� �÷��̾ �ݿ��Ѵ�. �ݿ��� �� ������ ��� Calculate�� �Ἥ ������ �����Ѵ�.
    private void Awake()
    {
        if (SceneChanger.instance != null)
        {
            SceneChanger.instance.savePlayerProfileAct.AddListener(SavePlayerInvenProfile);
        }
        LoadPlayerInvenProfile();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SavePlayerInvenProfile() // save : save�� �� ���� ���� �÷��̾��� ü�°�, �÷��̾��� �κ��丮 ������ �����Ѵ�.
    {
        player = FindObjectOfType<PlayerController>();
        float _curhp = player.GetCurHP(); // ���� ü���� ������
        for (int i = 0; i < 7; i++)
        {
            if (playerItems[i] != null)
            {
                saveItems[i] = playerItems[i];
            }
        }

        PlayerInventoryProfile playerProfile = new PlayerInventoryProfile(_curhp, saveItems);

        // �����͸� JSON �������� ����ȭ
        string json = JsonUtility.ToJson(playerProfile);
        // JSON ���Ϸ� ����
        File.WriteAllText(savePath, json);
    }

    void LoadPlayerInvenProfile()
    {
        player = FindObjectOfType<PlayerController>();
        // JSON ���Ϸκ��� ������ �б�
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            // JSON�� ������ ������ ������ȭ
            PlayerInventoryProfile playerProfile = JsonUtility.FromJson<PlayerInventoryProfile>(json);

            // ĳ���� ��ġ ����
            if (player != null) player.Initialize(playerProfile.GetPlayerCurHP());
            for(int i = 0; i < 7; i++)
            {
                if (playerProfile.GetItemProperty(i) != null)
                {
                    loadItems[i] = playerProfile.GetItemProperty(i);
                    updatePlayerInvenAct?.Invoke(loadItems[i].GetItemStat());
                }
            }

            Debug.Log("Character position loaded from " + savePath);
        }
        else
        {
            Debug.LogWarning("No saved character position found at " + savePath);
        }
    }

    [System.Serializable]
    public class PlayerInventoryProfile
    {
        float playerCurHp;
        ItemProperty[] savedInven = new ItemProperty[7];

        public PlayerInventoryProfile(float _playerCurHp, ItemProperty[] _savedInven)
        {
            this.playerCurHp = _playerCurHp;
            this.savedInven = _savedInven;
        }

        public float GetPlayerCurHP()
        {
            return this.playerCurHp;
        }

        public ItemProperty GetItemProperty(int idx)
        {
            return this.savedInven[idx];
        }
    }

}
