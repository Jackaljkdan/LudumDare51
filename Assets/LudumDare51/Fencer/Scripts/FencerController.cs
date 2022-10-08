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

        public FencerAttributes attributes;

        public UnityEvent onAttack = new UnityEvent();
        public UnityEvent onRebound = new UnityEvent();
        public UnityEvent onHit = new UnityEvent();

        [Header("Internals")]

        public Animator animator;

        public float parryToAttackOffsetSeconds = 0.2f;
        public float hitToAnyOffsetSeconds = 0.2f;
        public float reboundToParryOffsetSeconds = 0.1f;

        public float reboundBlendSeconds = 0.1f;

        [RuntimeHeader]

        public bool canAttack;
        public bool canParry;

        public bool isAttackActive;
        public bool isParryActive;
        public bool isFocusActive;

        public FencerCommand bufferedCommand;

        public FencerStance stance;

        [ContextMenu("Get Hit")]
        private void GetHidInEditMode()
        {
            if (Application.isPlaying)
                GetHit(stance, 1);
        }

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        #endregion

        private void Start()
        {
            stance = FencerStance.Thrust;
            ForceAllowAll();
            attributes.Reset();
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
                case FencerCommand.EnterFocus:
                    EnterFocus();
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

            if (IsCurrentAnimation(stance.ParryAnimationName()))
                CrossFade(stance.AttackAnimationName(), transitionSeconds: 0.2f, parryToAttackOffsetSeconds);
            else if (IsCurrentAnimationHit())
                CrossFade(stance.AttackAnimationName(), transitionSeconds: 0.2f, hitToAnyOffsetSeconds);
            else if (IsCurrentAnimation(stance.AttackAnimationName()))
                CrossFade(stance.AttackAnimationName(), transitionSeconds: 0.2f);
            else
                CrossFade(stance.AttackAnimationName());

            onAttack.Invoke();
        }

        public void Parry()
        {
            if (!canParry)
            {
                bufferedCommand = FencerCommand.Parry;
                return;
            }

            DisallowAll();

            if (IsCurrentAnimation(stance.AttackAnimationName()) && GetAttackBlend() > 0)
            {
                isParryActive = true;
                CrossFade(stance.ParryAnimationName(), transitionSeconds: 0.2f, reboundToParryOffsetSeconds);
            }
            else if (IsCurrentAnimationHit())
            {
                isParryActive = true;
                CrossFade(stance.ParryAnimationName(), transitionSeconds: 0.2f, 0);
            }
            else
            {
                CrossFade(stance.ParryAnimationName());
            }
        }

        public void EnterFocus()
        {
            if (isFocusActive)
                return;

            if (!canParry)
            {
                bufferedCommand = FencerCommand.EnterFocus;
                return;
            }

            isFocusActive = true;
            animator.SetBool("CanIdle", false);
            CrossFade("Focus", transitionSeconds: 0.4f);
        }

        public void ForceFocus()
        {
            ForceAllowAll();
            EnterFocus();
            DisallowAll();
        }

        public void ExitFocus()
        {
            if (!isFocusActive)
                return;

            isFocusActive = false;
            animator.SetBool("CanIdle", true);
            CrossFade("Idle", transitionSeconds: 0.2f);

            AllowAll();

            if (bufferedCommand == FencerCommand.EnterFocus)
                bufferedCommand = FencerCommand.None;
        }

        public void Rebound()
        {
            ResetStatus();
            SetAttackBlend(1, reboundBlendSeconds);

            onRebound.Invoke();
        }

        public void GetHit(FencerStance byStance, int damage)
        {
            if (attributes.healthPoints.Value <= 0)
                return;

            DisallowAll();
            ResetStatus();

            attributes.healthPoints.Value = Mathf.Min(attributes.healthPoints.Value - damage, attributes.maxHealthPoints.Value);

            if (attributes.healthPoints.Value > 0)
            {
                animator.SetBool("CanIdle", true);
                CrossFade(byStance.HitAnimationName());
            }
            else
            {
                CrossFade("Ko");
                animator.SetBool("CanIdle", false);
                enabled = false;
            }

            onHit.Invoke();
        }

        public void Revive()
        {
            animator.Play("Revive");
            animator.SetBool("CanIdle", true);
            ForceAllowAll();
            bufferedCommand = FencerCommand.None;
            enabled = true;
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
                ForceAllowAll();
        }

        public void ForceAllowAll()
        {
            canAttack = true;
            canParry = true;
            ResetStatus();
        }

        public void ResetStatus()
        {
            isAttackActive = false;
            isParryActive = false;
            isFocusActive = false;
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
                isAttackActive = true;
        }

        public void DeactivateAttack()
        {
            isAttackActive = false;
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

        public float GetAttackBlend()
        {
            return animator.GetFloat("AttackBlend");
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

        public float GetNextAnimatorNormalizedTime()
        {
            return animator.GetNextAnimatorStateInfo(0).normalizedTime;
        }

        public bool TryGetAttackNormalizedTime(out float normalizedTime, out bool isNextAnimation)
        {
            if (!IsCurrentOrNextAnimation(stance.AttackAnimationName()))
            {
                normalizedTime = 0;
                isNextAnimation = false;
                return false;
            }

            isNextAnimation = IsNextAnimation(stance.AttackAnimationName());

            if (isNextAnimation)
                normalizedTime = GetNextAnimatorNormalizedTime();
            else
                normalizedTime = GetCurrentAnimationNormalizedTime();

            return true;
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

        public bool IsCurrentAnimationHit()
        {
            return IsCurrentAnimation(FencerStance.Thrust.HitAnimationName());
        }

        public bool IsAttacking()
        {
            return isAttackActive || IsCurrentAnimation(stance.AttackAnimationName()) && GetCurrentAnimationNormalizedTime() <= 0.4f;
        }
    }
}