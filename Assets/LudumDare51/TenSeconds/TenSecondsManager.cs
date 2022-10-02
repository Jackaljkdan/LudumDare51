using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using JK.PropertyDrawers;
using JK.Utils;
using LudumDare51.Rounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    public class TenSecondsManager : MonoBehaviour
    {
        #region Inspector

        public ObservableProperty<TenSecondsEffect> currentDistraction = new ObservableProperty<TenSecondsEffect>();

        public ObservableProperty<TenSecondsEffect> nextDistraction = new ObservableProperty<TenSecondsEffect>();

        [RuntimeHeader]

        public ObservableProperty<int> seconds = new ObservableProperty<int>();

        [Injected]
        public RoundsManager roundsManager;

        #endregion

        private List<TenSecondsEffect> distractions = new List<TenSecondsEffect>(16);

        private void Awake()
        {
            Context context = Context.Find(this);
            roundsManager = context.Get<RoundsManager>(this);
        }

        private void Start()
        {
            roundsManager.onFight.AddListener(OnFight);
            roundsManager.onRoundEnd.AddListener(OnRoundEnd);
            roundsManager.onWinOrLose.AddListener(OnWinOrLose);
            Setup();
        }

        private void OnFight()
        {
            Resume();
        }

        private void OnRoundEnd()
        {
            Stop();
        }

        private void OnWinOrLose()
        {
            Stop();

            if (currentDistraction.Value != null)
                currentDistraction.Value.Revert();
        }

        public void Restart()
        {
            Setup();
            Resume();
        }

        private void Setup()
        {
            seconds.Value = 10;

            if (nextDistraction.Value == null)
                nextDistraction.Value = GetRandomDistraction();
        }

        public void Resume()
        {
            InvokeRepeating(nameof(Timer), 1, 1);
        }

        public void Stop()
        {
            CancelInvoke(nameof(Timer));
        }

        private void Timer()
        {
            seconds.Value--;

            if (seconds.Value == 0)
            {
                if (currentDistraction.Value != null)
                    currentDistraction.Value.Revert();

                currentDistraction.Value = nextDistraction.Value;
                nextDistraction.Value = null;

                if (currentDistraction.Value != null)
                {
                    Stop();
                    currentDistraction.Value.Apply(Restart);
                }
                else
                {
                    Restart();
                }
            }
        }

        private TenSecondsEffect GetRandomDistraction()
        {
            GetComponentsInChildren(distractions);
            distractions.ShuffleInPlace();

            foreach (var distraction in distractions)
            {
                if (!distraction.gameObject.activeInHierarchy)
                    continue;

                if (!distraction.enabled)
                    continue;

                if (distraction == currentDistraction.Value)
                    continue;

                if (distraction.CanApply())
                    return distraction;
            }

            return null;
        }
    }
}