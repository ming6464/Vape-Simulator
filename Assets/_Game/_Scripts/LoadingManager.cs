using System.Threading.Tasks;
using BlackBoardSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    
    private bool   _isLoading;
    private string _mainScene = null;
    private string _startScene = null;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.LoadSceneByName,LoadScene);
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

    private void OnDisable()
    {
        this.RemoveListener(EventID.LoadSceneByName,LoadScene);
        this.RemoveEvent(EventID.LoadSceneMain);
        this.RemoveEvent(EventID.LoadSceneStart);
    }

    private void LoadSceneByName(object obj)
    {
        SetActiveLoadingUI(true);
        LoadScene(obj);
    }

    private void SetActiveLoadingUI(bool isActive)
    {
        _canvasGroup.alpha          = isActive ? 1 : 0;
        _canvasGroup.interactable   = !isActive;
        _canvasGroup.blocksRaycasts = isActive;
    }
    
    private async void LoadScene(object obj)
    {
        if(_isLoading) return;
        _isLoading = true;
        var name = (string)obj;
        var progress = SceneManager.LoadSceneAsync(name);
        while (!progress.isDone)
        {
            this.PostEvent(EventID.ProgressLoading,progress.progress);
            await Task.Yield();
        }
        await Task.Delay(100);
        this.PostEvent(EventID.FinishLoading);
        SetActiveLoadingUI(false);
        _isLoading = false;
    }
}
