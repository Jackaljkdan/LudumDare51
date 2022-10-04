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
    [RequireComponent(typeof(Button))]
    public class ChangeSceneButton : MonoBehaviour
    {
        #region Inspector

        public string targetSceneName;

        #endregion

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            GetComponent<Button>().enabled = false;
            SceneManager.LoadSceneAsync(targetSceneName);
        }
    }
}