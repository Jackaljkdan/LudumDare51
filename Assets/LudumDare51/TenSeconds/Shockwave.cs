using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Utils;
using LudumDare51.Fencer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    public class Shockwave : MonoBehaviour
    {
        #region Inspector

        public AudioClip clip;

        [Injected]
        public FencerController player;

        [Injected]
        public FencerController ai;

        [Injected]
        public AudioSource soundsAudioSource;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);

            player = context.Get<FencerController>(this, "player");
            ai = context.Get<FencerController>(this, "ai");
            soundsAudioSource = context.Get<AudioSource>("sounds");
        }

        private void Start()
        {
            GetComponent<MeshRenderer>().material.DOFade(0, 0.5f);
            transform.DOScale(8, 0.5f).onComplete += () => Destroy(gameObject);

            soundsAudioSource.PlayOneShotSafely(clip);

            Invoke(nameof(Hit), 0.1f);
        }

        private void Hit()
        {
            if (!player.isFocusActive)
                player.GetHit(player.stance, 1);

            if (!ai.isFocusActive)
                ai.GetHit(ai.stance, 1);
        }
    }
}