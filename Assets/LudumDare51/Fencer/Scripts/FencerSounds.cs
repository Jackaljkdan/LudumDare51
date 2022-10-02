using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [DisallowMultipleComponent]
    public class FencerSounds : MonoBehaviour
    {
        #region Inspector

        public FencerController controller;

        public AudioSource audioSource;

        public List<AudioClip> painClips;

        private void Reset()
        {
            controller = GetComponent<FencerController>();
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        private void Start()
        {
            controller.onHit.AddListener(OnHit);
        }

        private void OnHit()
        {
            audioSource.PlayRandomClip(painClips, oneShot: true);
        }
    }
}