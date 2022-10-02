using JK.Injection;
using JK.Injection.PropertyDrawers;
using LudumDare51.Fencer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    public class InvisibilityTenSecondsEffect : TenSecondsEffect
    {
        #region Inspector

        [Injected]
        public FencerController player;

        [Injected]
        public FencerController ai;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            player = context.Get<FencerController>(this, "player");
            ai = context.Get<FencerController>(this, "ai");
        }

        public override void Apply(UnityAction doneCallback)
        {
            player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            ai.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

            doneCallback?.Invoke();
        }

        public override bool CanApply()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "Invisibility";
        }

        public override string GetInstructions()
        {
            return null;
        }

        public override void Revert()
        {
            player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            ai.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    }
}