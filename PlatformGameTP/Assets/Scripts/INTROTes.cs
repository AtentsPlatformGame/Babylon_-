using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class INTROTes : MonoBehaviour
{
    public TMP_Text text1;

    string dialog1;

    // Start is called before the first frame update
    void Start()
    {
        dialog1 = "���� ���ϵ� ���� ������ �־���.";
        StartCoroutine(Typing(dialog1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Typing(string talk)
    {
        text1.text = null;
        for(int i = 0; i < talk.Length; i++)
        {
            text1.text += talk[i];

            yield return new WaitForSeconds(0.1f);
        }
    }
}
