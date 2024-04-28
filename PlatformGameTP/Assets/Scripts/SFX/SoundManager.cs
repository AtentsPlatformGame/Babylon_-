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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1)
    {
        for (int i = 0; i< bglist.Length; i++)
        {
            if(arg0.name == bglist[i].name)
                BgSoundPlay(bglist[i]);
        }
    }


    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.Play();
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
    public void SetVolume() // ȿ����
    {
        soundValue = volumeSlider.value;
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
