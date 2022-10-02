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
    public class TenSecondsText : MonoBehaviour
    {
        #region Inspector

        [Injected]
        public TenSecondsManager tenSecondsManager;

        #endregion

        private int standardFontSize;

        private void Awake()
        {
            Context context = Context.Find(this);
            tenSecondsManager = context.Get<TenSecondsManager>(this);
        }

        private void Start()
        {
            tenSecondsManager.seconds.onChange.AddListener(OnSecondsChanged);

            var text = GetComponent<Text>();
            standardFontSize = text.fontSize;
            text.text = "10";
        }

        private void OnSecondsChanged(ObservableProperty<int>.Changed arg)
        {
            if (arg.updated == 0)
            {
                GetComponent<Text>().text = string.Empty;
                return;
            }

            var text = GetComponent<Text>();
            text.text = arg.updated.ToString();
            text.fontSize = FontSizeForSeconds(arg.updated);

            if (arg.updated <= 3)
            {
                text.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                var seq = DOTween.Sequence();
                seq.Append(text.transform.DOScale(1.5f, 0.12f));
                seq.Append(text.transform.DOScale(1, 0.12f));
            }
        }

        private int FontSizeForSeconds(int seconds)
        {
            switch (seconds)
            {
                case 3:
                    return standardFontSize + 10;
                case 2:
                    return standardFontSize + 20;
                case 1:
                    return standardFontSize + 30;
                default:
                    return standardFontSize;
            }
        }
    }
}