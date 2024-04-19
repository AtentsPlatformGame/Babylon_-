using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] Transform gKeyPopUp; // GŰ �˾�â ������Ʈ
    [SerializeField] Canvas portalCanvas; // ��Ż ĵ����
    [SerializeField] Button portalButton; // ��Ż ĵ���� �ȿ� �ִ� ��ư

    public UnityEvent jumpToTargetSceneAct; // ��� ������ ���� ���ε� �ؼ� �����, Jumper����
    public LayerMask playerLayerMask; // Ʈ���� ������ ���� ���̾� ����ũ
    public Transform savePoint; // Ư���濡 ���� ���ƿ� �� �̵��� ��ġ

    Jumper jumper;
    private void Start()
    {
        jumper = FindObjectOfType<Jumper>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer & playerLayerMask) != 0)
        {
            if (gKeyPopUp != null) gKeyPopUp.gameObject.SetActive(true); // Ʈ���ſ� �÷��̾ ������ GŰ �˾��� Ű��
            jumper.SetSavePoint(savePoint); // Ư�� ��ġ�� ���̺�����Ʈ�� ������
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & playerLayerMask) != 0)
        {
            if (gKeyPopUp.gameObject.activeSelf && Input.GetKeyDown(KeyCode.G))
            {
                if (portalCanvas != null)portalCanvas.gameObject.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & playerLayerMask) != 0)
        {
            if (gKeyPopUp != null && portalCanvas != null)
            {
                gKeyPopUp.gameObject.SetActive(false);
                portalCanvas.gameObject.SetActive(false);
            }
        }
    }

    
}
