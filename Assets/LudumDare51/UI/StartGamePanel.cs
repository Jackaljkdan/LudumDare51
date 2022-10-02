using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class StartGamePanel : MonoBehaviour
    {
        #region Inspector

        public Button button;

        private void Reset()
        {
            button = GetComponentInChildren<Button>();
        }

        #endregion

        private void Start()
        {
            button.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            button.enabled = false;
            SceneManager.LoadSceneAsync("IntroTalk");
        }
    }
}