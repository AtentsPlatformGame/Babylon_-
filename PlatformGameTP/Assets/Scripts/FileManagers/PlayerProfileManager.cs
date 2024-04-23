using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using InventorySystem;

public class PlayerProfileManager : MonoBehaviour
{
    public UnityEvent<ItemStat> updatePlayerInvenAct;
    public InventorySlot_LNH[] playerItems = new InventorySlot_LNH[7];
    string savePath;
    public Transform inventory;
    public Transform StartPos;

    [SerializeField, Header("�÷��̾� �⺻ ����")] PlayerStatData playerStatData;
    PlayerController player;
    GoldManager goldManager;
    ItemStat[] saveItems = new ItemStat[7];
    ItemStat[] loadItems = new ItemStat[7];


    // load, load�� �� ���� ����� �÷��̾��� ���� ü�°� �κ��丮 ������ �о�� �÷��̾ �ݿ��Ѵ�. �ݿ��� �� ������ ��� Calculate�� �Ἥ ������ �����Ѵ�.
    private void Awake()
    {
        if (SceneChanger.instance != null)
        {
            SceneChanger.instance.savePlayerProfileAct.AddListener(SavePlayerInvenProfile);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerProfile();
    }

    public void SavePlayerInvenProfile() // save : save�� �� ���� ���� �÷��̾��� ü�°�, �÷��̾��� �κ��丮 ������ �����Ѵ�.
    {
        player = FindObjectOfType<PlayerController>();
        goldManager = FindObjectOfType<GoldManager>();
        int _gold = goldManager.GetPlayerGold();
        float _curhp = player.GetCurHP(); // ���� ü���� ������
        for (int i = 0; i < 7; i++)
        {
            if (playerItems[i] != null)
            {
                saveItems[i] = playerItems[i].GetItemStat();
            }
        }


        PlayerInventoryProfile playerProfile = new PlayerInventoryProfile(_curhp, _gold, saveItems);
        //PlayerInventoryProfile playerProfile = new PlayerInventoryProfile(_curhp, saveItems);

        // �����͸� JSON �������� ����ȭ
        string json = JsonUtility.ToJson(playerProfile);
        // JSON ���Ϸ� ����
        savePath = SceneChanger.instance.filepath_playerProfile;
        File.WriteAllText(savePath, json);
    }

    void LoadPlayerProfile()
    {
        StartCoroutine(LoadingPlayerInvenProfile());
    }

    IEnumerator LoadingPlayerInvenProfile()
    {
        if(inventory != null)inventory.gameObject.SetActive(true);
        player = FindObjectOfType<PlayerController>();
        goldManager = FindObjectOfType<GoldManager>();
        player.gameObject.transform.position = StartPos.position;
        if(SceneChanger.instance != null) savePath = SceneChanger.instance.filepath_playerProfile;
        Debug.Log("�ε� ����");
        // JSON ���Ϸκ��� ������ �б�
        if (File.Exists(savePath))
        {
            Debug.Log("���� ����");
            string json = File.ReadAllText(savePath);

            // JSON�� ������ ������ ������ȭ
            PlayerInventoryProfile playerProfile = JsonUtility.FromJson<PlayerInventoryProfile>(json);

            if (player != null)
            {
                if (playerProfile.GetPlayerCurHP() == 0) player.Initialize(1);
                else player.Initialize(playerProfile.GetPlayerCurHP());
            }
            if (goldManager != null) goldManager.SetPlayerGold(playerProfile.GetPlayerCurGold());
            //SetGold();
            Debug.Log($"�о�� ���� ü�� : {playerProfile.GetPlayerCurHP()}");
            for (int i = 0; i < 7; i++)
            {
                if (playerProfile.GetItemProperty(i).ItemType != ITEMTYPE.NONE)
                {
                    loadItems[i] = playerProfile.GetItemProperty(i);
                    Debug.Log($"�о�� ������ ���� : {loadItems[i]}");
                }
            }

            Debug.Log("Character position loaded from " + savePath);
            yield return StartCoroutine(UpdatingPlayerInventory());
        }
        else
        {
            Debug.LogWarning("No saved at " + savePath);
            if (player != null && playerStatData != null)
                player.Initialize(playerStatData);
            goldManager.SetPlayerGold(0);
            inventory.gameObject.SetActive(false);
            yield return null;
        }

    }

    IEnumerator UpdatingPlayerInventory()
    {
        Debug.Log("������Ʈ ����");

        for (int i = 0; i < 7; i++)
        {
            if (loadItems[i].ItemType != ITEMTYPE.NONE)
            {
                updatePlayerInvenAct?.Invoke(loadItems[i]);
            }
        }
        inventory.gameObject.SetActive(false);
        yield return null;
    }

    [System.Serializable]
    public class PlayerInventoryProfile
    {
        public float playerCurHp;
        public int playerGold;
        public ItemStat[] savedInven = new ItemStat[7];

        public PlayerInventoryProfile(float _playerCurHp, int _playerGold, ItemStat[] _savedInven)
        {
            this.playerCurHp = _playerCurHp;
            this.playerGold = _playerGold;
            this.savedInven = _savedInven;
        }

        /*public PlayerInventoryProfile(float _playerCurHp,  ItemStat[] _savedInven)
        {
            this.playerCurHp = _playerCurHp;
            this.savedInven = _savedInven;
        }*/

        public float GetPlayerCurHP()
        {
            return this.playerCurHp;
        }

        public ItemStat GetItemProperty(int idx)
        {
            return this.savedInven[idx];
        }

        public int GetPlayerCurGold()
        {
            return this.playerGold;
        }
    }

}
