using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    private static SceneMgr instance;
    public static SceneMgr Instance =>instance;

    
    public List<GameScene_So> scenes;
    private GameScene_So gameScene;
    private GameScene_So sceneToLoad;
    private float duration;
    private UnityAction loadingAction;

    private void Awake()
    {
        duration = 1f;
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        UIManager.Instance.ShowPanel<BeginPanel>("BeginPanel", true);
    }

    public void LoadScene(GameScene_So scene, UnityAction callBack=null)
    {
        sceneToLoad = scene;
        loadingAction = callBack;

        StartCoroutine(UnLoadPreviousScene());
    }

    private IEnumerator UnLoadPreviousScene()
    {
        SceneFadeInOut.Instance.FadeIn(duration);
        yield return new WaitForSeconds(duration);

        //¼ÓÔØÐÂ³¡¾°
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = Addressables.LoadSceneAsync(sceneToLoad.sceneReference, LoadSceneMode.Single);
        loadingOption.Completed += OnLoadCompleted; 
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        loadingAction();
        SceneFadeInOut.Instance.FadeOut(duration);
    }

    public GameScene_So GetScene(string name)
    {
        switch (name)
        {
            case "Main":
                gameScene = scenes[0];
                break;
            case "Game":
                gameScene = scenes[1];
                break;
            case "Boss":
                gameScene = scenes[2];
                break;
        }
        return gameScene;
    }
}
