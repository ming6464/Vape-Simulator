using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private EventDispatcher _eventDispatcher;
    private void OnEnable()
    {
        if (!_eventDispatcher)
        {
            _eventDispatcher = EventDispatcher.Instance;
        }
        
        _eventDispatcher.RegisterListener(EventID.LoadSceneByName,LoadSceneByName);
        _eventDispatcher.RegisterListener(EventID.LoadSceneByIndex,LoadSceneByIndex);
        _eventDispatcher.RegisterListener(EventID.LoadSceneMain, (_) =>
        {
            LoadSceneByName(DataConfig.Instance.MainScene);
        });
        _eventDispatcher.RegisterListener(EventID.LoadSceneStart, (_) =>
        {
            LoadSceneByName(DataConfig.Instance.StartScene);
        });
    }

    private void LoadSceneByName(object obj)
    {
        LoadScene((string)obj);
    }

    private void LoadSceneByIndex(object obj)
    {
        var name = SceneManager.GetSceneAt((int)obj).name;
        LoadScene(name);
    }
    

    private void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
