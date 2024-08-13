using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game._Scripts
{
    public class LoadingLayerScript : MonoBehaviour
    {
        private void Start()
        {
            this.PostEvent(EventID.LoadSceneStart);
            // SceneManager.LoadScene("Test__");
        }
    }
}