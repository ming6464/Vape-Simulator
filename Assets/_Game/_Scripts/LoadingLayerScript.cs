using System;
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
        
        private float _splashDeltaTime;
        
        private void Awake()
        {
            _splashScreen.SetActive(true);
            _loadingLayer.SetActive(false);
        }

        private void OnEnable()
        {
            this.RegisterListener(EventID.ProgressLoading,UpdateProgressLoading);
        }

        private void OnDisable()
        {
            this.RemoveListener(EventID.ProgressLoading,UpdateProgressLoading);
        }
        private void UpdateProgressLoading(object obj)
        {
            _sliderUI.fillAmount = (float) obj;
        }

        private void Start()
        {
            AnimationLoadingScene();
        }

        private async void AnimationLoadingScene()
        {
            while (true)
            {
                _splashDeltaTime += Time.deltaTime;
                if(_splashDeltaTime >= _splashTime)
                {
                    _splashScreen.SetActive(false);
                    _loadingLayer.SetActive(true);
                    if (BlackBoard.Instance.TryGetValue(BlackBoardKEY.StartSceneName, out string nameScene))
                    {
                        this.PostEvent(EventID.LoadSceneByName,nameScene);
                        return;
                    }
                    
                }
                await Task.Yield();
            }
        }
        
    }
}