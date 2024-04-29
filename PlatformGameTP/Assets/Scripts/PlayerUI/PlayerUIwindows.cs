using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIwindows : MonoBehaviour
{
    public static PlayerUIwindows Instance;
    [SerializeField] public Slider myHpSlider;

    public TextMeshProUGUI APstat;
    public TextMeshProUGUI MoveSpdstat;

    public Transform player;
    PlayerController pc;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pc = player.GetComponent<PlayerController>();
        
    }

    private void Update()
    {
        //�̺�Ʈ �ڵ鷯 ����
        UpdateUI();
    }

    private void OnDisable()
    {
        // �̺�Ʈ �ڵ鷯 ����
        UpdateUI();
    }

    public void UpdateHpbar(float curHp, float maxHp)
    {
        myHpSlider.value = (float)curHp / maxHp;//ü�� ������ ����
    }


    public void UpdateUI()
    {
        if (pc != null)
        {
            float ap = pc.GetAp();
            float spd = pc.GetMoveSpeed();

            APstat.text = ap.ToString();
            MoveSpdstat.text = spd.ToString();
        }
    }

    void UpdateHP()
    {
        PlayerUIwindows.Instance.UpdateHpbar(pc.GetCurHP(),pc.GetMaxHP());
    }

    public void ChangeHpSlider()
    {
        float curHP = pc.GetCurHP();
        float maxHP = pc.GetMaxHP();
        //���� HP�� �ִ�HP ������ ���� ���
        float ratio = curHP / maxHP;

        myHpSlider.value = Mathf.Lerp(myHpSlider.value,ratio,Time.deltaTime*3);
    }

   

}
