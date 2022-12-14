using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
using LudumDare51.Rounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class TenSecondsInstructionsText : MonoBehaviour
    {
        #region Inspector

        [Injected]
        public TenSecondsManager tenSecondsManager;

        [Injected]
        public RoundsManager roundsManager;

        #endregion

        private Tween tween;

        private void Awake()
        {
            Context context = Context.Find(this);
            tenSecondsManager = context.Get<TenSecondsManager>(this);
            roundsManager = context.Get<RoundsManager>(this);
        }

        private void Start()
        {
            tenSecondsManager.seconds.onChange.AddListener(OnSecondsChanged);
            roundsManager.onWinOrLose.AddListener(OnWinOrLose);
            GetComponent<Text>().text = string.Empty;
        }

        private void OnWinOrLose()
        {
            GetComponent<Text>().DOFade(0, 0.25f);
        }

        private void OnSecondsChanged(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == 3 && tenSecondsManager.nextDistraction.Value != null)
            {
                string instructions = tenSecondsManager.nextDistraction.Value.GetInstructions();

                if (!string.IsNullOrEmpty(instructions))
                {
                    GetComponent<Text>().text = instructions;
                    transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    tween = transform.DOScale(1.05f, 1).SetLoops(-1);
                }
            }
            else if (arg.updated == 0)
            {
                GetComponent<Text>().text = string.Empty;
                tween?.Kill();
            }
        }
    }
}