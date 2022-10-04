using JK.PropertyDrawers;
using JK.Utils;
using LudumDare51.Fencer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Weapon
{
    [DisallowMultipleComponent]
    public class Sword : MonoBehaviour
    {
        #region Inspector

        public int damage = 1;

        public List<AudioClip> parryClips;
        public List<AudioClip> hitClips;

        [RuntimeHeader]

        [ReadOnly]
        public FencerController wielder;

        #endregion

        private void Start()
        {
            wielder = GetComponentInParent<FencerController>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!wielder.isAttackActive)
                return;

            if (!other.TryGetComponentInParent(out FencerController victim))
                return;

            if (victim.isParryActive)
            {
                wielder.Rebound();

                if (TryGetComponent(out AudioSource source))
                    source.PlayRandomClip(parryClips, oneShot: true);
            }
            else
            {
                victim.GetHit(wielder.stance, damage);

                if (TryGetComponent(out AudioSource source))
                    source.PlayRandomClip(hitClips, oneShot: true);
            }

            wielder.ResetStatus();
        }
    }
}