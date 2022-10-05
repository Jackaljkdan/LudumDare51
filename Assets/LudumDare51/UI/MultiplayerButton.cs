using JK.Utils;
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
    public class MultiplayerButton : MonoBehaviour
    {
        #region Inspector



        #endregion

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            GetComponent<Button>().interactable = false;

            if (PlatformUtils.IsDesktopBuild)
                SceneManager.LoadSceneAsync("MultiplayerIntro");
            else
                SceneManager.LoadSceneAsync("MultiplayerMatch");
        }
    }
}