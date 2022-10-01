using JK.Attributes;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using JK.PropertyDrawers;
using LudumDare51.Fencer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Rounds
{
    [DisallowMultipleComponent]
    [LateExecutionOrder]
    public class RoundsManager : MonoBehaviour
    {
        #region Inspector

        public float nextRoundDelaySeconds = 5f;

        public float fightDelaySeconds = 3;

        public UnityEvent onFight = new UnityEvent();

        public UnityEvent onRoundEnd = new UnityEvent();

        [RuntimeHeader]

        public ObservableProperty<int> round = new ObservableProperty<int>();

        public ObservableProperty<int> playerWins = new ObservableProperty<int>();
        public ObservableProperty<int> aiWins = new ObservableProperty<int>();

        [Injected]
        public FencerAttributes playerAttributes;
        public FencerAttributes aiAttributes;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            playerAttributes = context.Get<FencerAttributes>(this, "player");
            aiAttributes = context.Get<FencerAttributes>(this, "ai");
        }

        private void Start()
        {
            round.SetSilently(0);
            StartNextRound();

            playerAttributes.healthPoints.onChange.AddListener(OnHealthChanged);
            aiAttributes.healthPoints.onChange.AddListener(OnHealthChanged);
        }

        private void OnHealthChanged(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == 0)
            {
                if (playerAttributes.healthPoints.Value > aiAttributes.healthPoints.Value)
                    playerWins.Value++;
                else
                    aiWins.Value++;

                Invoke(nameof(StartNextRound), nextRoundDelaySeconds);
                onRoundEnd.Invoke();
            }
        }

        private void StartNextRound()
        {
            playerAttributes.ResetAttributes();
            aiAttributes.ResetAttributes();

            round.Value++;

            Invoke(nameof(StartFight), fightDelaySeconds);
        }

        private void StartFight()
        {
            onFight.Invoke();
        }
    }
}