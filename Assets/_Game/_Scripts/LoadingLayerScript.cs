using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class LoadingLayerScript : MonoBehaviour
    {
        [SerializeField] private Image _sliderUI;
        private void Start()
        {
            this.PostEvent(EventID.LoadSceneStart);
            // SceneManager.LoadScene("Test__");
        }
    }
}