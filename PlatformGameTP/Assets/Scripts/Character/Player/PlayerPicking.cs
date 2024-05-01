using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerPicking : MonoBehaviour
{
    
    [Header("���� ��� ������Ʈ")]
    [Header("���� ���� ��� ��ġ �̹��� ")]public Transform attackSpellPointImg;
    [Header("���� ���� ��� ��ġ �̹���")] public Transform buffSpellPoinImg;
    [Header("���� ��� �����Ÿ� �̹���")]public Transform spellRangeImg;
    [Header("���� ���� ��� �����Ÿ�")]public float spellMaxRange = 6.0f;

    public LayerMask layermask;
    public UnityEvent attackAct; // PlayerController Attack()
    public UnityEvent<bool> spellReadyAct; // ���� ��� �غ� 
    public UnityEvent<Vector3> useSpellAct; // ���� ���
    public UnityEvent useBuffSpellAct;
    public AudioSource spellAudioSource;
    PlayerController playerController;
    public AudioClip attackspellClip;
    public AudioClip buffspellClip;
    public AudioClip canclespellClip;

    Vector3 pos;
    Ray ray;
    RaycastHit hit;
    Vector3 newHitPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        attackSpellPointImg.gameObject.SetActive(false);
        buffSpellPoinImg.gameObject.SetActive(false);
        spellRangeImg.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isAlive() == false || playerController == null || !playerController.GetMovePossible()) return;

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
                if (playerController.GetCurrentSpell() == null) return;
                Debug.Log("���� ��� �غ�");
                spellReadyAct?.Invoke(true);
                // �̶����� �����Ÿ��� ǥ���ؾ���
                if (playerController.GetCurrentSpell().gameObject.tag == "AttackSpell") // ����ϰ����ϴ� ������ Attack�� ��
                {
                    SpellObjectEnabled(attackSpellPointImg, spellRangeImg, true);
                    if (spellAudioSource != null)
                    {
                        spellAudioSource.clip = attackspellClip;
                        spellAudioSource.PlayOneShot(attackspellClip);
                    }
                    if (spellAudioSource.isPlaying) Debug.Log("�����ֹ� ���� ȿ���� ");
                }
                else // ����ϰ����ϴ� ������ Buff�� ��
                {
                    SpellObjectEnabled(buffSpellPoinImg, spellRangeImg, true);
                    if (spellAudioSource != null)
                    {
                        spellAudioSource.clip = buffspellClip;
                        spellAudioSource.PlayOneShot(buffspellClip);
                    }
                    if (spellAudioSource.isPlaying) Debug.Log("�������� ȿ����");
                }
                
            }
        }
        else // ���� ��� �غ� ��
        {
            if (playerController.GetCurrentSpell().gameObject.tag == "AttackSpell") // ����ϰ����ϴ� ������ Attack�� ��
            {
                // 2D ��Ʈ�� �� ���� �׸���
                DrawSpell(attackSpellPointImg);
            }

            if (Input.GetMouseButtonDown(0)) // ���� ���
            {

                Debug.Log("���� ���");
                //SpellCanvasEnabled(false);
                //playerController.ResetSpellTrigger();
                //SpellObjectEnabled(spellPointImg, spellRangeImg, false);
                if (playerController.GetCurrentSpell().gameObject.tag == "AttackSpell") // ����ϰ����ϴ� ������ Attack�� ��
                {
                    SpellObjectEnabled(attackSpellPointImg, spellRangeImg, false);
                    
                }
                else // ����ϰ����ϴ� ������ Buff�� ��
                {
                    SpellObjectEnabled(buffSpellPoinImg, spellRangeImg, false);
                    //useBuffSpellAct?.Invoke();
                }
                useSpellAct?.Invoke(newHitPoint);
                spellReadyAct?.Invoke(false);
            }
            else if (Input.GetMouseButtonDown(1)) // ���� ��� ���
            {
                Debug.Log("���� ��� ���");
                spellReadyAct?.Invoke(false);
                if (spellAudioSource != null)
                {
                    spellAudioSource.clip = canclespellClip;
                    spellAudioSource.PlayOneShot(canclespellClip);
                }
                if (spellAudioSource.isPlaying) Debug.Log("���� ���");
                //SpellCanvasEnabled(false);
                //SpellObjectEnabled(spellPointImg, spellRangeImg, false);
                if (playerController.GetCurrentSpell().gameObject.tag == "AttackSpell") // ����ϰ����ϴ� ������ Attack�� ��
                {
                    SpellObjectEnabled(attackSpellPointImg, spellRangeImg, false);
                }
                else // ����ϰ����ϴ� ������ Buff�� ��
                {
                    SpellObjectEnabled(buffSpellPoinImg, spellRangeImg, false);
                }
            }
            
        }
        
    }

    
    void SpellObjectEnabled(Transform _spellPointImg,Transform _spellRangeImg,bool state)
    {
        _spellPointImg.gameObject.SetActive(state);
        _spellRangeImg.gameObject.SetActive(state);
    }


    void DrawSpell(Transform _spellPointImg)
    {
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            pos = hit.point;
        }

        Vector3 hitDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);

        distance = Mathf.Min(distance, spellMaxRange);

        newHitPoint = transform.position + hitDir * distance;
        // 2D�϶�
        if (playerController.GetControllType())
        {
            // _spellPointImg.transform.position = new Vector3(0, newHitPoint.y + 0.1f, newHitPoint.z);
            _spellPointImg.transform.position = new Vector3(0, playerController.gameObject.transform.position.y + 0.1f, newHitPoint.z);
        }
        else
        {
            //_spellPointImg.transform.position = new Vector3(newHitPoint.x, newHitPoint.y + 0.1f, newHitPoint.z);
            _spellPointImg.transform.position = new Vector3(newHitPoint.x, playerController.gameObject.transform.position.y + 0.1f, newHitPoint.z);
        }
        
    }

   


}
