using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("���� ĵ����")][SerializeField] GameObject Canvas;
    [Header("���� ĵ����")][SerializeField] GameObject FailCanvas;
    [Header("���� ĵ����")][SerializeField] GameObject ClearCanvas;
    [Header("ī��Ʈ�ٿ� ĵ����")][SerializeField] GameObject CountDownCanvas;
    

    [Header("�˾� ĵ����")] public GameObject GKeyPopup;
    [Header("���� ���� ������Ʈ")] public GameObject Raypoint;
    [Header("�÷��̾�")] public Transform Player;
    [Header("��� ���� ��ġ")] public Transform GimicStart;
    [Header("������ �� ��ġ(�� ��)")] public Transform GimicEnd;

    [Header("���� �� �ڷ� �� ���� �ϴ� �ݶ��̴�")] public GameObject NotGoBack; //���� �� �ڷ� ���ư� �״� �� ����

    public LayerMask TP;
    public bool isTpobject = false;
    public bool isPopup = false;
    public float GimicHp = 1; //�ִ� ��� ���� Ƚ��
    public float PlayerHp; // ���� ��� ���� Ƚ��

    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
    public GameObject card6;
    public int cardCount;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHp = GimicHp;
        GKeyPopup.SetActive(false);
        NotGoBack.SetActive(false); 
        cardCount = 0;
        card1.SetActive(false);
        card2.SetActive(false);
        card3.SetActive(false);
        card4.SetActive(false);
        card5.SetActive(false);
        card6.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Tphit;
        if (Physics.Raycast(Raypoint.transform.position, Raypoint.transform.forward, out Tphit, Mathf.Infinity, TP) ||
            Physics.Raycast(Raypoint.transform.position + Vector3.forward * 0.1f, Raypoint.transform.forward, out Tphit, Mathf.Infinity, TP) ||
            Physics.Raycast(Raypoint.transform.position + Vector3.back * 0.1f, Raypoint.transform.forward, out Tphit, Mathf.Infinity, TP))
        {
            isTpobject = true;
            GKeyPopup.SetActive(true);
        }
        else
        {
            isTpobject = false;
            GKeyPopup.SetActive(false);
        }
        if (isTpobject && Input.GetKeyDown(KeyCode.G) && PlayerHp == 1)
        {
            TimeScaleOff();
            if (isPopup)
            {
                CanvasOff();
            }
            else
            {
                CanvasOn();  //���ӽ���
            }
        }
    }

    void Hp()
    {
        GimicHp--;
        if(GimicHp <= 0)
        {
            GimicHp = 0;
            PlayerHp = GimicHp;
        }
    }

    void CanvasOn()
    {
        Canvas.SetActive(true);  //��ư ĵ���� ��
        GKeyPopup.SetActive(false);  
        isPopup = true;
    }

    void CanvasOff()
    {
        Canvas.SetActive(false);
        isPopup = false;
    }

    //���� ��ư GameStart
    public void GameStart()
    {
        TimeScaleOn();
        CanvasOff();
        Tp(Player, GimicStart);
        CountDownCanvas.gameObject.SetActive(true);
        NotGoBack.SetActive(false);
    }

    //������ ��ư Exit
    public void Exit()
    {
        TimeScaleOn();
        CanvasOff();
    }

    //��� ���� FailGimic
    public void FailGimic()
    {
        FailCanvas.SetActive(true); //Fail UI //��ư�� ������ �������� �÷��̾ ��������.
        TimeScaleOff();
        Hp();
       
    }

    void TimeScaleOff()
    {
        Time.timeScale = 0.0f;
    }

    void TimeScaleOn()
    {
        Time.timeScale = 1.0f;
    }

    //��� ���� ClearGimic
    public void ClearGimic()
    {
        
        CountDownCanvas.gameObject.SetActive(false); // ī��Ʈ�ٿ��� ����������, �����.
        ClearCanvas.SetActive(true); //Clear UI //��ư�� ������ �������� �÷��̾ ��������.
        Hp();
        TimeScaleOff();
    }

    //������ ���� ItemGet
    public void ItemGet()
    {
        
        RandomCard();
        TimeScaleOff();
    }

    public void CountDownEnd()
    {
        NotGoBack.SetActive(true);
        CountDownCanvas.gameObject.SetActive(false); //ī��Ʈ�ٿ��� ��������.
    }

    public void Tp(Transform tpstart, Transform tpend)
    {
        StartCoroutine(Teleport(tpstart, tpend));
    }

    IEnumerator Teleport(Transform tpstart, Transform tpend)
    {
        yield return new WaitForSeconds(0.1f);
        tpstart.transform.position = tpend.transform.position;
    }

    //Fail ��ư�� ������ �������� ��������.
    public void FailAct()
    {
        FailCanvas.SetActive(false);
        Tp(Player, GimicEnd);
        TimeScaleOn();
    }

    //���� ��ư�� ������ �������� ��������.
    public void ClearAct()
    {
        ClearCanvas.SetActive(false);
        Tp(Player, GimicEnd);
        TimeScaleOn();
        NotGoBack.SetActive(false );
    }

    //�� ��ư�� ������ �ð��� �帥��.
    public void itemGetAct()
    {
        
        
        TimeScaleOn();
    }

    public void DieFalling()
    {
        PlayerHp = 0.0f;
        GimicHp = 0.0f; 
    }
  
    public void RandomCard()
    {
        
        int RndcardNum = Random.Range(1, 7);
        if (RndcardNum == 1 && cardCount == 0)
        {
            card1.SetActive(true);
            cardCount++;
        }
        if (RndcardNum == 2 && cardCount == 0)
        {
            card2.SetActive(true);
            cardCount++;
        }
        if (RndcardNum == 3 && cardCount == 0)
        {
            card3.SetActive(true);
            cardCount++;
        }
        if (RndcardNum == 4 && cardCount == 0)
        {
            card4.SetActive(true);
            cardCount++;
        }
        if (RndcardNum == 5 && cardCount == 0)
        {
            card5.SetActive(true);
            cardCount++;
        }
        if (RndcardNum == 6 && cardCount == 0)
        {
            card6.SetActive(true);
            cardCount++;
        }
    }

}
