using JK.Attributes;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using JK.PropertyDrawers;
using JK.Utils;
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

        public int bestOf = 3;

        public List<AudioClip> roundClips;

        public AudioClip winClip;
        public AudioClip loseClip;

        public float nextRoundDelaySeconds = 5f;

        public float fightDelaySeconds = 3;

        public UnityEvent onFight = new UnityEvent();

        public UnityEvent onRoundEnd = new UnityEvent();

        public UnityEvent onWinOrLose = new UnityEvent();

        [RuntimeHeader]

        public ObservableProperty<int> round = new ObservableProperty<int>();

        public ObservableProperty<int> playerWins = new ObservableProperty<int>();
        public ObservableProperty<int> aiWins = new ObservableProperty<int>();

        [Injected]
        public FencerController player;
        public FencerController ai;

        [Injected]
        public AudioSource voiceAudioSource;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            player = context.Get<FencerController>(this, "player");
            ai = context.Get<FencerController>(this, "ai");
            voiceAudioSource = context.Get<AudioSource>(this, "voice");
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
            if (arg.updated <= 0)
            {
                onRoundEnd.Invoke();

                bool someoneWon = false;

                if (player.attributes.healthPoints.Value > ai.attributes.healthPoints.Value)
                {
                    playerWins.Value++;

                    if (playerWins.Value > bestOf / 2)
                    {
                        someoneWon = true;
                        player.ForceFocus();
                        this.RunAfterSeconds(1, () => voiceAudioSource.PlayOneShotSafely(winClip));
                    }
                }
                else
                {
                    aiWins.Value++;

                    if (aiWins.Value > bestOf / 2)
                    {
                        someoneWon = true;
                        ai.ForceFocus();
                        this.RunAfterSeconds(1, () => voiceAudioSource.PlayOneShotSafely(loseClip));
                    }
                }

                if (someoneWon)
                    onWinOrLose.Invoke();
                else
                    Invoke(nameof(StartNextRound), nextRoundDelaySeconds);
            }
        }

        private void StartNextRound()
        {
            if (player.attributes.healthPoints.Value <= 0)
                player.Revive();
            else if (ai.attributes.healthPoints.Value <= 0)
                ai.Revive();

            player.attributes.Reset();
            ai.attributes.Reset();

            voiceAudioSource.PlayOneShot(roundClips[round.Value]);

            round.Value++;

            Invoke(nameof(StartFight), fightDelaySeconds);
        }

        private void StartFight()
        {
            onFight.Invoke();
        }
    }
}