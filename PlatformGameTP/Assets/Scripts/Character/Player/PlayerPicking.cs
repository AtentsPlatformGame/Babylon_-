using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerPicking : MonoBehaviour
{
    [Header("���� ��� VFX")]
    public Canvas spellCanvas;
    public Image spellRange;
    public float spellMaxRange = 6.0f;

    public LayerMask layermask;
    public UnityEvent attackAct; // PlayerController Attack()
    public UnityEvent<bool> spellReadyAct; // ���� ��� �غ� 
    public UnityEvent useSpellAct; // ���� ���
    PlayerController playerController;

    Vector3 pos;
    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        spellCanvas.enabled = false;
        spellRange.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isAlive() == false || playerController == null) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerController.isSpellReady == false) // ���� ��� �غ� ��
        {
            
            if (Input.GetMouseButtonDown(0)) // ���� �ȵ� ���߿� �����ҵ� unityevents�� ����غ� ����, �ٵ� isAlive�� ��� ���� �𸣰���
            {
                attackAct?.Invoke();
                Debug.Log("Click");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("���� ��� �غ�");
                spellReadyAct?.Invoke(true);
                // �̶����� �����Ÿ��� ǥ���ؾ���
                SpellCanvasEnabled(true);
            }
        }
        else // ���� ��� �غ� ��
        {

            DrawSpellPoint();

            if (Input.GetMouseButtonDown(0)) // ���� ���
            {
                useSpellAct?.Invoke();
                spellReadyAct?.Invoke(false);
                Debug.Log("���� ���");
                SpellCanvasEnabled(false);
                //playerController.ResetSpellTrigger();
            }
            else if (Input.GetMouseButtonDown(1)) // ���� ��� ���
            {
                Debug.Log("���� ��� ���");
                spellReadyAct?.Invoke(false);
                SpellCanvasEnabled(false);
            }
            
        }
        
    }

    void SpellCanvasEnabled(bool state)
    {
        spellCanvas.enabled = state;
        spellRange.enabled = state;
    }

    void DrawSpellPoint()
    {
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            pos = hit.point;
        }

        Vector3 hitDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);

        distance = Mathf.Min(distance, spellMaxRange);

        Vector3 newHitPoint = transform.position + hitDir * distance;
        spellCanvas.transform.position = new Vector3(0,newHitPoint.y, newHitPoint.z);



    }

   
}
