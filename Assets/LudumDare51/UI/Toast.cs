using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class Toast : MonoBehaviour
    {
        #region Inspector

        public Text text;

        public CanvasGroup group;

        private void Reset()
        {
            text = GetComponentInChildren<Text>();
            group = GetComponent<CanvasGroup>();
        }

        #endregion

        private Sequence tween;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(string content, float seconds = 2)
        {
            text.text = content;

            gameObject.SetActive(true);
            tween?.Kill();
            CancelInvoke(nameof(Dismiss));

            group.alpha = 0;
            transform.localScale = Vector3Utils.Create(0.8f);

            tween = DOTween.Sequence();
            tween.Insert(0, group.DOFade(1, 0.25f));
            tween.Insert(0, transform.DOScale(1, 0.25f));

            Invoke(nameof(Dismiss), seconds);
        }

        public void Dismiss()
        {
            tween = DOTween.Sequence();
            tween.Insert(0, group.DOFade(0, 0.25f));
            tween.Insert(0, transform.DOScale(0.8f, 0.25f));

            tween.onComplete += () => gameObject.SetActive(false);
        }
    }
}