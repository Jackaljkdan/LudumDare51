using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Weapon
{
    [DisallowMultipleComponent]
    public class OneShotParticleSystem : MonoBehaviour
    {
        #region Inspector

        public ParticleSystem system;

        private void Reset()
        {
            system = GetComponent<ParticleSystem>();
        }

        #endregion

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Play()
        {
            gameObject.SetActive(true);
            system.Play();
        }

        public void Stop()
        {
            system.Stop();
            this.RunAfterSeconds(system.main.startLifetime.constant, () => gameObject.SetActive(false));
        }
    }
}