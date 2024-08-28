using System.Threading.Tasks;
using BlackBoardSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class LoadingLayerScript : MonoBehaviour
    {
        [SerializeField] private GameObject _splashScreen;
        [SerializeField] private float      _splashTime = 1f;
        [SerializeField] private GameObject _loadingLayer;
        [SerializeField] private Image      _sliderUI;
        [SerializeField] private float      _loadingTime = 2f;
        
        private float _splashDeltaTime;
        private float _loadingDeltaTime;
        private int _stateIndex;
        
        private void Awake()
        {
            _splashScreen.SetActive(true);
            _loadingLayer.SetActive(false);
            _stateIndex = 0;
        }

        private void Start()
        {
            AnimationLoadingScene();
        }

        private async void AnimationLoadingScene()
        {
            while (_stateIndex >= 0)
            {
                switch (_stateIndex)
                {
                    case 0:
                    {
                        _splashDeltaTime += Time.deltaTime;
                        if(_splashDeltaTime >= _splashTime)
                        {
                            _splashScreen.SetActive(false);
                            _loadingLayer.SetActive(true);
                            _stateIndex = 1;
                        }
                        break;
                    }
                    case 1:
                    {
                        _loadingDeltaTime    += Time.deltaTime;
                        _sliderUI.fillAmount =  _loadingDeltaTime * 1.0f / _loadingTime;
                        if(_loadingTime <= _loadingDeltaTime)
                        {
                            _stateIndex = -1;
                            if (!BlackBoard.Instance.TryGetValue(BlackBoardKEY.StartSceneName,out string nameScene)) return;
                            this.PostEvent(EventID.LoadSceneByName,nameScene);
                        }

                        break;
                    }
                }
                await Task.Yield();
            }
        }
        
    }
}