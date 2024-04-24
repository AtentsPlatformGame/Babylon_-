using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public static HPbar Instance;
    [SerializeField] public Slider myHpSlider;

    public TextMeshProUGUI APstat;
    public TextMeshProUGUI MoveSpdstat;
    public TextMeshProUGUI AttackRangestat;


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
        if(pc != null)
        {
            HPbar.Instance.UpdateHpbar(pc.GetCurHP(),pc.GetMaxHP());
          //HPbar.Instance.UpdateStats(pc.GetAp(),pc.GetAttackRange(),pc.GetMoveSpeed());
        }
        
    }

    public void ChangeHpSlider()
    {
        myHpSlider.value = Mathf.Lerp(myHpSlider.value, pc.GetCurHP(),Time.deltaTime*2) ;
    }

   

}
