using InventorySampleScene;
using LGH;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class Tp : MonoBehaviour
{
    [SerializeField] Transform tp;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject countDown;
    [SerializeField] GameObject Canvas;
    public GameObject GKeyPopup;

    public Life life;
    public LayerMask mask;
    public LayerMask TP;

    public bool isTpobject = false;
    public bool isPopup = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        RaycastHit Tphit;
        if (Physics.Raycast(transform.position, transform.forward, out Tphit, Mathf.Infinity, TP))
        {
            isTpobject = true;
            GKeyPopup.SetActive(true);
        }
        else
        {
            isTpobject = false;
            GKeyPopup.SetActive(false);
        }
        if (isTpobject && Input.GetKeyDown(KeyCode.G))
        {
            if (isPopup)
            {
                OutTp();
            }
            else
            {
                InTp();
            }
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        //���̾��ũ ����
        if ((mask & 1 << other.gameObject.layer) != 0)
        {
            if(life != null && life.GetLife() > 0)
            // �������� Ȯ���ؼ� �װ� 0�� �ƴϸ� ������ �Ѵ�.
                StartCoroutine(Teleport());
        }

    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(0.1f);
        Player.transform.position = new Vector3(
            tp.transform.position.x,
            tp.transform.position.y,
            tp.transform.position.z);

        //if (countDown != null) //���࿡ null �� �ƴ϶��
        //countDown.SetActive(true); // ī��Ʈ�ٿ� ����

        if (countDown.activeSelf == true)
        {
            countDown.SetActive(false);
        }
        else if (countDown.activeSelf == false)
        {
            countDown.SetActive(true);
        }
        //���࿡ ������Ʈ�� ���������� Ű��, ���������� ����.
        //
    }

    void InTp()
    {
        Canvas.SetActive(true);
        GKeyPopup.SetActive(false);
        isPopup = true;
    }

    void OutTp()
    {
        Canvas.SetActive(false);
        isPopup = false;
    }

}


