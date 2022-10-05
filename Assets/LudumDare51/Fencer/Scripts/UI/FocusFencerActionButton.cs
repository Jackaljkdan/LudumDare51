using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LudumDare51.Fencer.UI
{
    [DisallowMultipleComponent]
    public class FocusFencerActionButton : FencerActionButton, IPointerDownHandler, IPointerUpHandler
    {
        #region Inspector



        #endregion

        private void Start()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (enabled)
                controller.EnterFocus();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (enabled)
                controller.ExitFocus();
        }
    }
}