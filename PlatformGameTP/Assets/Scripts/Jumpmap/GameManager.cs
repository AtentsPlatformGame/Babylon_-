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
    [Header("�����ۺ��� ĵ����")][SerializeField] GameObject ItemGetCanvas;

    [Header("�˾� ĵ����")] public GameObject GKeyPopup;
    [Header("���� ���� ������Ʈ")] public GameObject Raypoint;
    [Header("�÷��̾�")] public Transform Player;
    [Header("��� ���� ��ġ")] public Transform GimicStart;
    [Header("������ �� ��ġ(�� ��)")] public Transform GimicEnd;

    public LayerMask TP;
    public bool isTpobject = false;
    public bool isPopup = false;
    public float GimicHp = 1; //�ִ� ��� ���� Ƚ��
    public float PlayerHp; // ���� ��� ���� Ƚ��

    // Start is called before the first frame update
    void Start()
    {
        PlayerHp = GimicHp;
        GKeyPopup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Tphit;
        if (Physics.Raycast(Raypoint.transform.position, Raypoint.transform.forward, out Tphit, Mathf.Infinity, TP) ||
            Physics.Raycast(Raypoint.transform.position + Vector3.forward * 0.5f, Raypoint.transform.forward, out Tphit, Mathf.Infinity, TP) ||
            Physics.Raycast(Raypoint.transform.position + Vector3.back * 0.5f, Raypoint.transform.forward, out Tphit, Mathf.Infinity, TP))
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

    public void GameStart()
    {
        TimeScaleOn();
        CanvasOff();
        Tp(Player, GimicStart);
        CountDownCanvas.gameObject.SetActive(true);
    }

    public void Exit()
    {
        TimeScaleOn();
        Canvas.SetActive(false);
    }


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

    //���� ClearGimic
    public void ClearGimic()
    {
        CountDownCanvas.gameObject.SetActive(false); // ī��Ʈ�ٿ��� ����������, �����.
        ClearCanvas.SetActive(true); //Clear UI //��ư�� ������ �������� �÷��̾ ��������.
        Hp();
        TimeScaleOff();
    }

    public void ItemGet()
    {
        ItemGetCanvas.SetActive(true);
        TimeScaleOff();
    }

    public void CountDownEnd()
    {
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

    public void FailAct() // Act -> ��ư�� ������ �� ������ �Լ�
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
    }

    public void itemGetAct()
    {
        ItemGetCanvas.SetActive(false);
        TimeScaleOn();
    }

}
