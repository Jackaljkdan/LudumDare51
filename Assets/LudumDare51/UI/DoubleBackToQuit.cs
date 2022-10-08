using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class DoubleBackToQuit : MonoBehaviour
    {
        #region Inspector

        public float secondBackSeconds = 2;

        [Injected]
        public Toast toast;

        #endregion

        private float firstTapTime;

        private void Awake()
        {
            Context context = Context.Find(this);
            toast = context.Get<Toast>(this);
        }

        private void Start()
        {
            if (PlatformUtils.IsWebGL)
            {
                Destroy(this);
                return;
            }

            firstTapTime = -secondBackSeconds;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                OnBackClicked();
        }

        private void OnBackClicked()
        {
            if (Time.time > firstTapTime + secondBackSeconds)
                toast.Show("Tap again to quit", secondBackSeconds);
            else
                Application.Quit();

            firstTapTime = Time.time;
        }
    }
}