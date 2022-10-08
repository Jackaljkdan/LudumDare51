using JK.PropertyDrawers;
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

        [RuntimeHeader]

        public bool isDown;

        #endregion

        public void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            Update();
        }

        private void Update()
        {
            if (interactable && isDown)
                controller.EnterFocus();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
            controller.ExitFocus();
        }
    }
}