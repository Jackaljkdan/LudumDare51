using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using LudumDare51.Rounds;
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
    public class ContinuePanel : MonoBehaviour
    {
        #region Inspector

        public CanvasGroup group;

        public Button button;

        public string nextScene;

        public bool alwaysGoToNextScene;

        [Injected]
        public RoundsManager roundsManager;

        private void Reset()
        {
            group = GetComponent<CanvasGroup>();
            button = GetComponent<Button>();
        }

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            roundsManager = context.Get<RoundsManager>(this);
        }

        private void Start()
        {
            roundsManager.onWinOrLose.AddListener(OnWinOrLose);
            button.onClick.AddListener(OnClicked);
            gameObject.SetActive(false);
        }

        private void OnWinOrLose()
        {
            gameObject.SetActive(true);
            enabled = false;

            group.alpha = 0;
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            group.DOFade(1, 0.25f).SetDelay(2);
            transform.DOScale(1, 0.25f).SetDelay(2).onComplete += () => enabled = true;
        }

        private void OnClicked()
        {
            button.onClick.RemoveListener(OnClicked);

            if (alwaysGoToNextScene || roundsManager.playerWins.Value > roundsManager.aiWins.Value)
                SceneManager.LoadSceneAsync(nextScene);
            else
                SceneManager.LoadSceneAsync(gameObject.scene.name);
        }
    }
}