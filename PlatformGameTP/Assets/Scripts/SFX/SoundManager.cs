using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager Instance;
    public GameObject Op;
    public Slider volumeSlider;
    public Slider bgVolumeSlider;
    public UnityEvent<Slider> SetVolumeAct; // ȿ���� ���� �Լ� << ����Ŵ����� �ϴ°� �ƴ϶� ���� ��ü���� ��
    public UnityEvent<Slider> SetBGVolumeAct; // ����� ���� �Լ� << ����Ŵ����� �ϴ°� �ƴ϶� ��ü�� ��
    public float soundValue = 1.0f;
    public float bgSoundValue = 1.0f;
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public AudioMixer mixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    
    public void SetBGSVolume() // �����
    {
        bgSoundValue = bgVolumeSlider.value;
    }
    public void SetBGSVolume(float value) // �����
    {
        bgSoundValue = value;
    }
    public void SetVolume() // ȿ����
    {
        soundValue = volumeSlider.value;
    }
    public void SetVolume(float value) // ȿ����
    {
        soundValue = value;
    }
    public void SetBGVolumeSlider(Slider _slider)
    {
        bgVolumeSlider = _slider;
        SetBGVolumeAct?.Invoke(bgVolumeSlider);
    }

    public void SetSFXVolumeSlider(Slider _slider)
    {
        volumeSlider = _slider;
        SetVolumeAct?.Invoke(volumeSlider);
        Debug.Log("SetSFXSlider");
    }
}
