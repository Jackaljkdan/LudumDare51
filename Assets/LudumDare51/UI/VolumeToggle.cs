using JK.Injection;
using JK.Injection.PropertyDrawers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Toggle))]
    public class VolumeToggle : MonoBehaviour
    {
        #region Inspector

        [Injected]
        public Volume volume;

        #endregion

        private void Awake()
        {
            Context context = Context.Find(this);
            volume = context.Get<Volume>(this);
        }

        private void Start()
        {
            var toggle = GetComponent<Toggle>();
            toggle.isOn = volume.gameObject.activeSelf;
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            volume.gameObject.SetActive(value);
        }
    }
}