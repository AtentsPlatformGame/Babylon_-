using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LLoading : MonoBehaviour
{
    public static LLoading instance;
    public string nextSceneName;
    public GameObject Loading;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncLoad.isDone)
        {
            // ���� ��Ȳ ������Ʈ
            // ��: ���� ��Ȳ�� �����ִ� �� ������Ʈ
            yield return null;
        }
    }
}
