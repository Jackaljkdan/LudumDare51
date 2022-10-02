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

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class ContinuePanel : MonoBehaviour
    {
        #region Inspector

        public CanvasGroup group;

        public string nextScene;

        [Injected]
        public RoundsManager roundsManager;

        private void Reset()
        {
            group = GetComponent<CanvasGroup>();
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
            gameObject.SetActive(false);
        }

        private void OnWinOrLose()
        {
            gameObject.SetActive(true);
            group.alpha = 0;
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            group.DOFade(1, 0.25f).SetDelay(2);
            transform.DOScale(1, 0.25f).SetDelay(2);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                enabled = false;

                if (roundsManager.playerWins.Value > roundsManager.aiWins.Value)
                    SceneManager.LoadSceneAsync(nextScene);
                else
                    SceneManager.LoadSceneAsync(gameObject.scene.name);
            }
        }
    }
}