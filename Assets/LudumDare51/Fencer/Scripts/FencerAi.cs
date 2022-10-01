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

        public float averageAttacksPerSecond = 2f;
        public float deltaAttacksPerSeconds = 0.5f;

        public float parryProbability = 0.5f;
        public float counterProbability = 0.5f;

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
        }

        private void Update()
        {
            switch (state)
            {
                default:
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
                nextAttackTime = Time.time + RandomUtils.TimeUntilNextEvent(averageAttacksPerSecond, deltaAttacksPerSeconds);

            if (Time.time >= nextAttackTime)
            {
                EnterAttacking();
                return;
            }

            if (player.wasAttackActive)
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
            if (!player.isAttacking)
            {
                EnterIdle();
                IdleUpdate();
                return;
            }

            if (player.IsCurrentAnimation(player.stance.AttackAnimationName())
                && player.GetCurrentAnimationNormalizedTime() >= parryDelay
            )
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
                EnterAttacking();
            else
                EnterIdle();
        }
    }
}