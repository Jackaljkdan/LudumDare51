using JK.Injection;
using JK.Injection.PropertyDrawers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LudumDare51.Fencer.UI
{
    [DisallowMultipleComponent]
    public abstract class FencerActionButton : MonoBehaviour
    {
        #region Inspector

        public string injectionId = "player";

        [Injected]
        public FencerController controller;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            controller = context.Get<FencerController>(this, injectionId);
        }
    }
}