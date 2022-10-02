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
    public class UpsideDownTenSecondsEffect : TenSecondsEffect
    {
        #region Inspector

        [Injected]
        public new Camera camera;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            camera = context.Get<Camera>(this);
        }

        public override void Apply(UnityAction doneCallback)
        {
            camera.transform.DOMoveY(camera.transform.position.y - 1, 1);
            Vector3 euler = camera.transform.eulerAngles;
            euler.z = 180;
            camera.transform.DORotate(euler, 1);

            doneCallback?.Invoke();
        }

        public override bool CanApply()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "Upside Down";
        }

        public override string GetInstructions()
        {
            return null;
        }

        public override void Revert()
        {
            camera.transform.DOMoveY(camera.transform.position.y + 1, 1);
            Vector3 euler = camera.transform.eulerAngles;
            euler.z = 0;
            camera.transform.DORotate(euler, 1);
        }
    }
}