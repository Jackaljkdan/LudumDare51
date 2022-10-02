using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    public abstract class TenSecondsEffect : MonoBehaviour
    {
        #region Inspector



        #endregion

        public abstract string GetDescription();

        public abstract string GetInstructions();

        public abstract bool CanApply();

        public abstract void Apply(UnityAction doneCallback);

        public abstract void Revert();
    }
}