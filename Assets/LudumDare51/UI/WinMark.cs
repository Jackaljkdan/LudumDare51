using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LudumDare51.UI
{
    [DisallowMultipleComponent]
    public class WinMark : MonoBehaviour
    {
        #region Inspector

        public Image image;

        private void Reset()
        {
            image = GetComponentInChildren<Image>();
        }

        #endregion

        private void OnEnable()
        {
            image.transform.localScale = new Vector3(3, 3, 3);
            image.transform.DOScale(1, 1);
        }

        private void OnDestroy()
        {
            DOTween.Kill(image.transform);
        }
    }
}