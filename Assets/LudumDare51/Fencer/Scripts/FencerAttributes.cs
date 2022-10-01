using JK.Observables;
using JK.PropertyDrawers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [DisallowMultipleComponent]
    public class FencerAttributes : MonoBehaviour
    {
        #region Inspector

        public ObservableProperty<int> maxHealthPoints = new ObservableProperty<int>(2);

        public FencerController controller;

        [RuntimeHeader]

        public ObservableProperty<int> healthPoints = new ObservableProperty<int>();

        private void Reset()
        {
            controller = GetComponent<FencerController>();
        }

        #endregion

        private void Start()
        {
            ResetAttributes();
            controller.onHit.AddListener(OnHit);
        }

        public void ResetAttributes()
        {
            healthPoints.Value = maxHealthPoints.Value;
        }

        private void OnHit()
        {
            healthPoints.Value--;
        }
    }
}