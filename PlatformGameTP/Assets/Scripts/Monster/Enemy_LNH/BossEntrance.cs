using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossEntrance : MonoBehaviour
{
    [SerializeField, Header("�Ա� ����")] Transform entranceObject;
    [SerializeField, Header("GŰ �˾�â")] Transform gKeyPopup;
    [SerializeField, Header("���� ���� ����� ĵ����")] Transform entranceCanvas;
    [SerializeField, Header("�÷��̾�")] Transform player;
    [SerializeField, Header("�÷��̾� �ڷ���Ʈ ��Ű��")] Transform warpPoint;
    [SerializeField, Header("1�� ����")] Transform boss1;

    [Header("�÷��̾� ������ �����ϴ� �Լ�")] public UnityEvent playerMoveFalse;
    [Header("�÷��̾� �����̰� �ϴ� �Լ�")] public UnityEvent playerMoveTrue;

    public LayerMask playerMask;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer & playerMask) != 0)
        {
            if(gKeyPopup != null)
            {
                gKeyPopup.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & playerMask) != 0)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if(entranceCanvas != null)
                {
                    playerMoveFalse?.Invoke();
                    entranceCanvas.gameObject.SetActive(true);
                    gKeyPopup.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & playerMask) != 0)
        {
            if (gKeyPopup != null)
            {
                gKeyPopup.gameObject.SetActive(false);
            }
        }
    }

    public void EntranceBossRoom()
    {
        playerMoveTrue?.Invoke();
        player.position = warpPoint.position;
        boss1.gameObject.SetActive(true);
        entranceObject.gameObject.SetActive(false);
    }

    public void ExitCanvas()
    {
        playerMoveTrue?.Invoke();
        entranceCanvas.gameObject.SetActive(false);
    }
}
