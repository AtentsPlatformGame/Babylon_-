using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savethis : MonoBehaviour
{
    public Transform portalPosition;
    void Start()
    {
        // ù ��° ������ ��Ż ��ġ�� ����
        PlayerPrefs.SetFloat("PortalX", portalPosition.position.x);
        PlayerPrefs.SetFloat("PortalY", portalPosition.position.y);
        PlayerPrefs.SetFloat("PortalZ", portalPosition.position.z);
    }
}
