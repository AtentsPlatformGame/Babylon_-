using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager Instance;
    public Slider volumeSlider;
    public Slider bgVolumeSlider;
    public UnityEvent<float> SetVolumeAct; // ȿ���� ���� �Լ� << ����Ŵ����� �ϴ°� �ƴ϶� ���� ��ü���� ��
    public UnityEvent<float> SetBGVolumeAct; // ����� ���� �Լ� << ����Ŵ����� �ϴ°� �ƴ϶� ��ü�� ��
    public float soundValue = 1.0f; // ȿ���� ��
    public float bgSoundValue = 1.0f; // ����� ��
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

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(go, clip.length);
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
        SetBGVolumeAct?.Invoke(bgSoundValue);
    }
    public void SetVolume() // ȿ����
    {
        soundValue = volumeSlider.value;
        SetVolumeAct?.Invoke(soundValue);
    }
}
