using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.PropertyDrawers;
using JK.Utils;
using LudumDare51.TenSeconds;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [DisallowMultipleComponent]
    public class FencerAi : MonoBehaviour
    {
        #region Inspector

        public FencerController controller;

        public float parryDelay = 0.2f;
        public float maxParryDelay = 0.32f;
        public float counterDelay = 0.2f;

        public float averageSecondsBetweenAttacks = 2f;
        public float deltaSecondsBetweenAttacks = 0.5f;

        public float parryProbability = 0.5f;
        public float blindParryAfterReboundProbability = 0.5f;
        public float counterProbability = 0.5f;
        public float counterDelayProbability = 0.5f;
        public float focusProbability = 0f;

        [RuntimeHeader]

        public float nextAttackTime;

        public FencerAiState state;

        [Injected]
        public FencerController player;

        [Injected]
        public TenSecondsManager tenSecondsManager;

        private void Reset()
        {
            controller = GetComponent<FencerController>();
        }

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            player = context.Get<FencerController>(this, "player");
            tenSecondsManager = context.Get<TenSecondsManager>(this);
        }

        private void Start()
        {
            state = FencerAiState.Idle;
            controller.onRebound.AddListener(OnRebound);
            controller.onHit.AddListener(OnHit);
        }

        private void Update()
        {
            switch (state)
            {
                case FencerAiState.Idle:
                    IdleUpdate();
                    break;
                case FencerAiState.IdleNoFocus:
                    IdleNoFocusUpdate();
                    break;
                case FencerAiState.IdleNoParry:
                    IdleNoParryUpdate();
                    break;
                case FencerAiState.Attacking:
                    AttackingUpdate();
                    break;
                case FencerAiState.ParryWaiting:
                    ParryWaitingUpdate();
                    break;
                case FencerAiState.Parrying:
                    ParryingUpdate();
                    break;
                case FencerAiState.Counter:
                    CounterUpdate();
                    break;
                case FencerAiState.Hit:
                    HitUpdate();
                    break;
                case FencerAiState.Focus:
                    FocusUpdate();
                    break;
            }
        }

        private void EnterIdle()
        {
            state = FencerAiState.Idle;
            nextAttackTime = 0;
        }

        private void IdleUpdate()
        {
            if (tenSecondsManager.seconds.Value == 1
                && (Time.time - tenSecondsManager.secondStartTime) >= 0.5f
                && tenSecondsManager.nextDistraction.Value != null
                && tenSecondsManager.nextDistraction.Value.GetInstructions() != null
            )
            {
                if (RandomUtils.Should(focusProbability))
                {
                    EnterFocus();
                    return;
                }

                state = FencerAiState.IdleNoFocus;
            }

            IdleNoFocusUpdate();
        }

        private void IdleNoFocusUpdate()
        {
            if (player.IsAttacking())
            {
                if (RandomUtils.Should(parryProbability))
                {
                    EnterParryWaiting();
                    ParryWaitingUpdate();
                    return;
                }

                state = FencerAiState.IdleNoParry;
            }

            IdleNoParryUpdate();
        }

        private void IdleNoParryUpdate()
        {
            if (nextAttackTime == 0)
                nextAttackTime = Time.time + RandomUtils.TimeUntilNextEvent(averageSecondsBetweenAttacks, deltaSecondsBetweenAttacks);

            if (Time.time >= nextAttackTime)
            {
                EnterAttacking();
                return;
            }

            if (!player.IsAttacking())
                state = FencerAiState.Idle;
        }

        private void EnterAttacking()
        {
            state = FencerAiState.Attacking;
            controller.Attack();
        }

        private void AttackingUpdate()
        {
            if (controller.canParry)
            {
                EnterIdle();
                IdleUpdate();
            }
        }

        private void EnterParryWaiting()
        {
            state = FencerAiState.ParryWaiting;
        }

        private void ParryWaitingUpdate()
        {
            if (!player.IsAttacking())
            {
                EnterIdle();
                IdleUpdate();
                return;
            }

            if (!player.TryGetAttackNormalizedTime(out float normalizedTime, out bool isNextAnimation))
                return;

            if (normalizedTime >= parryDelay)
                EnterParrying();
        }

        private void EnterParrying()
        {
            state = FencerAiState.Parrying;
            controller.Parry();
        }

        private void ParryingUpdate()
        {
            if (!controller.canAttack)
                return;

            if (RandomUtils.Should(counterProbability))
            {
                EnterCounter();
                CounterUpdate();
                return;
            }

            EnterIdle();
        }

        private void EnterCounter()
        {
            state = FencerAiState.Counter;

            if (RandomUtils.Should(counterDelayProbability))
                nextAttackTime = Time.time + counterDelay;
            else
                nextAttackTime = Time.time;
        }

        private void CounterUpdate()
        {
            if (Time.time >= nextAttackTime)
                EnterAttacking();
        }

        private void EnterHit()
        {
            state = FencerAiState.Hit;
        }

        private void HitUpdate()
        {
            if (controller.canAttack || controller.canParry)
            {
                EnterIdle();
                IdleUpdate();
            }
        }

        private float focusEnterTime;

        private void EnterFocus()
        {
            state = FencerAiState.Focus;
            controller.EnterFocus();
            focusEnterTime = Time.time;
        }

        private void FocusUpdate()
        {
            controller.EnterFocus();

            if (Time.time >= focusEnterTime + 0.8f)
            {
                controller.ExitFocus();
                EnterIdle();
                IdleUpdate();
            }
        }

        private void OnRebound()
        {
            if (RandomUtils.Should(blindParryAfterReboundProbability))
                EnterParrying();
        }

        private void OnHit()
        {
            controller.bufferedCommand = FencerCommand.None;
            EnterHit();
        }
    }
}