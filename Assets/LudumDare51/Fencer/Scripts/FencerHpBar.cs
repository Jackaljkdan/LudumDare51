using DG.Tweening;
using JK.Attributes;
using JK.Injection;
using JK.Observables;
using LudumDare51.Fencer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LudumDare51.Fencer
{
    [DisallowMultipleComponent]
    [LateExecutionOrder]
    public class FencerHpBar : MonoBehaviour
    {
        #region Inspector

        public Image fillImage;

        public string fencerInjectionId = "player";

        #endregion

        private FencerController fencer;

        private void Awake()
        {
            Context context = Context.Find(this);
            fencer = context.Get<FencerController>(fencerInjectionId);
        }

        private void Start()
        {
            fencer.attributes.healthPoints.onChange.AddListener(OnHealthChanged);
            fillImage.fillAmount = ((float)fencer.attributes.healthPoints.Value) / fencer.attributes.maxHealthPoints.Value;
        }

        private void OnHealthChanged(ObservableProperty<int>.Changed arg)
        {
            float value = ((float)fencer.attributes.healthPoints.Value) / fencer.attributes.maxHealthPoints.Value;
            fillImage.DOFillAmount(value, 0.25f);
        }
    }
}