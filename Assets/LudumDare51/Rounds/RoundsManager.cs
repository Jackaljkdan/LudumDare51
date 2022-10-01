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
        public FencerController player;
        public FencerController ai;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            player = context.Get<FencerController>(this, "player");
            ai = context.Get<FencerController>(this, "ai");
        }

        private void Start()
        {
            round.SetSilently(0);
            StartNextRound();

            player.attributes.healthPoints.onChange.AddListener(OnHealthChanged);
            ai.attributes.healthPoints.onChange.AddListener(OnHealthChanged);
        }

        private void OnHealthChanged(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == 0)
            {
                if (player.attributes.healthPoints.Value > ai.attributes.healthPoints.Value)
                    playerWins.Value++;
                else
                    aiWins.Value++;

                Invoke(nameof(StartNextRound), nextRoundDelaySeconds);
                onRoundEnd.Invoke();
            }
        }

        private void StartNextRound()
        {
            player.attributes.Reset();
            ai.attributes.Reset();

            if (player.IsKo())
                player.Revive();
            else if (ai.IsKo())
                ai.Revive();

            round.Value++;

            Invoke(nameof(StartFight), fightDelaySeconds);
        }

        private void StartFight()
        {
            onFight.Invoke();
        }
    }
}