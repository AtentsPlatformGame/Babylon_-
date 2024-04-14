using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternCanvas : MonoBehaviour
{
    [SerializeField, Header("1�� ���� ���� �̹���")] Transform boss1PatternImg;
    [SerializeField, Header("2�� ���� ���� �̹���")] Transform boss2PatternImg;

    public void TurnOnboss1PatternImg()
    {
        boss1PatternImg.gameObject.SetActive(true);
    }

    public void TurnOffboss1PatternImg()
    {
        boss1PatternImg.gameObject.SetActive(false);
    }

    public void TurnOnboss2PatternImg()
    {
        boss2PatternImg.gameObject.SetActive(true);
    }

    public void TurnOffboss2PatternImg()
    {
        boss2PatternImg.gameObject.SetActive(false);
    }
}
