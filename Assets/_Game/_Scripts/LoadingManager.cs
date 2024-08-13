using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts;
using _Game._Scripts.Support;
using BlackBoardSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _time;
    
    private bool   _isLoading;
    private float  _timeDelta;
    private string _nameSceneLoading;
    private string _mainScene = null;
    private string _startScene = null;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.LoadSceneByName,LoadScene);
        this.RegisterListener(EventID.LoadSceneByIndex,LoadSceneByIndex);
        this.RegisterListener(EventID.LoadSceneMain, (_) =>
        {
            if (_mainScene == null)
            {
                if (!BlackBoard.Instance.TryGetValue(BlackBoardKEY.MainSceneName ,out _mainScene)) return;
            }
            LoadSceneByName(_mainScene);
            
        });
        this.RegisterListener(EventID.LoadSceneStart, (_) =>
        {
            if (_startScene == null)
            {
                if (!BlackBoard.Instance.TryGetValue(BlackBoardKEY.StartSceneName,out _startScene)) return;
            }
            LoadSceneByName(_startScene);
        });
    }

    private void Update()
    {
        if (_isLoading)
        {
            _timeDelta += Time.deltaTime;
            var progressBar = _timeDelta / _time;
            progressBar   = Mathf.Clamp01(progressBar);
            _slider.value = progressBar;

            if (progressBar - 1 == 0)
            {
                LoadScene(_nameSceneLoading);
                SetActiveLoadingUI(false);
                _isLoading = false;
            }
        }
    }

    private void LoadSceneByName(object obj)
    {
        SetDataLoading((string)obj);
    }

    private void LoadSceneByIndex(object obj)
    {
        var name = SceneManager.GetSceneAt((int)obj).name;
        LoadScene(name);
    }

    private void SetDataLoading(string name)
    {
        SetActiveLoadingUI(true);
        _isLoading                  = true;
        _timeDelta                  = 0;
        _nameSceneLoading           = name;
    }

    private void SetActiveLoadingUI(bool isActive)
    {
        if (isActive)
        {
            _canvasGroup.alpha          = 1;
            _canvasGroup.interactable   = false;
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            _canvasGroup.alpha          = 0;
            _canvasGroup.interactable   = false;
            _canvasGroup.blocksRaycasts = false;   
        }
        
    }
    
    private void LoadScene(object obj)
    {
        var name = (string)obj;
        SceneManager.LoadScene(name);
    }
}
