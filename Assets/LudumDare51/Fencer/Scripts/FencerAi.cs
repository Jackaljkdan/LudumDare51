using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.PropertyDrawers;
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

        public float averageAttacksPerSecond = 2f;
        public float delta = 0.5f;

        public float parryDelay = 0.2f;

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
                case FencerAiState.Attack:
                    AttackUpdate();
                    break;
                case FencerAiState.Parry:
                    ParryUpdate();
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
                if (player.IsCurrentAnimation(player.stance.AttackAnimationName()) && player.GetCurrentAnimationNormalizedTime() >= parryDelay)
                {
                    EnterParry();
                    return;
                }
            }
        }

        private void EnterAttack()
        {
            state = FencerAiState.Attack;
        }

        private void AttackUpdate()
        {

        }

        private void EnterParry()
        {
            state = FencerAiState.Parry;
            controller.Parry();
        }

        private void ParryUpdate()
        {
            if (!controller.canAttack)
                return;

            EnterIdle();
        }
    }
}