using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using static TestData;

public class Portalllll1 : MonoBehaviour
{
    // ĳ������ Transform ������Ʈ
    public LayerMask Player;
    public float raycastDistance = 100;
    private SceneFadeInOut FadeManager;
    private Vector3 savedPosition;

    void Start()
    {
        FadeManager = FindObjectOfType<SceneFadeInOut>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, Player))
        {
            savedPosition = transform.position;
            FadeManager.FadeOutAndLoadScene("TutorialStage");
        }
    }
}