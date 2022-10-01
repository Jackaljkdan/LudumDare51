using JK.PropertyDrawers;
using JK.Utils.DGTweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [DisallowMultipleComponent]
    public class FencerController : MonoBehaviour
    {
        #region Inspector

        public Animator animator;

        public float parryToAttackOffsetSeconds = 0.2f;

        [RuntimeHeader]

        public bool canAttack;
        public bool canParry;

        public bool isAttacking;
        public bool isAttackActive;
        public bool wasAttackActive;

        public bool isParryActive;

        public FencerCommand bufferedCommand;

        public FencerStance stance;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        #endregion

        private void Start()
        {
            stance = FencerStance.Thrust;
            ForceAllowAll();
            ResetStatus();
        }

        private void Update()
        {
            switch (bufferedCommand)
            {
                case FencerCommand.Attack:
                    Attack();
                    break;
                case FencerCommand.Parry:
                    Parry();
                    break;
            }
        }

        public void Attack()
        {
            if (!canAttack)
            {
                bufferedCommand = FencerCommand.Attack;
                return;
            }

            DisallowAll();
            SetAttackBlend(0);
            isAttacking = true;

            if (IsCurrentAnimation(stance.ParryAnimationName()))
                CrossFade(stance.AttackAnimationName(), transitionSeconds: 0.2f, offsetSeconds: parryToAttackOffsetSeconds);
            else
                CrossFade(stance.AttackAnimationName());
        }

        public void Parry()
        {
            if (!canParry)
            {
                bufferedCommand = FencerCommand.Parry;
                return;
            }

            DisallowAll();
            CrossFade(stance.ParryAnimationName());
        }

        public void DisallowAll()
        {
            canAttack = false;
            canParry = false;
            bufferedCommand = FencerCommand.None;
        }

        public void AllowAll()
        {
            if (!IsInTransition() || IsNextAnimation("Idle"))
            {
                ForceAllowAll();
                ResetStatus();
            }
        }

        public void ForceAllowAll()
        {
            canAttack = true;
            canParry = true;
        }

        public void ResetStatus()
        {
            isAttacking = false;
            isAttackActive = false;
            wasAttackActive = false;
            isParryActive = false;
        }

        public void AllowAttack()
        {
            if (!IsInTransition() || IsNextAnimation("Idle"))
                canAttack = true;
        }

        public void AllowParry()
        {
            if (!IsInTransition() || IsNextAnimation("Idle"))
                canParry = true;
        }

        public void ActivateAttack()
        {
            if (!IsInTransition() || IsNextAnimation(stance.AttackAnimationName()))
            {
                isAttackActive = true;
                wasAttackActive = false;
            }
        }

        public void DeactivateAttack()
        {
            isAttackActive = false;
            wasAttackActive = true;
        }

        public void ActivateParry()
        {
            if (!IsInTransition() || IsNextAnimation(stance.ParryAnimationName()))
                isParryActive = true;
        }

        public void DeactivateParry()
        {
            isParryActive = false;
        }

        public void SetAttackBlend(float blend, float seconds = 0)
        {
            if (seconds > 0)
                animator.DOFloat("AttackBlend", blend, seconds);
            else
                animator.SetFloat("AttackBlend", blend);
        }

        public void CrossFade(string name, float transitionSeconds = 0.1f, float offsetSeconds = 0, int layer = 0)
        {
            animator.CrossFadeInFixedTime(name, transitionSeconds, layer, offsetSeconds);
        }

        public float GetCurrentAnimationNormalizedTime()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public bool IsInTransition()
        {
            return animator.IsInTransition(0);
        }

        public bool IsCurrentAnimation(string name)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
        }

        public bool IsNextAnimation(string name)
        {
            return animator.GetNextAnimatorStateInfo(0).IsName(name);
        }

        public bool IsCurrentOrNextAnimation(string name)
        {
            return IsCurrentAnimation(name) || IsNextAnimation(name);
        }
    }
}