using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Utils;
using LudumDare51.Rounds;
using LudumDare51.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    public class ShockwaveTenSecondsEffect : TenSecondsEffect
    {
        #region Inspector

        public Shockwave prefab;

        [Injected]
        public RoundsManager roundsManager;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            roundsManager = context.Get<RoundsManager>(this);
        }

        public override void Apply(UnityAction doneCallback)
        {
            Instantiate(prefab, transform.root);
            doneCallback?.Invoke();
        }

        public override bool CanApply()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "Shockwave";
        }

        public override string GetInstructions()
        {
            return Tokens.Format("Hold down {Focus}!", roundsManager.isMultiplayer);
        }

        public override void Revert()
        {
        }
    }
}