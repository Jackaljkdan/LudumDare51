using JK.Attributes;
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
    public class TenSecondsNextUpText : MonoBehaviour
    {
        #region Inspector

        [Injected]
        public TenSecondsManager tenSecondsManager;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            tenSecondsManager = context.Get<TenSecondsManager>(this);
        }

        private void Start()
        {
            Refresh();
            tenSecondsManager.nextDistraction.onChange.AddListener(OnNextDistractionChanged);
            tenSecondsManager.seconds.onChange.AddListener(OnSecondsChanged);
        }

        private void OnNextDistractionChanged(ObservableProperty<TenSecondsEffect>.Changed arg)
        {
            Refresh();
        }

        private void OnSecondsChanged(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == 0)
                GetComponent<Text>().text = string.Empty;
        }

        private void Refresh()
        {
            if (tenSecondsManager.nextDistraction.Value != null)
                GetComponent<Text>().text = "Next Up: " + tenSecondsManager.nextDistraction.Value.GetDescription();
            else
                GetComponent<Text>().text = string.Empty;
        }
    }
}