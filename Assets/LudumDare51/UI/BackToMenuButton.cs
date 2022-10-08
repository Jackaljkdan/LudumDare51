using JK.Injection;
using JK.Injection.PropertyDrawers;
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
    public class BackToMenuButton : MonoBehaviour
    {
        #region Inspector

        public float secondTapSeconds = 2;

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
            firstTapTime = -secondTapSeconds;
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                OnClicked();
        }

        private void OnClicked()
        {
            if (Time.time > firstTapTime + secondTapSeconds)
                toast.Show("Tap again to exit", secondTapSeconds);
            else
                SceneManager.LoadSceneAsync("Start");

            firstTapTime = Time.time;
        }
    }
}