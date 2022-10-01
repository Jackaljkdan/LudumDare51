using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Rounds
{
    [DisallowMultipleComponent]
    public class FightPanel : MonoBehaviour
    {
        #region Inspector

        public CanvasGroup group;

        [Injected]
        public RoundsManager roundsManager;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            roundsManager = context.Get<RoundsManager>(this);
        }

        private void Start()
        {
            roundsManager.round.onChange.AddListener(OnRoundChanged);
            gameObject.SetActive(false);
        }

        private void OnRoundChanged(ObservableProperty<int>.Changed arg)
        {
            gameObject.SetActive(true);

            group.alpha = 0;
            Vector3 scale = new Vector3(0.8f, 0.8f, 0.8f);
            transform.localScale = scale;

            float delay = roundsManager.fightDelaySeconds - 1;
            transform.DOScale(1, 0.25f).SetDelay(delay);
            group.DOFade(1, 0.25f).SetDelay(delay);

            transform.DOScale(scale, 0.25f).SetDelay(delay + 0.75f);
            group.DOFade(0, 0.25f).SetDelay(delay + 0.75f).onComplete += () =>
            {
                gameObject.SetActive(false);
            };
        }
    }
}