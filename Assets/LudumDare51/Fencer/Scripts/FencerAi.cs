using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.PropertyDrawers;
using JK.Utils;
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
        public float hitParryDelay = 0.3f;
        public float counterDelay = 0.2f;

        public float averageSecondsBetweenAttacks = 2f;
        public float deltaSecondsBetweenAttacks = 0.5f;

        public float parryProbability = 0.5f;
        public float counterProbability = 0.5f;
        public float counterDelayProbability = 0.5f;

        [RuntimeHeader]

        public float nextAttackTime;

        public FencerAiState state;

        [Injected]
        public FencerController player;

        private void Reset()
        {
            controller = GetComponent<FencerController>();
        }

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            player = context.Get<FencerController>("player");
        }

        private void Start()
        {
            state = FencerAiState.Idle;
            controller.onHit.AddListener(OnHit);
        }

        private void Update()
        {
            switch (state)
            {
                case FencerAiState.Idle:
                    IdleUpdate();
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
            }
        }

        private void EnterIdle()
        {
            state = FencerAiState.Idle;
            nextAttackTime = 0;
        }

        private void IdleUpdate()
        {
            if (player.isAttacking && !player.wasAttackActive)
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

            if (!player.isAttacking || player.wasAttackActive)
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
            if (!player.isAttacking || player.wasAttackActive)
            {
                EnterIdle();
                IdleUpdate();
                return;
            }

            float normalizedTime;

            bool isNext = player.IsNextAnimation(player.stance.AttackAnimationName());

            if (isNext)
                normalizedTime = player.GetNextAnimatorNormalizedTime();
            else
                normalizedTime = player.GetCurrentAnimationNormalizedTime();

            float relevantParryDelay;

            if (controller.IsCurrentAnimationHit())
                relevantParryDelay = hitParryDelay;
            else
                relevantParryDelay = parryDelay;

            if (normalizedTime >= relevantParryDelay)
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

        private void OnHit()
        {
            controller.bufferedCommand = FencerCommand.None;
            EnterHit();
        }
    }
}