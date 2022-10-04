using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    public class LightsOutTenSecondsEffect : TenSecondsEffect
    {
        #region Inspector

        [Injected]
        public Light mainLight;
        [Injected]
        public Light backLight;

        [Injected]
        public new Camera camera;

        [Injected]
        public AudioSource musicAudioSource;

        #endregion

        private Color cameraBackgroundColor;

        private Color ambientLightColor;

        private float musicVolume;

        private void Awake()
        {
            Context context = Context.Find(this);
            mainLight = context.Get<Light>(this, "main");
            backLight = context.Get<Light>(this, "back");
            camera = context.Get<Camera>(this);
            musicAudioSource = context.Get<AudioSource>(this, "music");
        }

        private void Start()
        {
            cameraBackgroundColor = camera.backgroundColor;
            ambientLightColor = RenderSettings.ambientLight;
            musicVolume = musicAudioSource.volume;
        }

        public override void Apply(UnityAction doneCallback)
        {
            camera.backgroundColor = Color.black;
            RenderSettings.ambientLight = Color.black;

            mainLight.enabled = false;
            backLight.enabled = false;

            musicAudioSource.DOFade(musicVolume / 4, 0.5f);

            doneCallback?.Invoke();
        }

        public override bool CanApply()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "Lights Out";
        }

        public override string GetInstructions()
        {
            return null;
        }

        public override void Revert()
        {
            camera.backgroundColor = cameraBackgroundColor;
            RenderSettings.ambientLight = ambientLightColor;

            mainLight.enabled = true;
            backLight.enabled = true;

            musicAudioSource.DOFade(musicVolume, 0.5f);
        }
    }
}