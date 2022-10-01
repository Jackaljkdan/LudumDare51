using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LudumDare51.Rounds
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class RoundText : MonoBehaviour
    {
        #region Inspector

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
            roundsManager.round.onChange.AddListener(OnRoundChanged);
        }

        private void OnRoundChanged(ObservableProperty<int>.Changed arg)
        {
            GetComponent<Text>().text = $"Round {arg.updated}";

            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(1.5f, 0.5f));
            seq.Append(transform.DOScale(1, 0.5f));
        }
    }
}