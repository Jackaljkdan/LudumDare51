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

        #endregion

        private Color cameraBackgroundColor;

        private Color ambientLightColor;

        private void Awake()
        {
            Context context = Context.Find(this);
            mainLight = context.Get<Light>(this, "main");
            backLight = context.Get<Light>(this, "back");
            camera = context.Get<Camera>(this);
        }

        public override void Apply(UnityAction doneCallback)
        {
            cameraBackgroundColor = camera.backgroundColor;
            ambientLightColor = RenderSettings.ambientLight;

            camera.backgroundColor = Color.black;
            RenderSettings.ambientLight = Color.black;

            mainLight.enabled = false;
            backLight.enabled = false;

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
        }
    }
}