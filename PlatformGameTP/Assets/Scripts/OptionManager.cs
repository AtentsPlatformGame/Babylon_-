using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [Header("�������� ��ư")][SerializeField] GameObject MainCanvas;
    [Header("�������� ��ư")][SerializeField] GameObject GameCanvas;
    [SerializeField] Transform MyOptions;
    PlayerController player;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {

    }

    public void GoMain()
    {
        SceneChanger.instance.GoToMain();
    }

    public void GoGame()
    {
        PopDown(MyOptions);
        player.ControllPlayerAttack(true);
    }

    public void PopDown(Transform popup)
    {
        popup.gameObject.SetActive(false);
    }

}
