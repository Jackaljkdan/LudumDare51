using DG.Tweening;
using JK.Injection;
using JK.Injection.PropertyDrawers;
using JK.Observables;
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

        #endregion

        private Tween tween;

        private void Awake()
        {
            Context context = Context.Find(this);
            tenSecondsManager = context.Get<TenSecondsManager>(this);
        }

        private void Start()
        {
            tenSecondsManager.seconds.onChange.AddListener(OnSecondsChanged);
            GetComponent<Text>().text = string.Empty;
        }

        private void OnSecondsChanged(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == 3 && tenSecondsManager.nextDistraction.Value != null)
            {
                GetComponent<Text>().text = tenSecondsManager.nextDistraction.Value.GetInstructions();
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                tween = transform.DOScale(1.2f, 1).SetLoops(-1);
            }
            else if (arg.updated == 10)
            {
                GetComponent<Text>().text = string.Empty;
                tween?.Kill();
            }
        }
    }
}