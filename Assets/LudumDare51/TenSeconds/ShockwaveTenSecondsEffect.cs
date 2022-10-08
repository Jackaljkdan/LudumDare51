using JK.Utils;
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

        #endregion

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
            return Tokens.Format("Hold down {Focus}!");
        }

        public override void Revert()
        {
        }
    }
}