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

        //[Injected]
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

        private void OnRoundEnd()
        {
            target.enabled = false;
        }

        private void OnFight()
        {
            target.enabled = true;
        }
    }
}