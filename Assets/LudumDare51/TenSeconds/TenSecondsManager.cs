using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using JK.PropertyDrawers;
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

        [RuntimeHeader]

        public ObservableProperty<int> seconds = new ObservableProperty<int>();

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
            roundsManager.onFight.AddListener(OnFight);
            roundsManager.onRoundEnd.AddListener(OnRoundEnd);
            seconds.Value = 10;
        }

        private void OnFight()
        {
            Resume();
        }

        private void OnRoundEnd()
        {
            Stop();
        }

        public void Resume()
        {
            InvokeRepeating(nameof(Timer), 1, 1);
        }

        private void Timer()
        {
            seconds.Value--;

            if (seconds.Value == 0)
            {
                TriggerDistraction();
                Stop();
            }
        }

        private void TriggerDistraction()
        {

        }

        public void Stop()
        {
            CancelInvoke(nameof(Timer));
        }
    }
}