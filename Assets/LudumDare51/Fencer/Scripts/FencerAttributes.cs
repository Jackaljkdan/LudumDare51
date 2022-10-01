using JK.Observables;
using JK.PropertyDrawers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [Serializable]
    public class FencerAttributes
    {
        #region Inspector

        public ObservableProperty<int> maxHealthPoints = new ObservableProperty<int>(2);

        [RuntimeHeader]

        public ObservableProperty<int> healthPoints = new ObservableProperty<int>();

        #endregion

        public void Reset()
        {
            healthPoints.Value = maxHealthPoints.Value;
        }
    }
}