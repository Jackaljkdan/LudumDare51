using JK.Injection;
using JK.Injection.PropertyDrawers;
using LudumDare51.Rounds;
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

        [Injected]
        public RoundsManager roundsManager;

        #endregion

        protected bool interactable;

        private void Awake()
        {
            Context context = Context.Find(this);
            controller = context.Get<FencerController>(this, injectionId);
            roundsManager = context.Get<RoundsManager>(this);
        }

        protected virtual void Start()
        {
            roundsManager.onRoundEnd.AddListener(OnRoundEnd);
            roundsManager.onFight.AddListener(OnFight);

            OnRoundEnd();
        }

        private void OnDestroy()
        {
            if (roundsManager != null)
            {
                roundsManager.onRoundEnd.RemoveListener(OnRoundEnd);
                roundsManager.onFight.RemoveListener(OnFight);
            }
        }

        private void OnRoundEnd()
        {
            interactable = false;
        }

        private void OnFight()
        {
            interactable = true;
        }
    }
}