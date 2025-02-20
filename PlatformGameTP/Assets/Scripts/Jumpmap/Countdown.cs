using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class Countdown : MonoBehaviour
{
    [Header("시간종료 레이 오브젝트")] public GameObject countdownpoint;
    float originTime;
    [SerializeField] float setTime = 60.0f;
    [SerializeField] TMPro.TMP_Text countdownText;
    public UnityEvent Fail;

    public bool countdown = false;
    string minutesS = "";
    string secondsS = "";
    int minute;
    float second;
    // Start is called before the first frame update
    void OnEnable()
    {
        originTime = setTime;
        countdownText.text = setTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (setTime > 0.0f)
            setTime -= Time.deltaTime;
        else if (setTime <= 0.0f)
        {
            Fail?.Invoke();
            setTime = originTime;
            this.gameObject.SetActive(false);
        }
     
        minute = (int)(setTime / 60.0f);
        second = setTime % 60.0f;

        minutesS = minute.ToString();
        secondsS = Mathf.Round(second).ToString();
        countdownText.text = minutesS + " : " + secondsS;
    }

}

