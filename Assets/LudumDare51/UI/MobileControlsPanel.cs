using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Utils;
using LudumDare51.Rounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class MobileControlsPanel : MonoBehaviour
    {
        #region Inspector

        public CanvasGroup group;

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
            if (!PlatformUtils.IsMobileBuild)
            {
                Destroy(gameObject);
                return;
            }

            roundsManager.onWinOrLose.AddListener(OnWinOrLose);
        }

        private void OnWinOrLose()
        {
            group.DOFade(0, 0.25f);
            transform.DOScale(0.8f, 0.25f).onComplete += () =>
            {
                gameObject.SetActive(false);
            };
        }
    }
}