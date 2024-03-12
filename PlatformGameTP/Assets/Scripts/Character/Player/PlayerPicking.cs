using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerPicking : MonoBehaviour
{
    
    [Header("���� ��� ������Ʈ")]
    [Header("���� ��� ��ġ �̹��� ")]public Transform spellPointImg;
    [Header("���� ��� �����Ÿ� �̹���")]public Transform spellRangeImg;
    [Header("���� ���� ��� �����Ÿ�")]public float spellMaxRange = 6.0f;

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

        spellPointImg.gameObject.SetActive(false);
        spellRangeImg.gameObject.SetActive(false);
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
                //SpellCanvasEnabled(true);
                SpellObjectEnabled(true);
            }
        }
        else // ���� ��� �غ� ��
        {

            //DrawSpellPoint();
            DrawSpell();

            if (Input.GetMouseButtonDown(0)) // ���� ���
            {
                useSpellAct?.Invoke();
                
                Debug.Log("���� ���");
                //SpellCanvasEnabled(false);
                //playerController.ResetSpellTrigger();
                SpellObjectEnabled(false);
                spellReadyAct?.Invoke(false);
            }
            else if (Input.GetMouseButtonDown(1)) // ���� ��� ���
            {
                Debug.Log("���� ��� ���");
                spellReadyAct?.Invoke(false);
                //SpellCanvasEnabled(false);
                SpellObjectEnabled(false);
            }
            
        }
        
    }

    
    void SpellObjectEnabled(bool state)
    {
        spellPointImg.gameObject.SetActive(state);
        spellRangeImg.gameObject.SetActive(state);
    }


    void DrawSpell()
    {
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            pos = hit.point;
        }

        Vector3 hitDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);

        distance = Mathf.Min(distance, spellMaxRange);

        Vector3 newHitPoint = transform.position + hitDir * distance;
        spellPointImg.transform.position = new Vector3(0, newHitPoint.y + 0.1f, newHitPoint.z);
    }

   
}
