using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldManager : Inventory
{
    
    
    public TextMeshProUGUI Owngold;
    // Start is called before the first frame update
    void Start()
    {
        PlayerGold = 0;
        Owngold = GetComponent<TextMeshProUGUI>();
        Owngold.text = PlayerGold.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        CountGold();
        Owngold.text = PlayerGold.ToString();
    }

    //CountGold => �ʵ� ��� ȹ�淮�� ������ ��差 ����Ͽ� �ݿ�
    public void CountGold()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
                PlayerGold++;
        }
    }
}
