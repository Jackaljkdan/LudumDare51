using JK.Injection;
using JK.Injection.PropertyDrawers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Rounds
{
    [DisallowMultipleComponent]
    public class RoundBasedEnabler : MonoBehaviour
    {
        #region Inspector

        public Behaviour target;

        [Injected]
        public RoundsManager roundsManager;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            roundsManager = context.Get<RoundsManager>(this);
        }

        private void Start()
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
            if (target != null)
                target.enabled = false;
        }

        private void OnFight()
        {
            if (target != null)
                target.enabled = true;
        }
    }
}