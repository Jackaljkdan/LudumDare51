using JK.PropertyDrawers;
using JK.Utils;
using LudumDare51.Fencer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Sword
{
    [DisallowMultipleComponent]
    public class Sword : MonoBehaviour
    {
        #region Inspector

        [RuntimeHeader]

        [ReadOnly]
        public FencerController wielder;

        public bool alreadyHit;

        #endregion

        private void Start()
        {
            wielder = GetComponentInParent<FencerController>();

            wielder.onAttack.AddListener(OnAttack);
        }

        private void OnAttack()
        {
            alreadyHit = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (alreadyHit)
                return;

            if (!wielder.isAttackActive)
                return;

            if (!other.TryGetComponentInParent(out FencerController victim))
                return;

            if (victim.isParryActive)
                wielder.Rebound();
            else
                victim.GetHit(wielder.stance);

            alreadyHit = true;
        }
    }
}