using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts;
using _Game._Scripts.Support;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private void OnEnable()
    {
        this.RegisterListener(EventID.LoadSceneByName,LoadSceneByName);
        this.RegisterListener(EventID.LoadSceneByIndex,LoadSceneByIndex);
        this.RegisterListener(EventID.LoadSceneMain, (_) =>
        {
            if (!this.TryGetData(DataShareKey.MainSceneName ,out string data)) return;
            LoadSceneByName(data);
            
        });
        this.RegisterListener(EventID.LoadSceneStart, (_) =>
        {
            if (!this.TryGetData(DataShareKey.StartSceneName,out string data)) return;
            LoadSceneByName(data);
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
