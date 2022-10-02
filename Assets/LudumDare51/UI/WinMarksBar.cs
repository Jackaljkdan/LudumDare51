using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using LudumDare51.Rounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class WinMarksBar : MonoBehaviour
    {
        #region Inspector

        public bool isPlayer;

        public WinMark markPrefab;

        [Injected]
        public RoundsManager roundsManager;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            roundsManager = context.Get<RoundsManager>(this);

            if (Application.isPlaying)
            {
                foreach (Transform child in transform)
                    Destroy(child.gameObject);

                transform.DetachChildren();
            }
        }

        private void Start()
        {
            if (isPlayer)
                roundsManager.playerWins.onChange.AddListener(OnWinsChanged);
            else
                roundsManager.aiWins.onChange.AddListener(OnWinsChanged);
        }

        private void OnWinsChanged(ObservableProperty<int>.Changed arg)
        {
            Instantiate(markPrefab, transform);
        }
    }
}